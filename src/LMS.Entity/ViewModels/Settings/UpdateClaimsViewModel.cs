namespace LMS.Entity.ViewModels.Settings;

public class UpdateClaimsViewModel
{
    public string Role { get; set; } = null!;
    public string ClaimType { get; set; } = null!;
    public string NewValue { get; set; } = null!;

    public Dictionary<string, Dictionary<string, string>>? RoleClaims { get; set; }
}