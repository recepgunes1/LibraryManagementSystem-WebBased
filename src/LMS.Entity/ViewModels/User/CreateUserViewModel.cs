using System.ComponentModel.DataAnnotations;

namespace LMS.Entity.ViewModels.User;

public class CreateUserViewModel
{
    [Required]
    public string FirstName { get; set; } = null!;
    
    [Required]
    public string LastName { get; set; } = null!;
    
    [Required]
    public string UserName { get; set; } = null!;
    
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    
    [Required]
    public string RoleId { get; set; } = null!;
    
    public Dictionary<string, string> Roles { get; set; } = new();
}