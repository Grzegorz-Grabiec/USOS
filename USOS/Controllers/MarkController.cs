using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using USOS.Models;

namespace USOS.Controllers
{
    public class MarkController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration configuration;
        private readonly IServiceProvider _provider;
        public MarkController(IConfiguration config, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IServiceProvider provider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.configuration = config;
            _provider = provider;
        }
        public IActionResult MarkStudent()
        {
            USOSContext context = this.initContext();

            List<LessonStudentMarkView> lessonsStudentMarkView = new List<LessonStudentMarkView>();
            List<StudentGroup> studentgroups = context.StudentGroup.Include(x => x.appUser).Include(x => x.group).Where(x => x.appUser.UserName == User.Identity.Name).Select(x => new StudentGroup(x)).ToList();

            foreach (StudentGroup studentgroup in studentgroups)
            {

                List<LessonsGroup> lessonsGroup = context.LessonsGroup.Where(x => x.group.ID == studentgroup.group.ID).Include(x => x.group).Include(x => x.lesson).Include(x => x.lesson.lecture).ToList();//.Select(x => new LessonsGroup(x)).ToList();
                foreach (LessonsGroup lg in lessonsGroup)
                {
                    LessonStudentMarkView view = new LessonStudentMarkView();
                    if (context.LessonStudentMark.Where(x => x.Lesson.ID == lg.lesson.ID && x.Username.UserName == studentgroup.appUser.UserName).Include(x => x.Mark).Count() != 0)
                    {
                        LessonStudentMark lessonstudentmark = context.LessonStudentMark.Where(x => x.Lesson.ID == lg.lesson.ID && x.Username.UserName == studentgroup.appUser.UserName).Include(x => x.Mark).First();
                        view.Mark = lessonstudentmark.Mark.Value;
                    }
                    view.Username = studentgroup.appUser.UserName;
                    view.Lesson = lg.lesson.lecture.Name;
                    lessonsStudentMarkView.Add(view);

                }

            }
            return View(lessonsStudentMarkView);
        }
        public IActionResult MarkLecturer()
        {
            USOSContext context = this.initContext();
            List<Lesson> lessons = context.Lesson.Where(x => x.lecturer.UserName == User.Identity.Name).Include(x => x.lecture).ToList();
            List<LessonStudentMarkView> lessonsStudentMarkView = new List<LessonStudentMarkView>();


            foreach (Lesson lesson in lessons)
            {
                List<LessonsGroup> lessonsGroup = context.LessonsGroup.Where(x => x.lesson.ID == lesson.ID).Include(x => x.group).Include(x => x.lesson).Include(x => x.lesson.lecture).ToList();//.Select(x => new LessonsGroup(x)).ToList();
                foreach (LessonsGroup lg in lessonsGroup)
                {
                    List<StudentGroup> studentgroups = context.StudentGroup.Include(x => x.appUser).Include(x => x.group).Where(x => x.group.ID == lg.group.ID).ToList();
                    foreach (StudentGroup studentgroup in studentgroups)
                    {
                        LessonStudentMarkView view = new LessonStudentMarkView();
                        if (context.LessonStudentMark.Where(x => x.Lesson.ID == lg.lesson.ID && x.Username.UserName == studentgroup.appUser.UserName).Include(x => x.Mark).Count() != 0)
                        {
                            LessonStudentMark lessonstudentmark = context.LessonStudentMark.Where(x => x.Lesson.ID == lg.lesson.ID && x.Username.UserName == studentgroup.appUser.UserName).Include(x => x.Mark).First();
                            view.Mark = lessonstudentmark.Mark.Value;
                            view.ID = lessonstudentmark.ID;


                        }                  
                        view.LessonID = lesson.ID;
                        view.Username = studentgroup.appUser.UserName;
                        view.Lesson = lg.lesson.lecture.Name;
                        lessonsStudentMarkView.Add(view);
                    }

                }

            }
            return View(lessonsStudentMarkView);
        }
        public IActionResult MarkWorker()
        {
            USOSContext context = this.initContext();
            List<Lesson> lessons = context.Lesson.Include(x => x.lecture).ToList();
            List<LessonStudentMarkView> lessonsStudentMarkView = new List<LessonStudentMarkView>();


            foreach (Lesson lesson in lessons)
            {
                List<LessonsGroup> lessonsGroup = context.LessonsGroup.Where(x => x.lesson.ID == lesson.ID).Include(x => x.group).Include(x => x.lesson).Include(x => x.lesson.lecture).ToList();//.Select(x => new LessonsGroup(x)).ToList();
                foreach (LessonsGroup lg in lessonsGroup)
                {
                    List<StudentGroup> studentgroups = context.StudentGroup.Include(x => x.appUser).Include(x => x.group).Where(x => x.group.ID == lg.group.ID).ToList();
                    foreach (StudentGroup studentgroup in studentgroups)
                    {
                        LessonStudentMarkView view = new LessonStudentMarkView();
                        if (context.LessonStudentMark.Where(x => x.Lesson.ID == lg.lesson.ID && x.Username.UserName == studentgroup.appUser.UserName).Include(x => x.Mark).Count() != 0)
                        {
                            LessonStudentMark lessonstudentmark = context.LessonStudentMark.Where(x => x.Lesson.ID == lg.lesson.ID && x.Username.UserName == studentgroup.appUser.UserName).Include(x => x.Mark).First();
                            view.Mark = lessonstudentmark.Mark.Value;   
                        }
                        view.LessonID = lesson.ID;

                        view.Username = studentgroup.appUser.UserName;
                        view.Lesson = lg.lesson.lecture.Name;
                        lessonsStudentMarkView.Add(view);
                    }

                }

            }
            return View(lessonsStudentMarkView);
        }
        public USOSContext initContext()
        {
            DbContextOptionsBuilder<USOSContext> options = new DbContextOptionsBuilder<USOSContext>();
            options.UseSqlServer(configuration.GetConnectionString("MyConnStr"));
            var context = new USOSContext(options.Options);

            return context;
        }
        [HttpGet]
        public ActionResult EditMark(int lessonId, string username)
        {
            USOSContext context = this.initContext();

            LessonStudentMarkView edit = new LessonStudentMarkView();
            edit.LessonID = lessonId;
            edit.Username = username;
            edit.Marks = context.Mark.Select(x => new SelectListItem() { Value = Convert.ToString(x.ID), Text = x.Name }).ToList();


            return PartialView("EditMark",edit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMark(LessonStudentMarkView model)
        {
            USOSContext context = this.initContext();
            LessonStudentMark studentmark = null;
            Mark mark = context.Mark.Where(x => x.ID == model.MarkID).First();
            AppUser user = context.Users.Where(x => x.UserName == model.Username).First();
            Lesson lesson = context.Lesson.Where(x => x.ID == model.LessonID).First();
            var query = context.LessonStudentMark.Where(x => x.Lesson.ID == model.LessonID && x.Username.UserName == model.Username).Include(x => x.Mark) ;        
            if (query.Count() != 0)
            {
               studentmark = query.First();
            }
            if (studentmark == null)
            {
                studentmark = new LessonStudentMark();
                studentmark.Mark = mark;
                studentmark.Lesson = lesson;
                studentmark.Username = user;
                context.LessonStudentMark.Add(studentmark);
                context.Entry(studentmark.Lesson).State = EntityState.Unchanged;
                context.Entry(studentmark.Username).State = EntityState.Unchanged;
            }
            else
            {
                studentmark.Mark = mark;
                context.LessonStudentMark.Update(studentmark);
            }
            context.SaveChanges();
            return RedirectToAction("MarkLecturer", "Mark");
        }
    }
}