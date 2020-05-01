using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using USOS.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace USOS.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration configuration;
        SqlConnection con;
        SqlCommand com;
        SqlDataReader dr;

        public LoginController(IConfiguration config,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.configuration = config;
        }

        public IActionResult Index()
        {
            return View();
        }
       
        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = vm.Login };
                var result = await _signInManager.PasswordSignInAsync(vm.Login, vm.Password,vm.RememberMe,false);

                if (result.Succeeded)
                {
                    TempData["status"] = "Success";
                    ViewBag.Message = "Login";
                    return RedirectToAction("Index", "Home",ViewBag.Message);
                }
                else
                {
                    ModelState.AddModelError("", "Zły login");
                    return View(vm);

                }

            }
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


     


    }
}