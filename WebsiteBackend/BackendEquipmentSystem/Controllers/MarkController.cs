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
    [Route("api/mark")]
    [ApiController]
    public class MarkController : Controller
    {
        string connString;
        public MarkController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("DynamicEquipmentSystemDatabase");
        }

        [HttpGet("{senderId}")]
        public List<dynamic> GetAllUserMarks(string senderId)
        {
            var markList = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = "select distinct m.IdMark, m.Name, m.IdStation, m.IsActive, m.IsGotten from Mark as m, Station as s where m.IdStation = s.IdStation and s.IdUser = @sender";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@sender", SqlDbType.VarChar, 100).Value = senderId;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            markList.Add(new
                            {
                                idMark = reader.GetInt32(0),
                                name = reader.GetString(1),
                                idStation = reader.GetString(2),
                                IsActive = reader.GetBoolean(3),
                                IsGotten = reader.GetBoolean(4)
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return markList;
        }

        [HttpGet("info/{IdMark}")]
        public dynamic GetMarkInfo(int IdMark)
        {
            dynamic markinfo = "";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = "SELECT * FROM Mark WHERE IdMark = @IdMark";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@IdMark", SqlDbType.Int, 100).Value = IdMark;
                    connection.Open();
                    command.ExecuteNonQuery();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            markinfo = new
                            {
                                idMark = reader.GetInt32(0),
                                name = reader.GetString(1),
                                idStation = reader.GetString(2),
                                IsActive = reader.GetBoolean(3),
                                IsGotten = reader.GetBoolean(4)
                            };
                        }
                    }

                    connection.Close();
                }
            }
            return markinfo;
        }

        [HttpPut("{IdMark}")]
        public void UpdateMark(int IdMark)
        {

            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = "UPDATE Mark SET IsActive = 'true' WHERE IdMark = @IdMark";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@IdMark", SqlDbType.Int, 100).Value = IdMark;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        [HttpDelete("{IdMark}")]
        public void ChangeName(int IdMark)
        {

            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = "DELETE Mark WHERE IdMark = @IdMark";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@IdMark", SqlDbType.Int, 100).Value = IdMark;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        [HttpPost("found")]
        public IActionResult Found([FromBody] dynamic value)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {

                var commandText = "SELECT IdMark FROM Mark WHERE IdMark = @IdMark";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@IdMark", SqlDbType.Int, 100).Value = Int32.Parse(value.mark.Value);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            using (SqlCommand command2 = new SqlCommand("UPDATE Mark SET IsGotten = 'true' WHERE IdMark = @IdMark"))
                            {
                                command2.Connection = connection;
                                command2.Parameters.Add("@IdMark", SqlDbType.Int, 100).Value = Int32.Parse(value.mark.Value);
                                command2.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            using (SqlCommand command1 = new SqlCommand("INSERT INTO Mark (IdMark, Name, IdStation, IsActive, IsGotten) VALUES (@IdMark, @IdMark, @idStation, 'false', 'true')"))
                            {
                                command1.Connection = connection;
                                command1.Parameters.Add("@IdMark", SqlDbType.Int, 100).Value = Int32.Parse(value.mark.Value);
                                command1.Parameters.Add("@idStation", SqlDbType.VarChar, 100).Value = value.station.Value;
                                command1.ExecuteNonQuery();
                            }
                        }
                    }

                    connection.Close();
                }

            }

            return NoContent();
        }

        [HttpPost("lost")]
        public IActionResult Lost([FromBody] dynamic value)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = "SELECT IdMark FROM Mark WHERE IdMark = @IdMark and IsActive = 'true'";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@IdMark", SqlDbType.Int, 100).Value = Int32.Parse(value.mark.Value);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            using (SqlCommand command2 = new SqlCommand("UPDATE Mark SET IsGotten = 'false' WHERE IdMark = @IdMark"))
                            {
                                command2.Connection = connection;
                                command2.Parameters.Add("@IdMark", SqlDbType.Int, 100).Value = Int32.Parse(value.mark.Value);
                                command2.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            using (SqlCommand command1 = new SqlCommand("DELETE FROM Mark where IdMark = @IdMark"))
                            {
                                command1.Connection = connection;
                                command1.Parameters.Add("@IdMark", SqlDbType.Int, 100).Value = Int32.Parse(value.mark.Value);
                                command1.ExecuteNonQuery();
                            }
                        }
                    }

                    connection.Close();
                }

            }

            return NoContent();
        }
    }
}