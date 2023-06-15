using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Areas.Home.Controllers;

[Area("Home")]
public class SettingsController : Controller
{
    private readonly ISettingsService _settingsService;

    public SettingsController(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    [Route("/")]
    public IActionResult Router()
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Auth", new { Area = "Account" });
        }
        return Ok();
    }
    
    public IActionResult RunBogus()
    {
        _settingsService.LoadFakeData();
        return Ok();
    }
}