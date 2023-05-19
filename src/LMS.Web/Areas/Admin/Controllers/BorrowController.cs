using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class BorrowController : Controller
{
    private readonly IBorrowService _borrowService;

    public BorrowController(IBorrowService borrowService)
    {
        _borrowService = borrowService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Approve(string id)
    {
        return Ok();
    }

    public async Task<IActionResult> Get()
    {
        var borrows = await _borrowService.GetAllBorrowsAsync();
        return Json(borrows.Select(p => new
        {
            p.Id, p.User, p.Book, p.IsReturned, p.IsApproved, p.BorrowDateTime, p.ReturnDateTime,
            ApproveLink = Url.Action("Approve", "Borrow", new { Area = "Admin", id = p.Id })
        }));
    }
}
