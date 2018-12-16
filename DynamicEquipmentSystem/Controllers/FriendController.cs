using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using DynamicEquipmentSystem.Models;
using DynamicEquipmentSystem.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;

namespace DynamicEquipmentSystem.Controllers
{
    public class FriendController : Controller
    {

        UserManager<User> _userManager;
        IConfiguration _configuration;

        public FriendController(IConfiguration configuration, UserManager<User> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        private async Task <List<User>> GetFriendsListAsync()
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;
            var friendList = new List<User>();

            WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/friends/" + userId);
            request.Method = "Get";

            using (var s = request.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var contributorsAsJson = sr.ReadToEnd();
                    friendList = JsonConvert.DeserializeObject<List<User>>(contributorsAsJson);
                }
            }


            return friendList;
        }
        
        public async Task<IActionResult> Index() => View(await GetFriendsListAsync());

        public IActionResult AddFriend() => View();

        [HttpPost]
        public async Task<IActionResult> AddFriend(AddFriendViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.FindByEmailAsync(model.Email);
                if (result != null)
                {
                    var user = await GetCurrentUserAsync();
                    var userId = user?.Id;

                    string connectionString = "Server=DESKTOP-AQQLHQR\\SQLEXPRESS;Database=DynamicEquipmentSystem;Trusted_Connection=True;MultipleActiveResultSets=true";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        var commandText = "INSERT INTO AspNetFriend (IdSender, IdReceiver, IsAccepted) VALUES (@sender, @reciever, 0)";
                        using (SqlCommand command = new SqlCommand(commandText))
                        {
                            command.Connection = connection;
                            command.Parameters.Add("@sender", SqlDbType.VarChar, 100).Value = userId;
                            command.Parameters.Add("@reciever", SqlDbType.VarChar, 100).Value = result.Id;
                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User with such mail not found");
                }
            }
            return View(model);

        }


    }
}