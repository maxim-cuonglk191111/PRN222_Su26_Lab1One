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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string email, string password)
    {
        var member = _accountService.Authenticate(email, password);
        if (member == null)
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
