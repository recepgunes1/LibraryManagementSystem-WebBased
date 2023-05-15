using AutoMapper;
using LMS.Entity.Entities;
using LMS.Entity.ViewModels.User;
using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LMS.Service.Services.Concretes;

public class UserService : IUserService
{
    private readonly HttpContext _httpContext;
    private readonly IMapper _mapper;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _mapper = mapper;
        _signInManager = signInManager;
        _httpContext = httpContextAccessor.HttpContext;
    }

    public async Task<EditUserProfileViewModel> GetCurrentUserProfileInformationAsync()
    {
        var user = await GetCurrentUserAsync();
        var mapped = _mapper.Map<EditUserProfileViewModel>(user);
        return mapped;
    }

    public async Task<IdentityResult> UpdateUserProfileInformationAsync(EditUserProfileViewModel viewModel)
    {
        var user = await GetCurrentUserAsync();
        var checkPassword = await _userManager.CheckPasswordAsync(user, viewModel.PasswordConfirmation);
        if (checkPassword)
        {
            _mapper.Map(viewModel, user);
            var checkUpdate = await _userManager.UpdateAsync(user);
            if (checkUpdate.Succeeded)
            {
                var checkSecurity = await _userManager.UpdateSecurityStampAsync(user);
                return checkSecurity;
            }

            return checkUpdate;
        }

        return IdentityResult.Failed(
            new IdentityError { Code = string.Empty, Description = "Your password is wrong" });
    }

    public async Task<string> GetCurrentUserFullNameAsync()
    {
        var user = await GetCurrentUserAsync();
        return $"{user.FirstName} {user.LastName}";
    }

    public async Task<string> GetCurrentUserId()
    {
        var user = await GetCurrentUserAsync();
        return user.Id;
    }

    public async Task<IdentityResult> ChangeUserPasswordAsync(ChangePasswordViewModel viewModel)
    {
        var user = await GetCurrentUserAsync();
        var result = await _userManager.ChangePasswordAsync(user, viewModel.CurrentPassword, viewModel.NewPassword);
        await _userManager.UpdateSecurityStampAsync(user);
        return result;
    }

    public async Task<SignInResult> LoginAsync(LoginViewModel viewModel)
    {
        var user = await _userManager.FindByEmailAsync(viewModel.EmailOrUsername);
        if (user != null)
        {
            var result =
                await _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.IsRememberMe, true);
            return result;
        }

        return new SignInResult();
    }

    public async Task<IdentityResult> RegisterAsync(RegisterViewModel viewModel)
    {
        var user = _mapper.Map<User>(viewModel);
        var result = await _userManager.CreateAsync(user, viewModel.Password);
        if (result.Succeeded) await _userManager.AddToRoleAsync(user, "student");

        return result;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    private async Task<User> GetCurrentUserAsync()
    {
        return (await _userManager.FindByNameAsync(_httpContext.User.Identity!.Name!))!;
    }
}