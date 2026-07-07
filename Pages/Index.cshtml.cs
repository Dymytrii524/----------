using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GuestBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuestBook.Pages;

public class IndexModel : PageModel
{
    private readonly IRepository _repo;

    public IndexModel(IRepository repo) => _repo = repo;

    [BindProperty]
    [Required(ErrorMessage = "Введіть повідомлення")]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = "Повідомлення: від 1 до 1000 символів")]
    [Display(Name = "Повідомлення")]
    public string MessageText { get; set; } = string.Empty;

    public void OnGet() { }

    // GET /?handler=Messages  — AJAX endpoint
    public JsonResult OnGetMessages()
    {
        var messages = _repo.GetAllMessages()
            .Select(m => new
            {
                userName = m.User != null ? m.User.Name : "Невідомий",
                text     = m.MessageText,
                date     = m.MessageDate.ToString("dd.MM.yyyy HH:mm")
            });
        return new JsonResult(messages);
    }

    // POST /  — form submit
    public IActionResult OnPost()
    {
        if (!User.Identity!.IsAuthenticated)
            return RedirectToPage("/Account/Login");

        if (!ModelState.IsValid)
            return Page();

        var user = _repo.GetUserByName(User.Identity.Name!);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Користувача не знайдено");
            return Page();
        }

        _repo.AddMessage(new Message
        {
            Id_User     = user.Id,
            MessageText = MessageText,
            MessageDate = DateTime.Now,
            User        = user
        });
        _repo.Save();

        return RedirectToPage();
    }
}
