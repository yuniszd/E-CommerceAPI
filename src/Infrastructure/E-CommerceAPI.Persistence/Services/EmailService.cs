namespace E_CommerceAPI.Persistence.Services;

using E_CommerceAPI.Application.Abstracts.Services;
using E_CommerceAPI.Application.Shared.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MailMessage();
        message.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
        message.To.Add(new MailAddress(toEmail));
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;

        using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
        {
            client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
            client.EnableSsl = true;
            await client.SendMailAsync(message);
        }
    }
}
