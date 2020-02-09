using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class News
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Header { get; set; }

        public DateTime Date { get; set; }

    }
}
