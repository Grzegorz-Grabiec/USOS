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
        [StringLength(100, ErrorMessage = "Hasło musi zawierać co najmniej {2} znaków.", MinimumLength = 8)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Hasło musi zawierać co najmniej osiem znaków, w tym co najmniej jedną cyfrę, a także małe i wielkie litery oraz znaki specjalne")]
        public string Password { get; set; }
        [NotMapped]
        public List<string> Role { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> Roles { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string groups { get; set; }
    }
}
