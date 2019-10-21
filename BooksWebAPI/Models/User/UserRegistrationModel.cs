using System.ComponentModel.DataAnnotations;

namespace BooksWebAPI.Models.User
{
    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The two passwords are not the same.")]
        public string ConfirmPassword { get; set; }
    }
}
