using LMS.Entity.ViewModels.Dashboard;
using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class HomeController : Controller
{
    private readonly IAuthorService _authorService;
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;
    private readonly IPublisherService _publisherService;
    private readonly IUserService _userService;


    public HomeController(IAuthorService authorService, IBookService bookService, ICategoryService categoryService,
        IPublisherService publisherService, IUserService userService)
    {
        _authorService = authorService;
        _bookService = bookService;
        _categoryService = categoryService;
        _publisherService = publisherService;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardIndexViewModel
        {
            Authors = await _authorService.CountAuthorsAsync(),
            Books = await _bookService.CountBooksAsync(),
            Categories = await _categoryService.CountCategoriesAsync(),
            Publishers = await _publisherService.CountPublishersAsync()
        };
        return View(viewModel);
    }

    public async Task<IActionResult> GetUserInfo()
    {
        var data = await _userService.CountUsersToRoleAsync();
        return Json(data);
    }

    public async Task<IActionResult> GetBookInfo()
    {
        var data = await _bookService.CountBooksToStatusNonDeletedAsync();
        return Json(data);
    }
}