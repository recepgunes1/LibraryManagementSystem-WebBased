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

    public IActionResult Login()
    {
        if (IsUserAuthenticated())
        {
            return RedirectToAction(nameof(AccessDenied));
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (IsUserAuthenticated())
        {
            return RedirectToAction(nameof(AccessDenied));
        }

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty,
                "The submitted form is not valid. Please correct the errors and try again.");
            return View(viewModel);
        }

        var result = await _userService.LoginAsync(viewModel);
        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty,
                    "Your account has been locked out due to multiple unsuccessful login attempts. Please try again later or contact support.");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty,
                    "Your account has not been confirmed. Please confirm your account or contact support.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(viewModel);
        }
        
        string? returnUrl = HttpContext.Request.Query["returnUrl"];
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        
        return RedirectToAction("Index", "Home", new { area = "Admin" });
    }

    public IActionResult ResetPassword()
    {
        if (IsUserAuthenticated())
        {
            return RedirectToAction(nameof(AccessDenied));
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ResetPassword(ResetPasswordViewModel viewModel)
    {
        if (IsUserAuthenticated())
        {
            return RedirectToAction(nameof(AccessDenied));
        }

        return View(viewModel);
    }

    public IActionResult Register()
    {
        if (IsUserAuthenticated())
        {
            return RedirectToAction(nameof(AccessDenied));
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (IsUserAuthenticated())
        {
            return RedirectToAction(nameof(AccessDenied));
        }

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
        return View();
    }

    private bool IsUserAuthenticated()
    {
        var identity = User.Identity;
        return identity is { IsAuthenticated: true };
    }
}