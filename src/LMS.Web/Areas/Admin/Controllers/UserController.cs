using LMS.Entity.ViewModels.User;
using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace LMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IToastNotification _toastNotification;

    public UserController(IUserService userService, IToastNotification toastNotification)
    {
        _userService = userService;
        _toastNotification = toastNotification;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Create()
    {
        var viewModel = new CreateUserViewModel()
        {
            Roles = await _userService.GetRolesWithIdAndNamesAsync(),
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.CreateNewUserAsync(viewModel);
            if (result.Succeeded)
            {
                _toastNotification.AddSuccessToastMessage(
                    $"Publisher was created successfully. Email:{viewModel.Email}");
                return RedirectToAction(nameof(Index));
            }

            _toastNotification.AddErrorToastMessage(string.Join(Environment.NewLine,
                result.Errors.Select(p => p.Description)));
            viewModel.Roles = await _userService.GetRolesWithIdAndNamesAsync();
            return View(viewModel);
        }

        viewModel.Roles = await _userService.GetRolesWithIdAndNamesAsync();
        return View(viewModel);
    }

    public IActionResult Delete()
    {
        return Ok();
    }

    public async Task<IActionResult> Update(string id)
    {
        var user = await _userService.GetUserByIdWithUpdateViewModelAsync(id);
        if (user != null) return View(user);
        _toastNotification.AddErrorToastMessage($"User doesn't exist. Id: {id}");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateUserViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.UpdateUserAsync(viewModel);
            if (result.Succeeded)
            {
                _toastNotification.AddSuccessToastMessage(
                    $"User was updated successfully. User ID:{viewModel.Id}");
                return RedirectToAction(nameof(Index));
            }

            _toastNotification.AddErrorToastMessage(string.Join(Environment.NewLine,
                result.Errors.Select(p => p.Description)));
        }

        if (!_toastNotification.GetToastMessages().Any())
        {
            _toastNotification.AddErrorToastMessage("Something went wrong.");
        }

        viewModel.Roles = await _userService.GetRolesWithIdAndNamesAsync();
        return View(viewModel);
    }

    public async Task<IActionResult> Get()
    {
        var users = await _userService.GetAllUsersAsync();
        return Json(users.Select(p => new
        {
            p.FirstName, p.LastName, p.Email, p.AmountOfBooks, p.Role,
            UpdateLink = Url.Action("Update", "User", new { Area = "Admin", id = p.Id }),
        }));
    }
}