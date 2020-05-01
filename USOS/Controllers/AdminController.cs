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

        [HttpGet]
        public PartialViewResult IndexGrid()
        {
            USOSContext context = this.initContext();

            List<LessonsView> lessonsView = new List<LessonsView>();

            List<Lesson> lessons = context.Lesson.Select(x => new Lesson() { ID = x.ID, lecture = x.lecture, lecturer = x.lecturer }).ToList();
            foreach (Lesson lesson in lessons)
            {
                LessonsView view = new LessonsView();
                view.ID = lesson.ID;
                view.LectureID = lesson.lecture.ID;
                view.LectureName = lesson.lecture.Name;
                view.LecturerName = lesson.lecturer.UserName;
                List<LessonsGroup> lessonsGroup = context.LessonsGroup.Where(x => x.lesson.ID == lesson.ID).Include(x => x.group).Include(x => x.lesson).ToList();
                foreach (LessonsGroup lg in lessonsGroup)
                {
                    if (view.GroupName == null)
                    {
                        view.GroupName += lg.group.Name;
                        view.GroupID = Convert.ToString(lg.group.ID);
                    }
                    else
                    {
                        view.GroupName += "," + lg.group.Name;
                        view.GroupID = "," + Convert.ToString(lg.group.ID);
                    }
                }
                lessonsView.Add(view);
            }
            return PartialView("_IndexGrid", lessonsView);
        }

        public IActionResult EditLecture(int ID)
        {
            USOSContext context = this.initContext();

            Lecture lecture = context.Lecture.Find(ID);

            return PartialView("EditLecture", lecture);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditLecture(Lecture model)
        {
            USOSContext context = this.initContext();

            context.Lecture.Update(model);
            context.SaveChanges();
            return RedirectToAction("Lectures", "Admin");
        }
        public IActionResult EditGroup(int ID)
        {
            USOSContext context = this.initContext();

            Group group = context.Group.Find(ID);

            return PartialView("EditGroup", group);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditGroup(Group model)
        {
            USOSContext context = this.initContext();

            context.Group.Update(model);
            context.SaveChanges();
            return RedirectToAction("Groups", "Admin");
        }

        public IActionResult EditStudentGroup(string userName)
        {
            AppUser editUser = _userManager.FindByNameAsync(userName).Result;
            USOSContext context = this.initContext();
            EditStudentGroup model = new EditStudentGroup();
            model.userName = editUser.UserName;
            model.groups = context.Group.Select(x => new SelectListItem() { Value = Convert.ToString(x.ID), Text = x.Name }).ToList();

            return PartialView("EditStudentGroup", model);
        }
        [HttpPost]
        public IActionResult EditStudentGroup(EditStudentGroup model)
        {
            USOSContext context = this.initContext();
            AppUser student = _userManager.FindByNameAsync(model.userName).Result;
            var select  = context.StudentGroup.Where(x => x.appUser.UserName == model.userName).Select(x => new StudentGroup() {ID = x.ID,appUser = x.appUser,group = x.group });
            context.StudentGroup.RemoveRange(select);

            foreach (int groupId in model.group)
            {
                Group group = context.Group.Find(groupId);
                if(group != null)
                {
                    StudentGroup newStudentGroup = new StudentGroup();
                    newStudentGroup.group       = group;
                    newStudentGroup.appUser = student;
                    context.StudentGroup.Add(newStudentGroup);
                    context.Entry(newStudentGroup.group).State = EntityState.Unchanged;
                    context.Entry(newStudentGroup.appUser).State = EntityState.Unchanged;
                }
            }
            context.SaveChanges();

            return RedirectToAction("Users", "Admin");
        }
        public IActionResult EditUser(string userName)
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

            return PartialView("EditUser", userEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(AdminUsersView model)
        {
            var roleManager = _provider.GetRequiredService<RoleManager<IdentityRole>>();

            AppUser editUser = _userManager.FindByNameAsync(model.UserName).Result;
            editUser.Email = model.Email;
            editUser.PhoneNumber = model.PhoneNumber;
            if (model.Role != null)
            {
                IList<string> userRoles = _userManager.GetRolesAsync(editUser).Result;
                foreach (string roleName in userRoles)
                {
                    if (!model.Role.Contains(roleName))
                    {
                        await _userManager.RemoveFromRoleAsync(editUser, roleName);
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

            return RedirectToAction("Users", "Admin");
        }
        public async Task<IActionResult> DeleteUser(string userName)
        {

            AppUser editUser = _userManager.FindByNameAsync(userName).Result;

            await _userManager.DeleteAsync(editUser);
            await _userManager.UpdateAsync(editUser);

            return RedirectToAction("Users", "Admin");
        }
        public async Task<IActionResult> DeleteLecture(int ID)
        {
            USOSContext context = this.initContext();


            Lecture lecture = context.Lecture.Find(ID);
            if (lecture != null)
            {
                context.Lecture.RemoveRange(lecture);
                context.Lecture.Remove(lecture);
                context.SaveChanges();
            }

            return RedirectToAction("Lectures", "Admin");
        }
        public async Task<IActionResult> DeleteGroup(int ID)
        {
            USOSContext context = this.initContext();

            Group group = context.Group.Find(ID);
            if(group != null)
            {
                context.Group.Remove(group);
                context.SaveChanges();
            }

            return RedirectToAction("Groups", "Admin");
        }
        public IActionResult Groups()
        {
            USOSContext context = this.initContext();
            List<Group> groups;

            groups = context.Group.ToArray().OrderBy(x => x.ID).Select(x => new Group() { ID = x.ID, Name = x.Name }).ToList();
            return View(groups);
        }
        public IActionResult Lectures()
        {
            USOSContext context = this.initContext();
            List<Lecture> lectures;

            lectures = context.Lecture.ToArray().OrderBy(x => x.ID).Select(x => new Lecture() { ID = x.ID, Name = x.Name }).ToList();
            return View(lectures);
        }
        public IActionResult CreateLecture()
        {
            USOSContext context = this.initContext();

            Lecture lecture = new Lecture();

            return PartialView("CreateLecture", lecture);
        }
        public IActionResult EditLesson(int ID)
        {
            USOSContext context = this.initContext();
            CreateLessonView lesson = new CreateLessonView();
            Lesson editLesson = context.Lesson.Where(x => x.ID == ID).Include(x => x.lecture).Include(x => x.lecturer).First();
            
            lesson.lectures = context.Lecture.Select(x => new SelectListItem() { Value = Convert.ToString(x.ID), Text = x.Name }).ToList();
            lesson.groups = context.Group.Select(x => new SelectListItem() { Value = Convert.ToString(x.ID), Text = x.Name }).ToList();

            IList<AppUser> lecturers = _userManager.GetUsersInRoleAsync("Lecturer").Result;
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (AppUser user in lecturers)
            {
                SelectListItem item = new SelectListItem() { Value = user.UserName, Text = user.UserName };

                list.Add(item);
            }

            foreach (SelectListItem g in lesson.groups)
            {
                var select = context.LessonsGroup.Where(x => x.lesson.ID == ID && x.group.ID == Convert.ToInt32(g.Value)).Include(x => x.lesson).Include(x => x.group).Select(x => new LessonsGroup(x));

                if (select.Count() > 0)
                    g.Selected = true;
            }
            lesson.lecturers = list;
            lesson.lectureID = editLesson.lecture.ID;
            lesson.lecturerID = editLesson.lecturer.UserName;
            lesson.lessonID = ID;
            return PartialView("EditLesson", lesson);
        }
        [HttpPost]
        public IActionResult EditLesson(CreateLessonView model)
        {
            USOSContext context = this.initContext();
            Lesson editLesson = context.Lesson.Find(model.lessonID);
            if(model.lectureID != null)
                editLesson.lecture = context.Lecture.Find(Convert.ToInt32(model.lectureID));
            if(model.lecturerID != null)
                editLesson.lecturer = _userManager.FindByNameAsync(model.lecturerID).Result;
            var select = context.LessonsGroup.Where(x => x.lesson.ID == model.lessonID).Select(x => new LessonsGroup(x));
            context.LessonsGroup.RemoveRange(select);
            if (model.group != null)
            {
                foreach (int g in model.group)
                {
                    Group group = context.Group.Find(g);
                    LessonsGroup newLessonGroup = new LessonsGroup();
                    newLessonGroup.group = group;
                    newLessonGroup.lesson = editLesson;
                    context.LessonsGroup.Add(newLessonGroup);
                }
            }
            context.Entry(editLesson.lecturer).State = EntityState.Unchanged;
            context.SaveChanges();
            return RedirectToAction("Lessons", "Admin");
        }
        public IActionResult CreateLesson()
        {
            USOSContext context = this.initContext();

            CreateLessonView lesson = new CreateLessonView();

            lesson.lectures = context.Lecture.Select(x => new SelectListItem() { Value = Convert.ToString(x.ID), Text = x.Name }).ToList();
            lesson.groups = context.Group.Select(x => new SelectListItem() { Value = Convert.ToString(x.ID), Text = x.Name }).ToList();
            IList<AppUser> lecturers = _userManager.GetUsersInRoleAsync("Lecturer").Result;
            List<SelectListItem> list = new List<SelectListItem>();
            foreach(AppUser user in lecturers)
            {
                list.Add(new SelectListItem() { Value = user.UserName, Text = user.UserName});
            }
            lesson.lecturers = list;
            return PartialView("CreateLesson", lesson);
        }
        [HttpPost]
        public IActionResult CreateLesson(CreateLessonView lecture)
        {
            USOSContext context = this.initContext();

            Lesson newLesson = new Lesson();
            newLesson.lecture = context.Lecture.Find(Convert.ToInt32(lecture.lectureID));
            newLesson.lecturer = _userManager.FindByNameAsync(lecture.lecturerID).Result;
            context.Lesson.Add(newLesson);
            context.Entry(newLesson.lecturer).State = EntityState.Unchanged;

            foreach (int g in lecture.group)
            {
                Group group = context.Group.Find(g);
                LessonsGroup newLessonGroup = new LessonsGroup();
                newLessonGroup.group = group;
                newLessonGroup.lesson = newLesson;
                context.LessonsGroup.Add(newLessonGroup);
            }
            context.SaveChanges();
            return RedirectToAction("Lessons", "Admin");
        }
        [HttpPost]
        public IActionResult CreateLecture(Lecture model)
        {
            USOSContext context = this.initContext();
            
            Lecture newLecture = new Lecture();

            Lecture result = context.Lecture.Find(model.ID);
            if (result == null)
            {
                newLecture.Name = model.Name;

                context.Lecture.Add(newLecture);
                context.SaveChanges();
            }

            return RedirectToAction("Lectures", "Admin");
        }
        public IActionResult CreateGroup()
        {
            USOSContext context = this.initContext();

            Group group = new Group();

            return PartialView("CreateGroup", group);
        }
        [HttpPost]
        public IActionResult CreateGroup(Group model)
        {
            USOSContext context = this.initContext();

            Group newGroup = new Group();

            Group result = context.Group.Find(model.ID);
            if(result == null)
            {
                newGroup.Name = model.Name;

                context.Group.Add(newGroup);
                context.SaveChanges();
            }

            return RedirectToAction("Groups", "Admin");
        }
            public IActionResult CreateUser()
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

            return PartialView("CreateUser", userEdit);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(AdminUsersView model)
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
                        TempData["status"] = "Success";
                        return RedirectToAction("Users", "Admin");
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

            return View("Index",model);
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
        public IActionResult Lessons()
        {
            USOSContext context = this.initContext();

            List<LessonsView> lessonsView = new List<LessonsView>();

            List<Lesson> lessons = context.Lesson.Select(x => new Lesson() { ID = x.ID, lecture = x.lecture, lecturer = x.lecturer }).ToList();
            foreach(Lesson lesson in lessons)
            {
                LessonsView view = new LessonsView();
                view.ID = lesson.ID;
                view.LectureID = lesson.lecture.ID;
                view.LectureName = lesson.lecture.Name;
                view.LecturerName = lesson.lecturer.UserName;
                List<LessonsGroup> lessonsGroup = context.LessonsGroup.Where(x => x.lesson.ID == lesson.ID).Include(x => x.group).Include(x => x.lesson).ToList();//.Select(x => new LessonsGroup(x)).ToList();
                foreach (LessonsGroup lg in lessonsGroup)
                {
                    if (view.GroupName == null)
                    {
                        view.GroupName += lg.group.Name;
                        view.GroupID = Convert.ToString(lg.group.ID);
                    }
                    else
                    {
                        view.GroupName += "," + lg.group.Name;
                        view.GroupID = "," + Convert.ToString(lg.group.ID);
                    }
                }
                lessonsView.Add(view);
            }
            return View(lessonsView);
        }
        public IActionResult Users()
        {
            if (!User.IsInRole("Admin"))
                return RedirectToAction("Index", "Home");

            USOSContext context = this.initContext();
            var userRoles = new List<AdminUsersView>();
            var userStore = new UserStore<AppUser>(context);
            
            foreach (var user in userStore.Users)
            {
                var r = new AdminUsersView
                {
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Role = new List<string>()
                };
                if(_userManager.IsInRoleAsync(user,"Student").Result)
                {
                    var list = context.StudentGroup.Where(x => x.appUser == user).Include(x => x.group).ToList();
                    foreach (StudentGroup s in list)
                    {
                        r.groups += s.group.Name + " ";
                    }
                }

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