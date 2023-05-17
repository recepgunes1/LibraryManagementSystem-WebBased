namespace LMS.Entity.ViewModels.User;

public class UpdateUserViewModel
{
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string RoleId { get; set; } = null!;
    public Dictionary<string, string> Roles { get; set; } = new();
}