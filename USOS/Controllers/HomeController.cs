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

    public class HomeController : Controller
    {
        
        private readonly IConfiguration configuration;
        SqlConnection con;
        SqlCommand com;
        SqlDataReader dr;

        public HomeController(IConfiguration config)
        {
            this.configuration = config;
        }
        
        public IActionResult Index()
        {
            return View();
        }

       

        [HttpGet]
       
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
