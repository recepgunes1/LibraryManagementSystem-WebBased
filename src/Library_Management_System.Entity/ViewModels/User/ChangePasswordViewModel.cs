namespace Library_Management_System.Entity.ViewModels.User;

public class ChangePasswordViewModel
{
    public string NewPassword { get; set; } = null!;
    public string NewPasswordConfirmation { get; set; } = null!;
    public string CurrentPassword { get; set; } = null!;
}