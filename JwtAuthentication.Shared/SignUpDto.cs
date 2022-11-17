using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthentication.Shared
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "Veuillez indiquer votre Nom.")]
        [Display(Name = "Nom")]
        public string Nom { get; set; }
        [Required(ErrorMessage = "Veuillez indiquer votre City.")]
        public string City { get; set; }
        [Required(ErrorMessage = "Veuillez indiquer votre Email Adress.")]
        [EmailAddress]
        [Display(Name = "Email Adress")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Veuillez indiquer votre Date .")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Veuillez indiquer votre Password .")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(80, ErrorMessage = "", MinimumLength = 6)]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Veuillez indiquer votre Tel .")]
        public string PhoneNumber { get; set; }


    }
}
