using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthentication.Shared
{
    public class SignInDto
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Email Adress")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(80, ErrorMessage = "", MinimumLength = 6)]
        public string Password { get; set; }
        [Display(Name = "Souviens-toi de moi")]
        public bool RememberMe { get; set; }
    }
}
