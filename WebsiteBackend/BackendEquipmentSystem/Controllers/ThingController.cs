using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BackendEquipmentSystem.Controllers
{
    [Route("api/thing")]
    [ApiController]
    public class ThingController : ControllerBase
    {
        string connString;
        public ThingController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("DynamicEquipmentSystemDatabase");
        }

        [HttpGet("{idList}")]
        public List<dynamic> GetThings(int idList)
        {
            var thingsList = new List<dynamic>();

            //using (SqlConnection connection = new SqlConnection(connString))
            //{
            //    var commandText = "select Id, PasswordHash from AspNetUsers where Email = @email";
            //    using (SqlCommand command = new SqlCommand(commandText))
            //    {
            //        command.Connection = connection;
            //        command.Parameters.Add("@email", System.Data.SqlDbType.NVarChar, 256).Value = Email;
            //        connection.Open();

            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            if (reader.Read())
            //            {
            //                userId = reader.GetString(0);
            //                userHash = reader.GetString(1);
            //            }
            //        }

            //        connection.Close();
            //    }
            //}

            thingsList.Add(new { idThing = "0", name = "Milk", isActive = "true" });
            thingsList.Add(new { idThing = "1", name = "Meat", isActive = "false" });
            thingsList.Add(new { idThing = "2", name = "Skillet", isActive = "false" });
            thingsList.Add(new { idThing = "3", name = "Ball", isActive = "true" });

            return thingsList;
        }
    }
}