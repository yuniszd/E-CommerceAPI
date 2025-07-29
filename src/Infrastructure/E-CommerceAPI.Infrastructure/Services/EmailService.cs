namespace E_CommerceAPI.Infrastructure.Services;

using NETCore.MailKit.Core;
using IEmailService = Application.Abstracts.Services.IEmailService;

public class EmailService : IEmailService
{
    private readonly NETCore.MailKit.Core.IEmailService _mailKitEmailService;

    public EmailService(NETCore.MailKit.Core.IEmailService mailKitEmailService)
    {
        _mailKitEmailService = mailKitEmailService;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        await _mailKitEmailService.SendAsync(toEmail, subject, body, isHtml: true);
    }
}

