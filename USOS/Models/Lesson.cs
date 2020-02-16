﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class Lesson
    {
        [key]
        public int ID { get; set; }
        public virtual AppUser lecturer { get; set; }
        public virtual Lecture lecture { get; set; }

    }
}
