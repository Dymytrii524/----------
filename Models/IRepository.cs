using System.Collections.Generic;

namespace GuestBook.Models
{
    public interface IRepository
    {
        User GetUserByName(string name);
        User GetUserByNameAndPwd(string name, string pwd);
        bool UserExists(string name);
        void AddUser(User user);

        IEnumerable<Message> GetAllMessages();
        void AddMessage(Message message);

        void Save();
    }
}
