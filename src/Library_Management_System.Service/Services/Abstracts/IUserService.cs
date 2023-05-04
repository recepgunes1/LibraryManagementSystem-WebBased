using Library_Management_System.Entity.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace Library_Management_System.Service.Services.Abstracts;

public interface IUserService
{
    Task<string> GetCurrentUserFullNameAsync();
    Task<IdentityResult> ChangeUserPasswordAsync(ChangePasswordViewModel viewModel);
    Task<IdentityResult> UpdateUserProfileInformationAsync(EditUserProfileViewModel viewModel);
    Task<EditUserProfileViewModel> GetCurrentUserProfileInformationAsync();
    Task<SignInResult> LoginAsync(LoginViewModel viewModel);
    Task<IdentityResult> RegisterAsync(RegisterViewModel viewModel);
    Task LogoutAsync();
}