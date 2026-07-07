using System.Security.Cryptography;
using System.Text;
using GuestBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuestBook.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly IRepository _repo;

    public RegisterModel(IRepository repo) => _repo = repo;

    [BindProperty]
    public RegisterViewModel Input { get; set; } = new();

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        if (_repo.UserExists(Input.Name))
        {
            ModelState.AddModelError("Input.Name", "Такий логін вже зайнятий");
            return Page();
        }

        _repo.AddUser(new User
        {
            Name = Input.Name,
            Pwd  = HashPassword(Input.Pwd)
        });
        _repo.Save();

        return RedirectToPage("/Account/Login");
    }

    private static string HashPassword(string pwd)
    {
        byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes(pwd));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
