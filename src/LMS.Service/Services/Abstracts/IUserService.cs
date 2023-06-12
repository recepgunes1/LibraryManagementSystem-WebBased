using LMS.Entity.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace LMS.Service.Services.Abstracts;

public interface IUserService
{
    Task<string> GetCurrentUserFullNameAsync();
    Task<string> GetCurrentUserId();
    Task<string> GetCurrentUserRole();
    Task<IdentityResult> ChangeUserPasswordAsync(ChangePasswordViewModel viewModel);
    Task<IdentityResult> UpdateUserProfileInformationAsync(EditUserProfileViewModel viewModel);
    Task<EditUserProfileViewModel> GetCurrentUserProfileInformationAsync();
    Task<SignInResult> LoginAsync(LoginViewModel viewModel);
    Task<IdentityResult> RegisterAsync(RegisterViewModel viewModel);
    Task LogoutAsync();
    Task<IEnumerable<IndexUserViewModel>> GetAllUsersAsync();
    Task<Dictionary<string, string>> GetRolesWithIdAndNamesAsync();
    Task<IdentityResult> CreateNewUserAsync(CreateUserViewModel viewModel);
    Task<UpdateUserViewModel?> GetUserByIdWithUpdateViewModelAsync(string id);
    Task<IdentityResult> UpdateUserAsync(UpdateUserViewModel viewModel);
    Task<Dictionary<string, int>> CountUsersToRoleAsync();
    Task<int> GetMaxDaysClaimAsync();
    Task<string> GetMaxBooksClaimAsync();
}