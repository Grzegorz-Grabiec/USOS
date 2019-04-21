using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using USOS.Models;
using Microsoft.Extensions.Configuration;

namespace USOS.Controllers
{
    public class LoginController : Controller
    {

        private readonly IConfiguration configuration;
        SqlConnection con;
        SqlCommand com;
        SqlDataReader dr;

        public LoginController(IConfiguration config)
        {
            this.configuration = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
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