using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GuestBook.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введіть логін")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введіть пароль")]
        public string Pwd { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
