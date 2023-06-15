namespace LMS.Service.Helpers.EmailService;

public interface IEmailService
{
    Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail);
}