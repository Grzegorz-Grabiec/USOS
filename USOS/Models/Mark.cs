using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class Mark
    {
        [key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        
    }
}
