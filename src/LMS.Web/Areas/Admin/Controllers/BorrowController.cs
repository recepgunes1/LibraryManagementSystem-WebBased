using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class BorrowController : Controller
{
    public IActionResult Index()
    {
        return Ok();
    }
}