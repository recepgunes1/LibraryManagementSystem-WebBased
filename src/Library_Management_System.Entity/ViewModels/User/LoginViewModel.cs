namespace Library_Management_System.Entity.ViewModels.User;

public class LoginViewModel
{
    public string EmailOrUsername { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool IsRememberMe { get; set; } = false;
}