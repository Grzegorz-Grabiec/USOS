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
        public USOSContext initContext()
        {
            DbContextOptionsBuilder<USOSContext> options = new DbContextOptionsBuilder<USOSContext>();
            options.UseSqlServer(configuration.GetConnectionString("MyConnStr"));
            var context = new USOSContext(options.Options);

            return context;
        }

        [HttpGet]
        public ActionResult News()
        {
            USOSContext context = this.initContext();
            List<NewsView> News;
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
            USOSContext context = this.initContext();
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Dodano";
                return View(model);
            }

            using (context)
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
            USOSContext context = this.initContext();

            using (context)
            {

                News Obj = context.News.Find(id);


                if (Obj == null)
                {
                    return Content("Strona nie istnieje");
                }

                model = new NewsView(Obj);
            }


            return PartialView(model);
        }

        [HttpPost]
        public ActionResult EditNews(NewsView model)
        {
            USOSContext context = this.initContext();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (context)
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
        public ActionResult DeleteNews(int id)
        {
            USOSContext context = this.initContext();
            using (context)
            {
               
                News Obj = context.News.Find(id);
                context.News.Remove(Obj);
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
