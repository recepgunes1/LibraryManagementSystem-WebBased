using LMS.Entity.ViewModels.Category;
using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace LMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
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

    public async Task<IActionResult> Create()
    {
        var viewModel = new CreateCategoryViewModel
        {
            Parents = await _categoryService.GetParentCategoriesAsync()
        };
        return View(viewModel);
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
                    $"Category was created successfully. FullName:{viewModel.Name}");
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
        var category = await _categoryService.GetCategoryByIdWithUpdateViewModelAsync(id);
        if (category == null)
        {
            _toastNotification.AddErrorToastMessage($"Category doesn't exist. Id: {id}");
            return RedirectToAction(nameof(Index));
        }
        
        category.Parents = await _categoryService.GetParentCategoriesAsync(category.Name);
        return View(category);
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
            return View();
        }

        _toastNotification.AddErrorToastMessage("Something went wrong.");
        return View();
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
            p.Name, p.BackStory, p.AmountOfBooks, p.ParentCategory,
            UpdateLink = Url.Action("Update", "Category", new { Area = "Admin", id = p.Id }),
            DeleteLink = Url.Action("Delete", "Category", new { Area = "Admin", id = p.Id })
        }));
    }
}