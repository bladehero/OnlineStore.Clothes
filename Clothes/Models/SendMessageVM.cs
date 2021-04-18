using System.ComponentModel.DataAnnotations;

namespace Clothes.Models
{
    public class SendMessageVM
    {
        [Required(ErrorMessage = "Name is a required field!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is a required field!")]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "Text is a required field!")]
        public string Text { get; set; }
    }
}