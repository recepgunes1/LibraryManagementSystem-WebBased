using System.Security.Claims;
using LMS.Data.Context;
using LMS.Data.Seeding;
using LMS.Entity.Entities;
using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Identity;

namespace LMS.Service.Services.Concretes;

public class SettingsService : ISettingsService
{
    private readonly AppDbContext _dbContext;
    private readonly RoleManager<Role> _roleManager;

    public SettingsService(AppDbContext dbContext, RoleManager<Role> roleManager)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
    }

    public void LoadFakeData()
    {
        DbInitializer.Initialize(_dbContext);
    }

    public async Task<Dictionary<string, Dictionary<string, string>>> GetRoleClaimsAsync()
    {
        var roleClaims = new Dictionary<string, Dictionary<string, string>>();


        foreach (var role in _roleManager.Roles)
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            var claimsDict = new Dictionary<string, string>();

            foreach (var claim in claims) claimsDict[claim.Type] = claim.Value;

            if (claimsDict.Count > 0) roleClaims[role.Name!] = claimsDict;
        }

        return roleClaims;
    }

    public async Task<IdentityResult> UpdateClaimAsync(string roleName, string claimType, string claimValue)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
            return IdentityResult.Failed(new IdentityError
                { Code = string.Empty, Description = "Role does not exist." });

        var claims = await _roleManager.GetClaimsAsync(role);
        var claim = claims.FirstOrDefault(p => p.Type == claimType);
        if (claim == null)
            return IdentityResult.Failed(new IdentityError
                { Code = string.Empty, Description = $"Claim does not exist for {role.Name!} role." });

        var remove = await _roleManager.RemoveClaimAsync(role, claim);
        if (!remove.Succeeded) return remove;

        var newClaim = new Claim(claimType, claimValue);
        var result = await _roleManager.AddClaimAsync(role, newClaim);
        return result;
    }
}