using Application.InputModels.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Shared.Services;

public class MailService : IMailService
{
    private readonly SmtpConfiguration _config;
    private readonly ILogger<MailService> _logger;
    
    public MailService(IOptions<SmtpConfiguration> config, ILogger<MailService> logger)
    {
        _config = config.Value;
        _logger = logger;
    }

    public async Task SendAsync(MailRequest request)
    {
        try
        {
            var email = new MimeMessage
            {
                Sender = new MailboxAddress(_config.From, request.From ?? _config.From),
                Subject = request.Subject,
                Body = new BodyBuilder
                {
                    HtmlBody = request.Body
                }.ToMessageBody(),
            };
            email.To.Add(MailboxAddress.Parse(request.To));
            email.From.Add(MailboxAddress.Parse(_config.From));

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.Host, _config.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.UserName, _config.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }
    }
}