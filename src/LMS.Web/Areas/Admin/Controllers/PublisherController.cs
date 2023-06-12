using LMS.Entity.ViewModels.Publisher;
using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace LMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class PublisherController : Controller
{
    private readonly IPublisherService _publisherService;
    private readonly IToastNotification _toastNotification;

    public PublisherController(IToastNotification toastNotification, IPublisherService publisherService)
    {
        _toastNotification = toastNotification;
        _publisherService = publisherService;
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
    public async Task<IActionResult> Create(CreatePublisherViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _publisherService.CreateNewPublisherAsync(viewModel);
            if (result)
            {
                _toastNotification.AddSuccessToastMessage(
                    $"Publisher was created successfully. Name:{viewModel.Name}");
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
        var publisher = await _publisherService.GetPublisherByIdWithUpdateViewModelAsync(id);
        if (publisher != null) return View(publisher);
        
        _toastNotification.AddErrorToastMessage($"Publisher doesn't exist. Id: {id}");
        return RedirectToAction(nameof(Index));

    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdatePublisherViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _publisherService.UpdatePublisherAsync(viewModel);
            if (result)
            {
                _toastNotification.AddSuccessToastMessage(
                    $"Publisher was updated successfully. Category:{viewModel.Name}");
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
        var result = await _publisherService.DeletePublisherWithIdAsync(id);
        if (result)
        {
            _toastNotification.AddInfoToastMessage($"The publisher was deleted successfully. Id: {id}");
            return RedirectToAction(nameof(Index));
        }

        _toastNotification.AddErrorToastMessage("Something went wrong.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Get()
    {
        var publishers = await _publisherService.GetAllPublishersNonDeletedAsync();
        return Json(publishers.Select(p => new
        {
            p.Name,
            BackStory = $"{(p.BackStory is { Length: > 100 } ? p.BackStory[..100].PadRight(103, '.') : p.BackStory)}",
            p.AmountOfBooks,
            UpdateLink = Url.Action("Update", "Publisher", new { Area = "Admin", id = p.Id }),
            DeleteLink = Url.Action("Delete", "Publisher", new { Area = "Admin", id = p.Id })
        }));
    }
}