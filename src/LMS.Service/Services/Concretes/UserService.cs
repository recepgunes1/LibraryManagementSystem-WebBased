using AutoMapper;
using LMS.Entity.Entities;
using LMS.Entity.ViewModels.User;
using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS.Service.Services.Concretes;

public class UserService : IUserService
{
    private readonly HttpContext _httpContext;
    private readonly IMapper _mapper;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public UserService(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager,
        IHttpContextAccessor httpContextAccessor, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _signInManager = signInManager;
        _roleManager = roleManager;
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
        if (user == null) return new SignInResult();
        var result =
            await _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.IsRememberMe, true);
        return result;
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
        var user = await _userManager.FindByNameAsync(_httpContext.User.Identity!.Name!);
        if (user != null)
        {
            return user;
        }

        return new();
    }

    // public async Task<IEnumerable<IndexUserViewModel>> GetAllUsersAsync()
    // {
    //     var users = await _userManager.Users.ToListAsync();
    //     var mappedUsers = new List<IndexUserViewModel>();
    //     foreach (var user in users)
    //     {
    //         foreach (var role in _roleManager.Roles)
    //         {
    //             if (await _userManager.IsInRoleAsync(user, role.Name!))
    //             {
    //                 mappedUsers.Add(new()
    //                 {
    //                     Id = user.Id,
    //                     FirstName = user.FirstName,
    //                     LastName = user.LastName,
    //                     Email = user.Email!,
    //                     Role = role.Name!,
    //                 });
    //             }
    //         }
    //     }
    //     return mappedUsers;
    // }

    public async Task<IEnumerable<IndexUserViewModel>> GetAllUsersAsync()
    {
        var users = _userManager.Users;
        var roles = await _roleManager.Roles.ToListAsync();

        var mappedUsers = new List<IndexUserViewModel>();

        foreach (var user in users)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                if (userRoles.Contains(role.Name!))
                {
                    mappedUsers.Add(new IndexUserViewModel
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email!,
                        Role = role.Name!,
                    });
                }
            }
        }

        return mappedUsers;
    }


    public async Task<Dictionary<string, string>> GetRolesWithIdAndNamesAsync()
    {
        return (await _roleManager.Roles.ToDictionaryAsync(p => p.Id, p => p.Name))!;
    }

    public async Task<IdentityResult> CreateNewUserAsync(CreateUserViewModel viewModel)
    {
        var mappedUser = _mapper.Map<User>(viewModel);
        var result = await _userManager.CreateAsync(mappedUser, viewModel.Password);
        if (!result.Succeeded) return result;

        var user = (await _userManager.FindByEmailAsync(viewModel.Email))!;
        var role = (await _roleManager.FindByIdAsync(viewModel.RoleId))!;
        await _userManager.AddToRoleAsync(user, role.Name!);
        return result;
    }

    public async Task<UpdateUserViewModel?> GetUserByIdWithUpdateViewModelAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return null;
        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()!;
        var mapped = _mapper.Map<UpdateUserViewModel>(user);
        mapped.RoleId = (await _roleManager.FindByNameAsync(role))!.Id;
        mapped.Roles = await GetRolesWithIdAndNamesAsync();
        return mapped;
    }

    public async Task<IdentityResult> UpdateUserAsync(UpdateUserViewModel viewModel)
    {
        var user = (await _userManager.FindByIdAsync(viewModel.Id))!;
        var oldRole = (await _userManager.GetRolesAsync(user)).First();
        var newRole = (await _roleManager.FindByIdAsync(viewModel.RoleId))!.Name!;
        _mapper.Map(viewModel, user);
        var result = await _userManager.UpdateAsync(user);

        if (oldRole == newRole) return result;

        await _userManager.RemoveFromRoleAsync(user, oldRole);
        await _userManager.AddToRoleAsync(user, newRole);
        return result;
    }
}