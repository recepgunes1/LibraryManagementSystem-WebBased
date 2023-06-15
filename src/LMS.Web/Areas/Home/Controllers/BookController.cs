using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace LMS.Web.Areas.Home.Controllers;

[Area("Home")]
[Authorize(Roles = "student, lecturer")]
public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly IBorrowService _borrowService;
    private readonly IToastNotification _toastNotification;

    public BookController(IBookService bookService, IToastNotification toastNotification, IBorrowService borrowService)
    {
        _bookService = bookService;
        _toastNotification = toastNotification;
        _borrowService = borrowService;
    }

    public IActionResult All()
    {
        return View();
    }

    public IActionResult History()
    {
        return View();
    }

    //[Authorize(Policy = "BookBorrowingCheck")]
    public async Task<IActionResult> Borrow(string id)
    {
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"Type:{claim.Type}\t\t\tValue:{claim.Value}");
        }
        var flag = User.Claims.Any(c => c is { Type: "borrowable", Value: "false" });
        if (flag)
        {
            _toastNotification.AddErrorToastMessage("You can't borrow any books right now. Check you history!");
            return RedirectToAction(nameof(History));
        }
        await _borrowService.BorrowAsync(id);
        _toastNotification.AddInfoToastMessage($"The book was borrowed successfully. BookId: {id}");
        return RedirectToAction(nameof(History));
    }

    public async Task<IActionResult> Return(string id)
    {
        await _borrowService.ReturnAsync(id);
        _toastNotification.AddInfoToastMessage($"The book was returned successfully. OperationId: {id}");
        return RedirectToAction(nameof(History));
    }

    public async Task<IActionResult> Detail(string id)
    {
        var book = await _bookService.GetBookByIdWithDetailBookViewModelAsync(id);
        if (book != null) return View(book);

        _toastNotification.AddErrorToastMessage($"Book doesn't exist. Id: {id}");
        return RedirectToAction(nameof(All));
    }

    public async Task<IActionResult> GetBorrowable()
    {
        var books = await _bookService.GetBorrowableBooksAsync();
        return Json(books.Select(p => new
        {
            p.Name, p.Category, p.Publisher, p.Author, Cover = p.ImagePath,
            BorrowLink = Url.Action("Borrow", "Book", new { Area = "Home", id = p.Id }),
            DetailLink = Url.Action("Detail", "Book", new { Area = "Home", id = p.Id })
        }));
    }

    public async Task<IActionResult> GetHistory()
    {
        var books = await _bookService.GetBorrowedBookHistoryAsync();
        return Json(books.Select(p => new
        {
            p.Book, p.BorrowDateTime, p.ReturnDateTime, p.Status, p.ButtonVisibility,
            ReturnUrl = Url.Action("Return", "Book", new { Area = "Home", id = p.Id })
        }));
    }
}