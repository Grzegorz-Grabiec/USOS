using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class LessonsGroup
    {
        [key]
        public int ID { get; set; }
        public Lesson lesson { get; set; }
        public Group group { get; set; }
    }
}
