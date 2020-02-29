﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class Group
    {
        [key]
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public Group()
        {

        }
        public Group(Group _group)
        {
            ID = _group.ID;
            Name = _group.Name;
            
        }
    }
}
