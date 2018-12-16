using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BackendEquipmentSystem.Models;
using DynamicEquipmentSystem.Models;
using DynamicEquipmentSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BackendEquipmentSystem.Controllers
{
    [Route("api/friends")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        string connString;
        public FriendController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("DynamicEquipmentSystemDatabase");
        }

        [HttpGet("{senderId}")]
        public List<dynamic> GetAllUserFriends(string senderId)
        {
            var friendList = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = "select u.Id, u.Email, u.Year from AspNetUsers as u, AspNetFriend as f where f.IdSender = @sender and f.IdReceiver = u.Id and IsAccepted = '1'";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@sender", SqlDbType.VarChar, 100).Value = senderId;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            friendList.Add(new
                            {
                                id = reader.GetString(0),
                                email = reader.GetString(1),
                                year = reader.GetInt32(2)
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return friendList;
        }

        [HttpPost("{senderId}/{recieverId}")]
        public string AddFriend(string senderId, string recieverId)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = "INSERT INTO AspNetFriend (IdSender, IdReceiver, IsAccepted) VALUES (@sender, @reciever, 0)";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@sender", SqlDbType.VarChar, 100).Value = senderId;
                    command.Parameters.Add("@reciever", SqlDbType.VarChar, 100).Value = recieverId;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return "nice";
        }
    }
}