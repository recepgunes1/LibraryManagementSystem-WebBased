using Library_Management_System.Entity.ViewModels.User;
using Library_Management_System.Service.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Web.Areas.Account.Controllers;

[Area("Account")]
[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> EditProfile()
    {
        var user = await _userService.GetCurrentUserProfileInformationAsync();
        return View(user);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditProfile(EditUserProfileViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        var result = await _userService.UpdateUserProfileInformationAsync(viewModel);
        if (result.Succeeded)
        {
            ModelState.Clear();
            viewModel  = await _userService.GetCurrentUserProfileInformationAsync();
            AddSuccessMessageToTempData();
            return View(viewModel);
        }
        foreach (var identityError in result.Errors)
        {
            ModelState.AddModelError(identityError.Code, identityError.Description);
        }
        return View(viewModel);
    }

    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
    {
        var result = await _userService.ChangeUserPasswordAsync(viewModel);
        if (result.Succeeded)
        {
            AddSuccessMessageToTempData();
        }
        return View();
    }

    private void AddSuccessMessageToTempData()
    {
        TempData["success"] = "the operation was completed successfully.";
    }
}