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
    [Route("api/list")]
    [ApiController]
    public class ListController : ControllerBase
    {
        string connString;
        public ListController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("DynamicEquipmentSystemDatabase");
        }

        [HttpGet("{senderId}")]
        public List<dynamic> GetUsersLists(string senderId)
        {
            var lists = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = "select l.IdList, l.Name, l.isActive, l.idOwner from List as l, Owners as o where l.IdList = o.IdList and o.IdUser = @sender";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@sender", SqlDbType.VarChar, 100).Value = senderId;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lists.Add(new
                            {
                                IdList = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                isActive = reader.GetBoolean(2),
                                idOwner = reader.GetString(3)
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return lists;
        }

        [HttpPost("{senderId}/{name}")]
        public void AddList(string senderId, string name)
        {
            int newId;
            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = "INSERT INTO List (Name, isActive, idOwner) VALUES (@name, 'true', @userId) SELECT @@IDENTITY";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@name", SqlDbType.VarChar, 100).Value = name;
                    command.Parameters.Add("@userId", SqlDbType.VarChar, 100).Value = senderId;
                    connection.Open();
                    newId = Convert.ToInt32(command.ExecuteScalar());
                }
                commandText = "INSERT INTO Owners (IdUser, IdList) VALUES (@idUser, @idList)";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@idUser", SqlDbType.VarChar, 100).Value = senderId;
                    command.Parameters.Add("@idList", SqlDbType.Int, 100).Value = newId;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        [HttpDelete("{IdList}")]
        public void DeleteMark(int IdList)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = "DELETE FROM List WHERE IdList = @idList";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@idList", SqlDbType.Int, 100).Value = IdList;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}