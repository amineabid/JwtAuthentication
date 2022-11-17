using System.ComponentModel.DataAnnotations;

namespace JwtAuthentication.Shared
{
    //public class User
    //{
    //    [Required]
    //    [EmailAddress]
    //    [Display(Name ="Email Adress")]
    //    public string EmailAdress { get; set; }

    //    [Required]
    //    [DataType(DataType.Password)]
    //    [Display(Name ="Password")]
    //    [StringLength(80,ErrorMessage ="",MinimumLength =6)]
    //    public string Password { get; set; }
    //}
    public class UserDto
    {
        [Required(ErrorMessage = "Veuillez indiquer votre Nom.")]
        [Display(Name = "Nom")]
        public string Nom { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
    }
}