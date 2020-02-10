using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class Group
    {
        [key]
        public int ID{ get; set; }
        public string Name { get; set; }
    }
}
