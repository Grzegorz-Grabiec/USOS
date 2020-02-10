﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class Lecture
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
