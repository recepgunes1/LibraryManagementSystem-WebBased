using System.ComponentModel.DataAnnotations;

namespace LMS.Entity.ViewModels.User;

public class ResetPasswordViewModel
{
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string PasswordConfirmation { get; set; } = null!;
}