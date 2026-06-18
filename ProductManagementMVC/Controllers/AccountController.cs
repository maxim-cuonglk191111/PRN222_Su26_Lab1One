using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ProductManagementMVC.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("UserId") != null)
            return RedirectToAction("Index", "Products");
        return View();
    }

    private string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password)
    {
        var member = await _accountService.GetAccountByEmailAsync(email);
        
        // Allow both plain text (Lab 2) and SHA256 (Lab 1) for compatibility during sync
        if (member == null || (member.MemberPassword != password && member.MemberPassword != HashPassword(password)))
        {
            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        HttpContext.Session.SetString("UserId", member.MemberID);
        HttpContext.Session.SetString("Username", member.FullName);
        HttpContext.Session.SetInt32("MemberRole", member.MemberRole);
        return RedirectToAction("Index", "Products");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
