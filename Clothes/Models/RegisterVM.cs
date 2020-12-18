using System.ComponentModel.DataAnnotations;

namespace Clothes.Models
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Имя - обязательное поле!")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Эл. почта - обязательное поле!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль - обязательное поле!")]
        public string Password { get; set; }
    }
}