using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USOS.Models;

namespace USOS.Models
{
    public class USOSContext : IdentityDbContext<AppUser>
    {
        public DbSet<News> News { get; set; }
        public USOSContext(DbContextOptions<USOSContext> options) : base(options)
        {
  
        }
        public DbSet<USOS.Models.Lecture> Lecture { get; set; }
        public DbSet<USOS.Models.Mark> Mark { get; set; }
        public DbSet<USOS.Models.Group> Group { get; set; }
        public DbSet<USOS.Models.StudentGroup> StudentGroup { get; set; }
        public DbSet<USOS.Models.Lesson> Lesson { get; set; }
        public DbSet<USOS.Models.LessonsGroup> LessonsGroup { get; set; }
        public DbSet<USOS.Models.LessonStudentMark> LessonStudentMark { get; set; }
        public DbSet<USOS.Models.LessonStudentMarkView> LessonStudentMarkView { get; set; }
        //public DbSet<USOS.Models.AdminUsersView> AdminUsersView { get; set; }
    }
}
