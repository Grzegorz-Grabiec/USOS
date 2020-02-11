using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class StudentGroup
    {
        [key]
        public int ID { get; set; }
        public Group group { get; set; }
        public AppUser appUser { get; set; }

    }
}
