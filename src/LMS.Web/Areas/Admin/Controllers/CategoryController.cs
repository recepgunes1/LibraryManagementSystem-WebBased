using LMS.Entity.ViewModels.Category;
using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace LMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IToastNotification _toastNotification;

    public CategoryController(ICategoryService categoryService, IToastNotification toastNotification)
    {
        _categoryService = categoryService;
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
    public async Task<IActionResult> Create(CreateCategoryViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _categoryService.CreateNewCategoryAsync(viewModel);
            if (result)
            {
                _toastNotification.AddSuccessToastMessage(
                    $"Category was created successfully. Name:{viewModel.Name}");
                return RedirectToAction(nameof(Index));
            }

            _toastNotification.AddErrorToastMessage("There are conflicting in your data.");
            return RedirectToAction(nameof(Create));
        }

        _toastNotification.AddErrorToastMessage("Something went wrong");
        return RedirectToAction(nameof(Create));
    }

    public async Task<IActionResult> Update(string id)
    {
        var category = await _categoryService.GetCategoryByIdWithUpdateViewModelAsync(id);
        if (category != null) return View(category);

        _toastNotification.AddErrorToastMessage($"Category doesn't exist. Id: {id}");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateCategoryViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _categoryService.UpdateCategoryAsync(viewModel);
            if (result)
            {
                _toastNotification.AddSuccessToastMessage(
                    $"Category was updated successfully. Category:{viewModel.Name}");
                return RedirectToAction(nameof(Index));
            }

            _toastNotification.AddErrorToastMessage("There are conflicting in your data.");
            return RedirectToAction(nameof(Update));
        }

        _toastNotification.AddErrorToastMessage("Something went wrong.");
        return RedirectToAction(nameof(Update));
    }

    public async Task<IActionResult> Delete(string id)
    {
        var result = await _categoryService.DeleteCategoryWithIdAsync(id);
        if (result)
        {
            _toastNotification.AddInfoToastMessage($"The category was deleted successfully. Id: {id}");
            return RedirectToAction(nameof(Index));
        }

        _toastNotification.AddErrorToastMessage("Something went wrong.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Get()
    {
        var categories = await _categoryService.GetAllCategoriesNonDeletedAsync();
        return Json(categories.Select(p => new
        {
            p.Name,
            BackStory = $"{(p.BackStory is { Length: > 100 } ? p.BackStory[..100].PadRight(103, '.') : p.BackStory)}",
            p.AmountOfBooks,
            UpdateLink = Url.Action("Update", "Category", new { Area = "Admin", id = p.Id }),
            DeleteLink = Url.Action("Delete", "Category", new { Area = "Admin", id = p.Id })
        }));
    }
}