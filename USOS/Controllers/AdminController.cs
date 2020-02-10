using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using USOS.Models;

namespace USOS.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration configuration;
        private readonly IServiceProvider _provider;
        public AdminController(IConfiguration config, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IServiceProvider provider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.configuration = config;
            _provider = provider;
        }

        public IActionResult Edit(string userName)
        {
            AppUser editUser = _userManager.FindByNameAsync(userName).Result;
            var userEdit = new AdminUsersView();
            userEdit.PhoneNumber    = editUser.PhoneNumber;
            userEdit.Email          = editUser.Email;
            userEdit.UserName       = editUser.UserName;

            userEdit.Roles = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Administrator", Value = "Admin",Selected = _userManager.IsInRoleAsync(editUser, "Admin").Result},
                new SelectListItem {Text = "Użytkownik", Value = "User",Selected = _userManager.IsInRoleAsync(editUser, "User").Result},
                new SelectListItem {Text = "Wykładowca", Value = "Lecturer",Selected = _userManager.IsInRoleAsync(editUser, "Lecturer").Result},
                new SelectListItem {Text = "Student", Value = "Student",Selected = _userManager.IsInRoleAsync(editUser, "Student").Result},
                new SelectListItem {Text = "Pracownik", Value = "Worker",Selected = _userManager.IsInRoleAsync(editUser, "Worker").Result}
            };

            return PartialView("Edit", userEdit);
        }

        public async Task<IActionResult> Delete(string userName)
        {
            AppUser editUser = _userManager.FindByNameAsync(userName).Result;

            await _userManager.DeleteAsync(editUser);

            return RedirectToAction("Index", "Admin");
        }
        public IActionResult Create()
        {
            var userEdit = new AdminUsersView();

            userEdit.Roles = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Administrator", Value = "Admin"},
                new SelectListItem {Text = "Użytkownik", Value = "User"},
                new SelectListItem {Text = "Wykładowca", Value = "Lecturer"},
                new SelectListItem {Text = "Student", Value = "Student"},
                new SelectListItem {Text = "Pracownik", Value = "Worker"}
            };

            return PartialView("Create", userEdit);
        }
        [HttpPost]
        public async Task<IActionResult> Create(AdminUsersView model)
        {
        if (ModelState.IsValid)
        {

            AppUser newUser = _userManager.FindByNameAsync(model.UserName).Result;
            if (newUser == null)
            {
                newUser = new AppUser();
                newUser.UserName = model.UserName;
                newUser.PhoneNumber = model.PhoneNumber;
                newUser.Email = model.Email;

                var resultCreate = await _userManager.CreateAsync(newUser, model.Password);
                if (resultCreate.Succeeded)
                {
                    var roleManager = _provider.GetRequiredService<RoleManager<IdentityRole>>();
                    if (model.Role != null)
                    {
                        foreach (string role in model.Role)
                        {

                            var roleCheck = roleManager.RoleExistsAsync(role).Result;
                            if (roleCheck)
                            {
                                var isInRole = await _userManager.IsInRoleAsync(newUser, role);
                                if (!isInRole)
                                {
                                    await _userManager.AddToRoleAsync(newUser, role);
                                }
                            }
                        }
                    }
                    return RedirectToAction("Index", "Admin");
                }
                else    
                {
                    foreach (var error in resultCreate.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
            }
            
        }

            model.Roles = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Administrator", Value = "Admin"},
                new SelectListItem {Text = "Użytkownik", Value = "User"},
                new SelectListItem {Text = "Wykładowca", Value = "Lecturer"},
                new SelectListItem {Text = "Student", Value = "Student"},
                new SelectListItem {Text = "Pracownik", Value = "Worker"}
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminUsersView model)
        {
            var roleManager = _provider.GetRequiredService<RoleManager<IdentityRole>>();

            AppUser editUser = _userManager.FindByNameAsync(model.UserName).Result;
            editUser.Email          = model.Email;
            editUser.PhoneNumber    = model.PhoneNumber;
            if (model.Role != null)
            {
                IList<string> userRoles = _userManager.GetRolesAsync(editUser).Result;
                foreach(string roleName in userRoles)
                {
                    if(!model.Role.Contains(roleName))
                    {
                        await _userManager.RemoveFromRoleAsync(editUser,roleName);
                    }
                }
                foreach (string role in model.Role)
                {

                    var roleCheck = roleManager.RoleExistsAsync(role).Result;
                    if (roleCheck)
                    {
                        var isInRole = await _userManager.IsInRoleAsync(editUser, role);
                        if (!isInRole)
                        {
                            await _userManager.AddToRoleAsync(editUser, role);
                        }
                    }
                }
            }
            var result = _userManager.UpdateAsync(editUser).Result;

            return RedirectToAction("Index", "Admin");
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Users()
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
                    UserName = user.UserName,
                    Role = new List<string>()
                    //Role = _userManager.GetRolesAsync(user).ToString()
                };
                var roles = _userManager.GetRolesAsync(user).Result;
                string roleStr = "";
                foreach (var role in roles)
                {
                    r.Role.Add(role);
                }
                userRoles.Add(r);
            }

            return View(userRoles);
        }
    }
}