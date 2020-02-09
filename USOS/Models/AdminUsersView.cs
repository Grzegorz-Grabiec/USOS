using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class AdminUsersView
    {
        [Key]
        public string UserName { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
