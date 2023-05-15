using LMS.Entity.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace LMS.Service.Services.Abstracts;

public interface IUserService
{
    Task<string> GetCurrentUserFullNameAsync();
    Task<string> GetCurrentUserId();
    Task<IdentityResult> ChangeUserPasswordAsync(ChangePasswordViewModel viewModel);
    Task<IdentityResult> UpdateUserProfileInformationAsync(EditUserProfileViewModel viewModel);
    Task<EditUserProfileViewModel> GetCurrentUserProfileInformationAsync();
    Task<SignInResult> LoginAsync(LoginViewModel viewModel);
    Task<IdentityResult> RegisterAsync(RegisterViewModel viewModel);
    Task LogoutAsync();
}