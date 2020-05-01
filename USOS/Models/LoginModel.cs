using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Pole login musi być wypełnione"), MaxLength(256)]
        public string Login { get; set; }
        [Required(ErrorMessage = "Pole hasło musi być wypełnione")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Hasło musi zawierać co najmniej {2} znaków.", MinimumLength = 7)]
        public string Password { get; set; }

        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }  
       
       
    }
}
