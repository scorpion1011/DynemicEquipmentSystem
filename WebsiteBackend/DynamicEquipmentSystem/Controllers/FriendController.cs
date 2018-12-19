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

        private async Task<List<Friend>> GetFriendsListAsync()
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;
            var friendList = new List<Friend>();

            WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/friends/" + userId);
            request.Method = "Get";

            using (var s = request.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var contributorsAsJson = sr.ReadToEnd();
                    friendList = JsonConvert.DeserializeObject<List<Friend>>(contributorsAsJson);
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

                    WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/friends/" + userId + "/" + result.Id);
                    request.Method = "Post";
                    request.GetResponse();

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User with such mail not found");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;

            WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/friends/" + id + "/" + userId);
            request.Method = "Delete";
            request.GetResponse();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFriendRequest(string id)
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;

            WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/friends/" + id + "/" + userId);
            request.Method = "Put";
            request.GetResponse();

            return RedirectToAction("Index");
        }
    }
}