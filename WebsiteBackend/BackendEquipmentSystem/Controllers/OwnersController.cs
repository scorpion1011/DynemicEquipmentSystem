using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BackendEquipmentSystem.Controllers
{
    [Route("api/owners")]
    [ApiController]
    public class OwnersController : Controller
    {
        string connString;
        public OwnersController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("DynamicEquipmentSystemDatabase");
        }

        [HttpGet("{listId}")]
        public List<dynamic> GetOwners(int listId)
        {
            var owners = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = @"select friendId, (select UserName from AspNetUsers where Id = friendId), case WHEN IdOwners is null THEN CAST(0 AS bit)  
                                    ELSE CAST(1 AS bit) end as isOwner from(select IdReceiver as friendId from AspNetFriend where IdSender = (select idOwner from List where IdList = @listId) and IsAccepted = 'true' union
                                    select IdSender from AspNetFriend where IdReceiver = (select idOwner from List where IdList = @listId) and IsAccepted = 'true') as f
                                    left join Owners as o on(f.friendId = o.IdUser and o.IdList = @listId)";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@listId", SqlDbType.Int, 100).Value = listId;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            owners.Add(new
                            {
                                friendId = reader.GetString(0),
                                friendName = reader.GetString(1),
                                isOwner = reader.GetBoolean(2)
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return owners;
        }

        [HttpDelete("{listId}")]
        public void ClearAllListOwners(int listId)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = @"delete from Owners where IdList = @listId";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@listId", SqlDbType.Int, 100).Value = listId;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        [HttpPost("{listId}/{ownerId}")]
        public void SetListOwners(int listId, string ownerId)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = "INSERT INTO Owners (IdUser, IdList) VALUES (@ownerId, @listId)";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@ownerId", SqlDbType.VarChar, 100).Value = ownerId;
                    command.Parameters.Add("@listId", SqlDbType.Int, 100).Value = listId;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}