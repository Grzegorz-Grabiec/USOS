using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class RegisterModel
    {
        [Required, MaxLength(256), Display(Name = "Login")]
        public string Login { get; set; }
        [Required,MinLength(6), MaxLength(25), DataType(DataType.Password), Display(Name="Password")]
        public string Password { get; set; }
        [Required, MinLength(6), MaxLength(25), DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage ="Złe hasło")]
        public string ConfirmPassword { get; set; }
    }
}
