namespace LMS.Service.OptionModels;

public class EmailSettings
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    
}