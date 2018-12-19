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

        [HttpPut("{IdMarker}")]
        public void SetMarkOn(int idMarker)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = "UPDATE Mark SET IsActive = 'true' WHERE IdMarker = @idMarker";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@IdMarker", SqlDbType.Int, 100).Value = idMarker;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}