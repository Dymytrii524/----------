using System;
using System.Linq;
using GuestBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuestBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repo;

        public HomeController(IRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetMessages()
        {
            var messages = _repo.GetAllMessages()
                .Select(m => new
                {
                    userName = m.User != null ? m.User.Name : "Невідомий",
                    text     = m.MessageText,
                    date     = m.MessageDate.ToString("dd.MM.yyyy HH:mm")
                });
            return Json(messages);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult AddMessage(string messageText)
        {
            if (string.IsNullOrWhiteSpace(messageText))
                return Json(new { success = false, error = "Порожнє повідомлення" });

            var user = _repo.GetUserByName(User.Identity.Name);
            if (user == null)
                return Json(new { success = false, error = "Користувача не знайдено" });

            _repo.AddMessage(new Message
            {
                Id_User     = user.Id,
                MessageText = messageText,
                MessageDate = DateTime.Now
            });
            _repo.Save();

            return Json(new { success = true });
        }
    }
}
