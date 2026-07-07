using System.ComponentModel.DataAnnotations;

namespace GuestBook.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Введіть логін")]
    [Display(Name = "Логін")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введіть пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Pwd { get; set; } = string.Empty;
}
