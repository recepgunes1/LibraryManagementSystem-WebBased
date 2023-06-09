using System.Security.Claims;
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
    private readonly RoleManager<Role> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

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

    public async Task<string> GetCurrentUserRole()
    {
        var user = await GetCurrentUserAsync();
        var roles = await _userManager.GetRolesAsync(user);
        return string.Join("", roles);
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
        var user = await _userManager.FindByEmailAsync(viewModel.EmailOrUsername) ??
                   await _userManager.FindByNameAsync(viewModel.EmailOrUsername);
        if (user == null) return SignInResult.Failed;
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

    public async Task<IEnumerable<IndexUserViewModel>> GetAllUsersAsync()
    {
        var users = _userManager.Users;
        var roles = await _roleManager.Roles.ToListAsync();

        var mappedUsers = new List<IndexUserViewModel>();

        foreach (var user in users)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            mappedUsers.AddRange(from role in roles
                where userRoles.Contains(role.Name!)
                select new IndexUserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    Role = role.Name!
                });
        }

        return mappedUsers;
    }

    public async Task<int> GetMaxBooksClaimAsync()
    {
        var role = (await _roleManager.FindByNameAsync(await GetCurrentUserRole()))!;
        if (role.Name == "admin") return 0;
        var claims = await _roleManager.GetClaimsAsync(role);
        var claim = claims.FirstOrDefault(c => c.Type == "MaxBooks")!;
        return Convert.ToInt32(claim.Value);
    }

    public async Task<string> GetUserEmailToInputAsync(string input)
    {
        var user = await _userManager.FindByEmailAsync(input) ??
                   await _userManager.FindByNameAsync(input);
        if (user == null)
        {
            return string.Empty;
        }

        return user.Email!;
    }

    public async Task<string> GetPasswordResetTokenAsync(string input)
    {
        var user = await _userManager.FindByEmailAsync(input);
        var token = await _userManager.GeneratePasswordResetTokenAsync(user!);
        return token;
    }

    public async Task<IndexUserViewModel> GetUserByInputAsync(string input)
    {
        var user = await _userManager.FindByEmailAsync(input) ??
                   await _userManager.FindByNameAsync(input);
        return user == null ? new IndexUserViewModel() : _mapper.Map<IndexUserViewModel>(user);
    }

    public async Task<IdentityResult> ResetPasswordAsync(string id, string token, string newPassword)
    {
        var hasUser = await _userManager.FindByIdAsync(id);

        if (hasUser == null)
        {
            return IdentityResult.Failed(new IdentityError
                { Code = string.Empty, Description = "User does not exist" });
        }

        var result = await _userManager.ResetPasswordAsync(hasUser, token, newPassword);
        return result;
    }

    public async Task<int> GetMaxDaysClaimAsync()
    {
        var role = (await _roleManager.FindByNameAsync(await GetCurrentUserRole()))!;
        if (role.Name == "admin") return 0;
        var claims = await _roleManager.GetClaimsAsync(role);
        var claim = claims.FirstOrDefault(c => c.Type == "MaxDays")!;
        return Convert.ToInt32(claim.Value);
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

    public async Task<Dictionary<string, int>> CountUsersToRoleAsync()
    {
        var admins = await _userManager.GetUsersInRoleAsync("admin");
        var lecturers = await _userManager.GetUsersInRoleAsync("lecturer");
        var students = await _userManager.GetUsersInRoleAsync("student");
        var userCountByRole = new Dictionary<string, int>
        {
            { "admin", admins.Count },
            { "lecturer", lecturers.Count },
            { "student", students.Count }
        };
        return userCountByRole;
    }

    private async Task<User> GetCurrentUserAsync()
    {
        var user = await _userManager.FindByNameAsync(_httpContext.User.FindFirst(ClaimTypes.Name)!.Value);
        if (user != null) return user;

        return new User();
    }
}