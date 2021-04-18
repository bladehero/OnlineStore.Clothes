using System.ComponentModel.DataAnnotations;

namespace Clothes.Models
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Name is a required field!")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is a required field!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is a required field!")]
        public string Password { get; set; }
    }
}