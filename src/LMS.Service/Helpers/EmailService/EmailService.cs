using System.Net;
using System.Net.Mail;
using LMS.Service.OptionModels;
using Microsoft.Extensions.Options;

namespace LMS.Service.Helpers.EmailService;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _emailSettings = options.Value;
    }

    public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail)
    {
        var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port);

        smtpClient.UseDefaultCredentials = false;
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);

        var mailMessage = new MailMessage();

        mailMessage.From = new MailAddress(_emailSettings.Email);
        mailMessage.To.Add(toEmail);

        mailMessage.Subject = "Password Reset Link";
        mailMessage.Body = @$"<h4>To reset your password, click on the url below:</h4><hr/><p><a href='{resetPasswordEmailLink}'>Reset Password</a></p>";

        mailMessage.IsBodyHtml = true;
        
        await smtpClient.SendMailAsync(mailMessage);

    }

}