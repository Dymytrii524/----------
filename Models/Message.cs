using System;
using System.ComponentModel.DataAnnotations;

namespace GuestBook.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int Id_User { get; set; }

        [Required(ErrorMessage = "Введіть повідомлення")]
        public string MessageText { get; set; }

        public DateTime MessageDate { get; set; }

        public virtual User User { get; set; }
    }
}
