using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace LMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class BorrowController : Controller
{
    private readonly IBorrowService _borrowService;
    private readonly IToastNotification _toastNotification;


    public BorrowController(IBorrowService borrowService, IToastNotification toastNotification)
    {
        _borrowService = borrowService;
        _toastNotification = toastNotification;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Approve(string id)
    {
        await _borrowService.ApproveAsync(id);
        _toastNotification.AddInfoToastMessage($"The book was approved successfully. Operation Id: {id}");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Get()
    {
        var borrows = await _borrowService.GetAllBorrowsAsync();
        return Json(borrows.Select(p => new
        {
            p.Id, p.User, p.Book, p.Status, p.BorrowDateTime, p.ReturnDateTime, p.ButtonVisibility,
            ApproveLink = Url.Action("Approve", "Borrow", new { Area = "Admin", id = p.Id })
        }));
    }
}