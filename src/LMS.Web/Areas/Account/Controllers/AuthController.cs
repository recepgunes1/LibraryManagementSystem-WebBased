using LMS.Entity.ViewModels.User;
using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Areas.Account.Controllers;

[Area("Account")]
public class AuthController : Controller
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "(1) Your credentials aren't confirmed.");
            return View();
        }

        var result = await _userService.LoginAsync(viewModel);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "(2) Your credentials aren't confirmed.");
            return View();
        }

        return RedirectToAction("Index", "Home", new { area = "Admin" });
    }

    public IActionResult ResetPassword()
    {
        return View();
    }


    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Your credentials aren't confirmed.");
            return View();
        }

        var result = await _userService.RegisterAsync(viewModel);
        if (!result.Succeeded)
        {
            foreach (var identityError in result.Errors)
                ModelState.AddModelError(identityError.Code, identityError.Description);
            return View();
        }

        await _userService.LoginAsync(new LoginViewModel
        {
            EmailOrUsername = viewModel.Email,
            Password = viewModel.Password,
            IsRememberMe = true
        });
        return RedirectToAction("List", "Book", new { area = "Home" });
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _userService.LogoutAsync();
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AccessDenied()
    {
        return BadRequest();
    }
}