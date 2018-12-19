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
    public class ProfileController : Controller
    {
        UserManager<User> _userManager;
        IConfiguration _configuration;

        public ProfileController(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            Task<User> u = _userManager.GetUserAsync(HttpContext.User);
            User user = await u;
            if (user == null)
            {
                return NotFound();
            }

            var IdStation = "";
            WebRequest request = WebRequest.Create(_configuration.GetValue<string>("BackendUrl") + "api/profile/" + user.Id);
            request.Method = "Get";

            using (var s = request.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    IdStation = sr.ReadToEnd();
                }
            }

            ProfileViewModel model = new ProfileViewModel { Id = user.Id, Email = user.Email, Year = user.Year, IdStation = IdStation };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var _passwordValidator =
                       HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
                    IdentityResult result =
                            await _passwordValidator.ValidateAsync(_userManager, user, model.Password);
                    if (result.Succeeded)
                    {
                        user.Email = model.Email;
                        user.UserName = model.Email;
                        user.Year = model.Year;
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

                        var result2 = await _userManager.UpdateAsync(user);
                        if (result2.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View("Index");
        }
    }
}