using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Areas.Home.Controllers;

[Area("Home")]
public class DataController : Controller
{
    private readonly ISettingsService _settingsService;

    public DataController(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public IActionResult RunBogus()
    {
        _settingsService.LoadFakeData();
        return Ok();
    }
}