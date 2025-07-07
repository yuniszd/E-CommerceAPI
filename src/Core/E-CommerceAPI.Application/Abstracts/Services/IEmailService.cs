namespace E_CommerceAPI.Application.Abstracts.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}

