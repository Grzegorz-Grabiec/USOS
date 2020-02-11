using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace USOS.Models
{
    public class Plan
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileText { get; set; }
        public string FileUrl { get; set; }
        public IEnumerable<Plan> FileList { get; set; }
    }
}
