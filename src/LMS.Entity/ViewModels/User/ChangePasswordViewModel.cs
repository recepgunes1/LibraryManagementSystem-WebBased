using System.ComponentModel.DataAnnotations;

namespace LMS.Entity.ViewModels.User;

public class ChangePasswordViewModel
{
    [Required, DataType(DataType.Password)]
    public string NewPassword { get; set; } = null!;
    
    [Required, DataType(DataType.Password), Compare(nameof(NewPassword))]
    public string NewPasswordConfirmation { get; set; } = null!;
    
    [Required, DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = null!;
}