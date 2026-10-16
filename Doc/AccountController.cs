using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayMVC.Data;
using RaceDayMVC.Models;
using RaceDayMVC.Services;
using RaceDayMVC.ViewModels;

namespace RaceDayMVC.Controllers;

public class AccountController : Controller
{
    private readonly RaceDayContext _db;
    private readonly IPasswordService _passwords;
    public AccountController(RaceDayContext db, IPasswordService passwords) { _db = db; _passwords = passwords; }

    [HttpGet] public IActionResult Register() => View();
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (model.Role is not ("Participant" or "Organiser")) ModelState.AddModelError(nameof(model.Role), "Choose Participant or Organiser.");
        if (await _db.Users.AnyAsync(u => u.Email == model.Email)) ModelState.AddModelError(nameof(model.Email), "This email is already registered.");
        if (!ModelState.IsValid) return View(model);
        var user = new User { FullName = model.FullName.Trim(), Email = model.Email.Trim().ToLowerInvariant(),
            PasswordHash = _passwords.Hash(model.Password), Role = model.Role, PhoneNumber = model.PhoneNumber };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        await SignIn(user);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet] public IActionResult Login() => View();
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == model.Email.Trim().ToLowerInvariant());
        if (user == null || !_passwords.Verify(model.Password, user.PasswordHash))
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }
        await SignIn(user);
        return RedirectToAction("Index", "Home");
    }

    private async Task SignIn(User user)
    {
        var claims = new List<Claim> {
            new(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
    }

    [HttpPost, Microsoft.AspNetCore.Authorization.Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied() => View();
}
