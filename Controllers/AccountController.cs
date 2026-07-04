using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GuestBook.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace GuestBook.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository _repo;

        public AccountController(IRepository repo)
        {
            _repo = repo;
        }

        private string HashPassword(string pwd)
        {
            using var md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Name, string Pwd)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Pwd))
            {
                ViewBag.Error = "Введіть логін та пароль";
                return View();
            }

            var user = _repo.GetUserByNameAndPwd(Name, HashPassword(Pwd));

            if (user == null)
            {
                ViewBag.Error = "Невірний логін або пароль";
                return View();
            }

            var claims = new[] { new Claim(ClaimTypes.Name, user.Name) };
            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Guest()
            => RedirectToAction("Index", "Home");

        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_repo.UserExists(model.Name))
            {
                ModelState.AddModelError("Name", "Такий логін вже зайнятий");
                return View(model);
            }

            _repo.AddUser(new User
            {
                Name = model.Name,
                Pwd = HashPassword(model.Pwd)
            });
            _repo.Save();

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
