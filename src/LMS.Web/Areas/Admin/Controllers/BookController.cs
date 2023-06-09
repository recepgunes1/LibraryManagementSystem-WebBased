using LMS.Entity.ViewModels.Book;
using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace LMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class BookController : Controller
{
    private readonly IAuthorService _authorService;
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;
    private readonly IPublisherService _publisherService;
    private readonly IToastNotification _toastNotification;

    public BookController(IAuthorService authorService, ICategoryService categoryService,
        IPublisherService publisherService, IToastNotification toastNotification, IBookService bookService)
    {
        _authorService = authorService;
        _categoryService = categoryService;
        _publisherService = publisherService;
        _toastNotification = toastNotification;
        _bookService = bookService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Update(string id)
    {
        return View();
    }

    public IActionResult Delete(string id)
    {
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Create()
    {
        var viewModel = new CreateBookViewModel
        {
            Authors = await _authorService.GetAuthorsWithKeyAndNameAsync(),
            Publishers = await _publisherService.GetPublishersWithKeyAndNameAsync(),
            Categories = await _categoryService.GetCategoriesWithKeyAndNameAsync()
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _bookService.CreateNewBookAsync(viewModel);
            if (result)
            {
                _toastNotification.AddSuccessToastMessage(
                    $"Book was created successfully. FullName:{viewModel.Name}");
                return RedirectToAction(nameof(Index));
            }

            _toastNotification.AddErrorToastMessage("There are conflicting in your data.");
            return View();
        }

        _toastNotification.AddErrorToastMessage("Something went wrong");
        return View();
    }


    public async Task<IActionResult> Get()
    {
        var books = await _bookService.GetAllBooksNonDeletedAsync();
        return Json(books.Select(p => new
        {
            p.Name, p.Category, p.Publisher, p.Author,
            UpdateLink = Url.Action("Update", "Author", new { Area = "Admin", id = p.Id }),
            DeleteLink = Url.Action("Delete", "Author", new { Area = "Admin", id = p.Id })
        }));
    }
}