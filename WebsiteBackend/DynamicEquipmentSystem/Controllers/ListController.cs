using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DynamicEquipmentSystem.Models;
using DynamicEquipmentSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DynamicEquipmentSystem.Controllers
{
    public class ListController : Controller
    {
        UserManager<User> _userManager;
        IConfiguration _configuration;

        public ListController(IConfiguration configuration, UserManager<User> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        private async Task<List<List>> GetUsersListsAsync()
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;
            var lists = new List<List>();

            WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/list/" + userId);
            request.Method = "Get";

            using (var s = request.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var contributorsAsJson = sr.ReadToEnd();
                    lists = JsonConvert.DeserializeObject<List<List>>(contributorsAsJson);
                }
            }
            return lists;
        }

        public async Task<IActionResult> Index() => View(await GetUsersListsAsync());

        public IActionResult AddList() => View();

        [HttpPost]
        public async Task<IActionResult> AddList(AddListViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                var userId = user?.Id;

                WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/list/" + userId + "/" + model.Name);
                request.Method = "Post";
                request.GetResponse();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteList(int id)
        {
            WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/list/" + id);
            request.Method = "Delete";
            request.GetResponse();

            return RedirectToAction("Index");
        }
    }
}