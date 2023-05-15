using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Areas.Home.Controllers;

[Area("Home")]
[Authorize(Roles = "student, lecturer")]
public class BookController : Controller
{
    public IActionResult List()
    {
        return View();
    }
}