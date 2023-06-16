using LMS.Entity.ViewModels.User;
using LMS.Service.Helpers.EmailService;
using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Areas.Account.Controllers;

[Area("Account")]
public class AuthController : Controller
{
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;

    public AuthController(IUserService userService, IEmailService emailService)
    {
        _userService = userService;
        _emailService = emailService;
    }

    public IActionResult Login()
    {
        if (IsUserAuthenticated()) return RedirectToAction(nameof(AccessDenied));

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
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
                ModelState.AddModelError(string.Empty,
                    "Your account has been locked out due to multiple unsuccessful login attempts. Please try again later or contact support.");
            else if (result.IsNotAllowed)
                ModelState.AddModelError(string.Empty,
                    "Your account has not been confirmed. Please confirm your account or contact support.");
            else
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return View(viewModel);
        }
        
        string? returnUrl = HttpContext.Request.Form["ReturnUrl"];
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        return Redirect("/");
    }

    public IActionResult ForgetPassword()
    {
        Console.WriteLine("worked1-get");
        if (IsUserAuthenticated()) return RedirectToAction(nameof(AccessDenied));
        Console.WriteLine("worked2-get");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel viewModel)
    {
        Console.WriteLine("worked1-post");
        var user = await _userService.GetUserByInputAsync(viewModel.EmailOrUsername);
        if (string.IsNullOrEmpty(user.Email))
        {
            ModelState.AddModelError(string.Empty, "User does not exist");
            return View(viewModel);
        }

        var token = await _userService.GetPasswordResetTokenAsync(user.Email);
        var link = Url.Action("ResetPassword", "Auth", new { Area = "Account", userId = user.Id, Token = token },
            HttpContext.Request.Scheme)!;
        await _emailService.SendResetPasswordEmail(link, user.Email);
        return View(viewModel);
    }

    public IActionResult ResetPassword(string userId, string token)
    {
        TempData["userId"] = userId;
        TempData["token"] = token;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
    {
        var userId = TempData["userId"]?.ToString();
        var token = TempData["token"]?.ToString();
        if (userId == null || token == null)
        {
            throw new Exception("There is an exception.");
        }

        var result = await _userService.ResetPasswordAsync(userId, token, viewModel.Password);
        if (result.Succeeded)
        {
            return RedirectToAction("Login", "Auth", new { Area = "Account" });
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }
        return View();
    }

    public IActionResult Register()
    {
        if (IsUserAuthenticated()) return RedirectToAction(nameof(AccessDenied));

        return View();
    }

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
        return RedirectToAction("All", "Books", new { area = "Home" });
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