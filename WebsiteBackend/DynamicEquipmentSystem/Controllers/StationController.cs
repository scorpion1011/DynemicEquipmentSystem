using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DynamicEquipmentSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DynamicEquipmentSystem.Controllers
{
    public class StationController : Controller
    {
        UserManager<User> _userManager;
        IConfiguration _configuration;

        public StationController(IConfiguration configuration, UserManager<User> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<IActionResult> Index() => View(await GetUsersStationAsync());

        private async Task<Station> GetUsersStationAsync()
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;
            var station = new Station();

            WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/station/" + userId);
            request.Method = "Get";

            using (var s = request.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var contributorsAsJson = sr.ReadToEnd();
                    station = JsonConvert.DeserializeObject<Station>(contributorsAsJson);
                }
            }


            return station;
        }
    }
}