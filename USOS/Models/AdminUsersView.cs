using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class AdminUsersView
    {
        [Key]
        public string UserName { get; set; }
        [NotMapped]
        public List<string> Role { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> Roles { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
