using System.Net;
using System.Net.Mail;
using Lilumi.Art.Application.Interfaces;
using Lilumi.Art.Infrastructure.Settings;

namespace Lilumi.Art.Infrastructure.Services;

public class SmtpEmailService(SmtpSettings smtpSettings) : IEmailService
{
    public async Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        using var message = new MailMessage(smtpSettings.FromEmail, string.IsNullOrWhiteSpace(to) ? smtpSettings.ToEmail : to, subject, body);
        using var client = new SmtpClient(smtpSettings.Host, smtpSettings.Port)
        {
            EnableSsl = smtpSettings.EnableSsl,
            Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password)
        };

        await client.SendMailAsync(message, cancellationToken);
    }
}
