using LMS.Entity.ViewModels.Author;
using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace LMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class AuthorController : Controller
{
    private readonly IAuthorService _authorService;
    private readonly IToastNotification _toastNotification;

    public AuthorController(IAuthorService authorService, IToastNotification toastNotification)
    {
        _authorService = authorService;
        _toastNotification = toastNotification;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAuthorViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _authorService.CreateNewAuthorAsync(viewModel);
            if (result)
            {
                _toastNotification.AddSuccessToastMessage(
                    $"Author was created successfully. FullName:{viewModel.FullName}");
                return RedirectToAction(nameof(Index));
            }

            _toastNotification.AddErrorToastMessage("There are conflicting in your data.");
            return View();
        }

        _toastNotification.AddErrorToastMessage("Something went wrong");
        return View();
    }

    public async Task<IActionResult> Update(string id)
    {
        Console.WriteLine(id);
        var author = await _authorService.GetAuthorByIdWithUpdateViewModelAsync(id);
        if (author == null)
        {
            _toastNotification.AddErrorToastMessage($"Author doesn't exist. Id: {id}");
            return RedirectToAction(nameof(Index));
        }

        return View(author);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateAuthorViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _authorService.UpdateAuthorAsync(viewModel);
            if (result)
            {
                _toastNotification.AddSuccessToastMessage(
                    $"Author was updated successfully. FullName:{viewModel.FullName}");
                return RedirectToAction(nameof(Index));
            }

            _toastNotification.AddErrorToastMessage("There are conflicting in your data.");
            return View();
        }

        _toastNotification.AddErrorToastMessage("Something went wrong.");
        return View();
    }

    public async Task<IActionResult> Delete(string id)
    {
        var result = await _authorService.DeleteAuthorWithIdAsync(id);
        if (result)
        {
            _toastNotification.AddInfoToastMessage($"The author was deleted successfully. Id: {id}");
            return RedirectToAction(nameof(Index));
        }

        _toastNotification.AddErrorToastMessage("Something went wrong.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Get()
    {
        var authors = await _authorService.GetAllAuthorsAsync();
        return Json(authors.Select(p => new
        {
            p.FullName, p.BackStory, p.AmountOfBooks,
            UpdateLink = Url.Action("Update", "Author", new { Area = "Admin", id = p.Id }),
            DeleteLink = Url.Action("Delete", "Author", new { Area = "Admin", id = p.Id })
        }));
    }
}