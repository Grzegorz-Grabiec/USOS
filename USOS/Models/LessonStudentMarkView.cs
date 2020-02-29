using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class LessonStudentMarkView
    {
        [key]
        public int ID { get; set; }
      
        public string Lesson { get; set; }
        
        public decimal Mark { get; set; }

        public string Username { get; set; }
        public int LessonID { get; set; }
        public int MarkID { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> Marks { get; set; }

        public LessonStudentMarkView()
        {

        }

        public LessonStudentMarkView(LessonStudentMarkView _LessonStudentMark)
        {
            ID = _LessonStudentMark.ID;
            Lesson = _LessonStudentMark.Lesson;
            Mark = _LessonStudentMark.Mark;
            Username = _LessonStudentMark.Username;
            LessonID = _LessonStudentMark.LessonID;
        }
    }
}
