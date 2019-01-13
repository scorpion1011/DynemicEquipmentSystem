﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DynamicEquipmentSystem.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BackendEquipmentSystem.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        string connString;
        public UserController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("DynamicEquipmentSystemDatabase");
        }

        [HttpGet("{Email}/{Password}")]
        public dynamic GetUserId(string Email, string Password)
        {
            string userId = "";
            string userHash = "";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = "select Id, PasswordHash from AspNetUsers where Email = @email";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@email", System.Data.SqlDbType.NVarChar, 256).Value = Email;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = reader.GetString(0);
                            userHash = reader.GetString(1);
                        }
                    }

                    connection.Close();
                }
            }

            
            var response = VerifyHashedPassword(userHash, Password);

            if (response == PasswordVerificationResult.Success)
            {
                return new { IdUser = userId };
            }
            else
            {
                return new { IdUser = "There is no such user" };
            }
        }


        private RandomNumberGenerator Myrng;
        private int _iterCount;
        private PasswordHasherCompatibilityMode _compatibilityMode;

        public virtual PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }
            if (providedPassword == null)
            {
                throw new ArgumentNullException(nameof(providedPassword));
            }

            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            // read the format marker from the hashed password
            if (decodedHashedPassword.Length == 0)
            {
                return PasswordVerificationResult.Failed;
            }
            switch (decodedHashedPassword[0])
            {
                case 0x00:
                    if (VerifyHashedPasswordV2(decodedHashedPassword, providedPassword))
                    {
                        // This is an old password hash format - the caller needs to rehash if we're not running in an older compat mode.
                        return (_compatibilityMode == PasswordHasherCompatibilityMode.IdentityV3)
                            ? PasswordVerificationResult.SuccessRehashNeeded
                            : PasswordVerificationResult.Success;
                    }
                    else
                    {
                        return PasswordVerificationResult.Failed;
                    }

                case 0x01:
                    int embeddedIterCount;
                    if (VerifyHashedPasswordV3(decodedHashedPassword, providedPassword, out embeddedIterCount))
                    {
                        // If this hasher was configured with a higher iteration count, change the entry now.
                        return (embeddedIterCount < _iterCount)
                            ? PasswordVerificationResult.SuccessRehashNeeded
                            : PasswordVerificationResult.Success;
                    }
                    else
                    {
                        return PasswordVerificationResult.Failed;
                    }

                default:
                    return PasswordVerificationResult.Failed; // unknown format marker
            }
        }


        private bool VerifyHashedPasswordV2(byte[] hashedPassword, string password)
        {
            const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA1; // default for Rfc2898DeriveBytes
            const int Pbkdf2IterCount = 1000; // default for Rfc2898DeriveBytes
            const int Pbkdf2SubkeyLength = 256 / 8; // 256 bits
            const int SaltSize = 128 / 8; // 128 bits

            // We know ahead of time the exact length of a valid hashed password payload.
            if (hashedPassword.Length != 1 + SaltSize + Pbkdf2SubkeyLength)
            {
                return false; // bad size
            }

            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPassword, 1, salt, 0, salt.Length);

            byte[] expectedSubkey = new byte[Pbkdf2SubkeyLength];
            Buffer.BlockCopy(hashedPassword, 1 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            // Hash the incoming password and verify it
            byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);
            return ByteArraysEqual(actualSubkey, expectedSubkey);
        }

        private bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }

        private bool VerifyHashedPasswordV3(byte[] hashedPassword, string password, out int iterCount)
        {
            iterCount = default(int);

            try
            {
                // Read header information
                KeyDerivationPrf prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
                iterCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
                int saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

                // Read the salt: must be >= 128 bits
                if (saltLength < 128 / 8)
                {
                    return false;
                }
                byte[] salt = new byte[saltLength];
                Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

                // Read the subkey (the rest of the payload): must be >= 128 bits
                int subkeyLength = hashedPassword.Length - 13 - salt.Length;
                if (subkeyLength < 128 / 8)
                {
                    return false;
                }
                byte[] expectedSubkey = new byte[subkeyLength];
                Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

                // Hash the incoming password and verify it
                byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subkeyLength);
                return ByteArraysEqual(actualSubkey, expectedSubkey);
            }
            catch
            {
                // This should never occur except in the case of a malformed payload, where
                // we might go off the end of the array. Regardless, a malformed payload
                // implies verification failed.
                return false;
            }
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)(buffer[offset + 0]) << 24)
                | ((uint)(buffer[offset + 1]) << 16)
                | ((uint)(buffer[offset + 2]) << 8)
                | ((uint)(buffer[offset + 3]));
        }

        //public string MyHashPassword(string password)
        //{
        //    Myrng = RandomNumberGenerator.Create();
        //    _iterCount = 10000;
        //    _compatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
        //    if (password == null)
        //    {
        //        throw new ArgumentNullException(nameof(password));
        //    }

        //    if (_compatibilityMode == PasswordHasherCompatibilityMode.IdentityV2)
        //    {
        //        return Convert.ToBase64String(HashPasswordV2(password, Myrng));
        //    }
        //    else
        //    {
        //        return Convert.ToBase64String(HashPasswordV3(password, Myrng));
        //    }
        //}

        //private byte[] HashPasswordV2(string password, RandomNumberGenerator rng)
        //{
        //    const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA1; // default for Rfc2898DeriveBytes
        //    const int Pbkdf2IterCount = 1000; // default for Rfc2898DeriveBytes
        //    const int Pbkdf2SubkeyLength = 256 / 8; // 256 bits
        //    const int SaltSize = 128 / 8; // 128 bits

        //    // Produce a version 2 (see comment above) text hash.
        //    byte[] salt = new byte[SaltSize];
        //    rng.GetBytes(salt);
        //    byte[] subkey = KeyDerivation.Pbkdf2(password, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);

        //    var outputBytes = new byte[1 + SaltSize + Pbkdf2SubkeyLength];
        //    outputBytes[0] = 0x00; // format marker
        //    Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
        //    Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, Pbkdf2SubkeyLength);
        //    return outputBytes;
        //}

        //private byte[] HashPasswordV3(string password, RandomNumberGenerator rng)
        //{
        //    return HashPasswordV3(password, rng,
        //        prf: KeyDerivationPrf.HMACSHA256,
        //        iterCount: _iterCount,
        //        saltSize: 128 / 8,
        //        numBytesRequested: 256 / 8);
        //}

        //private static byte[] HashPasswordV3(string password, RandomNumberGenerator rng, KeyDerivationPrf prf, int iterCount, int saltSize, int numBytesRequested)
        //{
        //    // Produce a version 3 (see comment above) text hash.
        //    byte[] salt = new byte[saltSize];
        //    rng.GetBytes(salt);
        //    byte[] subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

        //    var outputBytes = new byte[13 + salt.Length + subkey.Length];
        //    outputBytes[0] = 0x01; // format marker
        //    WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
        //    WriteNetworkByteOrder(outputBytes, 5, (uint)iterCount);
        //    WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
        //    Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
        //    Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);
        //    return outputBytes;
        //}

        //private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        //{
        //    buffer[offset + 0] = (byte)(value >> 24);
        //    buffer[offset + 1] = (byte)(value >> 16);
        //    buffer[offset + 2] = (byte)(value >> 8);
        //    buffer[offset + 3] = (byte)(value >> 0);
        //}
    }
}