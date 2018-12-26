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
    public class MarkController : Controller
    {
        UserManager<User> _userManager;
        IConfiguration _configuration;

        public MarkController(IConfiguration configuration, UserManager<User> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        private async Task<List<Mark>> GetMarkListAsync()
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;
            var markList = new List<Mark>();

            WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/mark/" + userId);
            request.Method = "Get";

            using (var s = request.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var contributorsAsJson = sr.ReadToEnd();
                    markList = JsonConvert.DeserializeObject<List<Mark>>(contributorsAsJson);
                }
            }
            return markList;
        }

        public async Task<IActionResult> Index() => View(await GetMarkListAsync());

        public async Task<IActionResult> AddMark() => View(await GetMarkListAsync());
        
        [HttpPost]
        public IActionResult SetMarkOn(int id)
        {
            WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/mark/" + id);
            request.Method = "Put";
            request.GetResponse();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditMark(int id)
        {
            Mark mark;
            WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/mark/info/" + id);
            request.Method = "Get";
            using (var s = request.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var contributorsAsJson = sr.ReadToEnd();
                    mark = JsonConvert.DeserializeObject<Mark>(contributorsAsJson);
                }
            }

            MarkViewModel model = new MarkViewModel { IdMark = id, Name = mark.Name, IsActive = mark.IsActive };

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateMark(MarkViewModel model)
        {
            WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/mark/" + model.IdMark + "/" + model.Name + "/" + model.IsActive);
            request.Method = "Put";
            request.GetResponse();

            return RedirectToAction("Index");
        }

    }
}