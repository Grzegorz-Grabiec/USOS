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
                    return RedirectToAction("Index", "Home");
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


        [HttpGet]
      //  public IActionResult Rejestruj()
       // {
       //     ViewData["Message"] = "Your application description page.";

        //    return View();
       // }
        [HttpPost]
        public async Task<IActionResult> Rejestruj(RegisterModel vm)
        {
            ViewData["Message"] = "Your application description page.";
            if (ModelState.IsValid)
          {                   
                 
            
                var user = new AppUser { UserName = vm.Login };
                var result = await _userManager.CreateAsync(user, vm.Password);
                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
               
                }
               
            }
            return View(vm);
        }


        [HttpPost]
        public ActionResult Verify(Account acc)
        {
            string connStr = configuration.GetConnectionString("MyConnStr");
            con = new SqlConnection(connStr);
            com = new SqlCommand();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from Users where Login='" + acc.Login + "' and Password='" + acc.Password + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                con.Close();

                return View("Akitek");
            }
            else
            {
                con.Close();
                return View("Login");
            }

        }
    }
}