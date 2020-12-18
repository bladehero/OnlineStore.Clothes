using System.ComponentModel.DataAnnotations;

namespace Clothes.Models
{
    public class SendMessageVM
    {
        [Required(ErrorMessage = "Имя - обязательное поле!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Эл. почта - обязательное поле!")]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "Текст сообщения не может быть пустым!")]
        public string Text { get; set; }
    }
}