using LMS.Entity.ViewModels.Settings;
using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace LMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class SettingsController : Controller
{
    private readonly ISettingsService _settingsService;
    private readonly IToastNotification _toastNotification;

    public SettingsController(ISettingsService settingsService, IToastNotification toastNotification)
    {
        _settingsService = settingsService;
        _toastNotification = toastNotification;
    }

    public async Task<IActionResult> UpdateClaim()
    {
        var viewModel = new UpdateClaimsViewModel
        {
            RoleClaims = await _settingsService.GetRoleClaimsAsync()
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateClaim(UpdateClaimsViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.RoleClaims = await _settingsService.GetRoleClaimsAsync();
            _toastNotification.AddErrorToastMessage("Something went wrong.");
            return View(viewModel);
        }

        var result = await _settingsService.UpdateClaimAsync(viewModel.Role, viewModel.ClaimType, viewModel.NewValue);

        if (!result.Succeeded)
        {
            viewModel.RoleClaims = await _settingsService.GetRoleClaimsAsync();
            _toastNotification.AddErrorToastMessage(string.Join("<br/>", result.Errors));
            return View(viewModel);
        }

        _toastNotification.AddSuccessToastMessage(
            $"Update operation was successfully for {viewModel.Role} -> {viewModel.ClaimType}");
        return RedirectToAction(nameof(UpdateClaim));
    }
}