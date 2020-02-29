using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class LessonStudentMark
    {
        [key]
        public int ID { get; set; }
      
        public Lesson Lesson { get; set; }
        
        public Mark Mark { get; set; }

        public AppUser Username { get; set; }

    }
}
