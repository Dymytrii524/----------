using System.ComponentModel.DataAnnotations;

namespace GuestBook.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введіть логін")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        public string Pwd { get; set; }

        [Required(ErrorMessage = "Підтвердіть пароль")]
        [DataType(DataType.Password)]
        [Compare("Pwd", ErrorMessage = "Паролі не співпадають")]
        public string PwdConfirm { get; set; }
    }
}
