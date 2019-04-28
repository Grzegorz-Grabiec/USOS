using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class LoginModel
    {
        [Required, MaxLength(256)]
        public string Login { get; set; }
        [Required, MinLength(6), MaxLength(25), DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }  
       
       
    }
}
