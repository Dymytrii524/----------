using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GuestBook.Models
{
    public class Repository : IRepository
    {
        private readonly GuestBookContext _db;

        public Repository(GuestBookContext db)
        {
            _db = db;
        }

        public User GetUserByName(string name)
            => _db.Users.FirstOrDefault(u => u.Name == name);

        public User GetUserByNameAndPwd(string name, string pwd)
            => _db.Users.FirstOrDefault(
                u => u.Name == name && u.Pwd == pwd);

        public bool UserExists(string name)
            => _db.Users.Any(u => u.Name == name);

        public void AddUser(User user)
            => _db.Users.Add(user);

        public IEnumerable<Message> GetAllMessages()
            => _db.Messages
                  .Include(m => m.User)
                  .OrderByDescending(m => m.MessageDate)
                  .ToList();

        public void AddMessage(Message message)
            => _db.Messages.Add(message);

        public void Save()
            => _db.SaveChanges();
    }
}
