﻿using System;
using System.IO;
using System.Web;
using System.Data;
//using System.Web.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;
using USOS.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace USOS.Controllers
{
    public class PlanController : Controller
    {
        
        public IActionResult Plan()
        {

            return View();
        }


        public void ConfigureServices(
           IServiceCollection services)
        {
            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Data")));

            services.AddMvc();
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "Data",
                        file.FileName);
            

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return RedirectToAction("Plan");
        }

        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
      
    }
}

