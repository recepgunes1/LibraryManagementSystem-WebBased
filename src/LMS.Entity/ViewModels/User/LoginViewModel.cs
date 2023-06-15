using System.ComponentModel.DataAnnotations;

namespace LMS.Entity.ViewModels.User;

public class LoginViewModel
{
    [Required]
    public string EmailOrUsername { get; set; } = default!;
    
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = default!;
    public bool IsRememberMe { get; set; } = false;
}