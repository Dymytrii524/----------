using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GuestBook.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuestBook.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IRepository _repo;

    public LoginModel(IRepository repo) => _repo = repo;

    [BindProperty]
    public LoginViewModel Input { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = _repo.GetUserByNameAndPwd(Input.Name, HashPassword(Input.Pwd));
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Невірний логін або пароль");
            return Page();
        }

        var claims = new[] { new Claim(ClaimTypes.Name, user.Name) };
        var identity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return RedirectToPage("/Index");
    }

    private static string HashPassword(string pwd)
    {
        byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes(pwd));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
