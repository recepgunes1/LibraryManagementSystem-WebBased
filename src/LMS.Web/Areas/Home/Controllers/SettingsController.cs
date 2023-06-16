using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Areas.Home.Controllers;

[Area("Home")]
public class SettingsController : Controller
{
    private readonly ISettingsService _settingsService;
    private readonly IUserService _userService;

    public SettingsController(ISettingsService settingsService, IUserService userService)
    {
        _settingsService = settingsService;
        _userService = userService;
    }

    [Route("/")]
    public async Task<IActionResult> Router()
    {
        if (!User.Identity!.IsAuthenticated) return RedirectToAction("Login", "Auth", new { Area = "Account" });

        var role = await _userService.GetCurrentUserRole();
        return role == "admin"
            ? RedirectToAction("Index", "Home", new { Area = "Admin" })
            : RedirectToAction("All", "Books", new { Area = "Home" });
    }

    [Authorize(Roles = "admin")]
    public IActionResult RunBogus()
    {
        _settingsService.LoadFakeData();
        return Redirect("/");
    }
}