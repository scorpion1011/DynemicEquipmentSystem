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
        static int lastClickedListId;

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

        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;

            ViewBag.UsersLists = await GetUsersListsAsync();
            ViewBag.UserId = userId;
            return View();
        }

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

        public IActionResult Owners() => View();

        [HttpPost]
        public IActionResult Owners(int id)
        {
            lastClickedListId = id;
            ListOfFriendsOwnersViewModel model = new ListOfFriendsOwnersViewModel();
            List<FriendOwnerModelView> models;
            WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/owners/" + id);
            request.Method = "Get";

            using (var s = request.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var contributorsAsJson = sr.ReadToEnd();
                    models = JsonConvert.DeserializeObject<List<FriendOwnerModelView>>(contributorsAsJson);
                }
            }
            foreach (var m in models)
            {
                model.List.Add(m);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> OwnersAsync(List<string> ownersId)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                var userId = user?.Id;
                WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/owners/" + lastClickedListId);
                request.Method = "Delete";
                request.GetResponse();

                request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/owners/" + lastClickedListId + "/" + userId);
                request.Method = "Post";
                request.GetResponse();

                foreach (string ownerId in ownersId)
                {
                    request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/owners/" + lastClickedListId + "/" + ownerId);
                    request.Method = "Post";
                    request.GetResponse();
                }

                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}