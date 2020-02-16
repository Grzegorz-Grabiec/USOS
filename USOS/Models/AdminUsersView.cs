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
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "A password contains at least eight characters, including at least one number and includes both lower and uppercase letters and special characters")]
        public string Password { get; set; }
        [NotMapped]
        public List<string> Role { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> Roles { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
