using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using USOS.Models;

namespace USOS.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration configuration;

        public AdminController(IConfiguration config, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.configuration = config;
        }
        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Index()
        {
            DbContextOptionsBuilder<USOSContext> options = new DbContextOptionsBuilder<USOSContext>();
            options.UseSqlServer(configuration.GetConnectionString("MyConnStr"));
            if (!User.IsInRole("Admin"))
                return RedirectToAction("Index", "Home");

            var userRoles = new List<AdminUsersView>();
            var context = new USOSContext(options.Options);
            var userStore = new UserStore<AppUser>(context);

            //Get all the usernames
            foreach (var user in userStore.Users)
            {
                var r = new AdminUsersView
                {
                    UserName = user.UserName
                    //Role = _userManager.GetRolesAsync(user).ToString()
                };
                var roles = _userManager.GetRolesAsync(user).Result;
                string roleStr = "";
                foreach (var role in roles)
                {
                    if (roleStr == "")
                        roleStr += role;
                    else
                        roleStr += "," + role;
                }
                r.Role = roleStr;
                userRoles.Add(r);
            }
 
            return View(userRoles);
        }
    }
}