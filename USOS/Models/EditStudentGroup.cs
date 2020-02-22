using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class EditStudentGroup
    {
        [key]
        public string userName { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> groups { get; set; }
        [NotMapped]
        public List<int> group { get; set; }
    }
}
