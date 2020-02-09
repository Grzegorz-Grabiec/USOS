using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using USOS.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Akitek()
        {
            return View();
        }

        [HttpGet]
        public ActionResult News()
        {
            DbContextOptionsBuilder<USOSContext> options = new DbContextOptionsBuilder<USOSContext>();
            options.UseSqlServer(configuration.GetConnectionString("MyConnStr"));
            List<NewsView> News;
            var context = new USOSContext(options.Options);
            News = context.News.ToArray().OrderBy(x => x.ID).Select(x => new NewsView(x)).ToList();
            return View(News);
        }
        [HttpGet]
        public ActionResult CreateNews()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateNews(NewsView model)
        {
            DbContextOptionsBuilder<USOSContext> options = new DbContextOptionsBuilder<USOSContext>();
            options.UseSqlServer(configuration.GetConnectionString("MyConnStr"));
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Dodano";
                return View(model);
            }

            using (var context = new USOSContext(options.Options))
            {
                News Obj = new News();

                if (context.News.Any(x => x.ID == model.ID))
                {
                    ModelState.AddModelError("", "Ta nazwa już istnieje");
                    return View(model);
                }
                Obj.Text = model.Text;
                Obj.Header = model.Header;
                Obj.Date = DateTime.Now;

                ViewBag.Message = "Dodano";
                context.News.Add(Obj);
                context.SaveChanges();

            }
            return RedirectToAction("News");

        }
        [HttpGet]
        public ActionResult EditNews(int id)
        {
            NewsView model;
            DbContextOptionsBuilder<USOSContext> options = new DbContextOptionsBuilder<USOSContext>();
            options.UseSqlServer(configuration.GetConnectionString("MyConnStr"));

            using (var context = new USOSContext(options.Options))
            {

                News Obj = context.News.Find(id);


                if (Obj == null)
                {
                    return Content("Strona nie istnieje");
                }

                model = new NewsView(Obj);
            }


            return PartialView();
        }

        [HttpPost]
        public ActionResult EditNews(NewsView model)
        {
            DbContextOptionsBuilder<USOSContext> options = new DbContextOptionsBuilder<USOSContext>();
            options.UseSqlServer(configuration.GetConnectionString("MyConnStr"));
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var context = new USOSContext(options.Options))
            {

                int id = model.ID;


                News Obj = context.News.Find(id);


                if (context.News.Where(x => x.ID != id).Any(x => x.Text == model.Text))

                {
                    ModelState.AddModelError("", "Aktualność już istnieje");
                }
                Obj.Text = model.Text;
                Obj.Header = model.Header;
                Obj.Date = DateTime.Now;




                context.SaveChanges();
            }
            return RedirectToAction("News");
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
