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
    [Route("api/station")]
    [ApiController]
    public class StationController : ControllerBase
    {
        string connString;
        public StationController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("DynamicEquipmentSystemDatabase");
        }

        [HttpGet("{senderId}")]
        public dynamic GetUsersStation(string senderId)
        {
            dynamic station = "";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                var commandText = "SELECT s.IdStation FROM Station as s WHERE s.IdUser = @sender";
                using (SqlCommand command = new SqlCommand(commandText))
                {
                    command.Connection = connection;
                    command.Parameters.Add("@sender", SqlDbType.VarChar, 100).Value = senderId;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            station = new
                            {
                                IdStation = reader.GetString(0)
                            };
                        }
                    }

                    connection.Close();
                }
            }

            return station;
        }
    }
}