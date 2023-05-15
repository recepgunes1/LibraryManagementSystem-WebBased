using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}