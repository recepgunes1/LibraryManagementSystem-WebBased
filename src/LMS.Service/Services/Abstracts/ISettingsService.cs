using Microsoft.AspNetCore.Identity;

namespace LMS.Service.Services.Abstracts;

public interface ISettingsService
{
    void LoadFakeData();
    Task<Dictionary<string, Dictionary<string, string>>> GetRoleClaimsAsync();
    Task<IdentityResult> UpdateClaimAsync(string roleName, string claimType, string claimValue);
}