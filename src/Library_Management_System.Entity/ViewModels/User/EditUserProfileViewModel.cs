namespace Library_Management_System.Entity.ViewModels.User;

public class EditUserProfileViewModel
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string PasswordConfirmation { get; set; } = null!;
}
