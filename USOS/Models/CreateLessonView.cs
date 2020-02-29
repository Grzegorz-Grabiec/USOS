using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class CreateLessonView
    {
        [NotMapped]
        public IEnumerable<SelectListItem> lectures { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> lecturers { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> groups { get; set; }
        public string lecturerID { get; set; }
        public int lectureID { get; set; }
        public int lessonID { get; set; }
        [NotMapped]
        public List<int> group { get; set; }
    }
}
