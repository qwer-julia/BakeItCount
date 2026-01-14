namespace BakeItCountApi.Services
{
    using MailKit.Net.Smtp;
    using MimeKit;
    using Microsoft.Extensions.Options;
    using BakeItCountApi.Models;
    using System.Net.Mail;

    public class EmailService
    {
        private readonly SmtpSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SmtpSettings> options, ILogger<EmailService> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var msg = new MimeMessage();
            msg.From.Add(MailboxAddress.Parse(_settings.From));
            msg.To.Add(MailboxAddress.Parse(to));
            msg.Subject = subject;
            msg.Body = new TextPart("plain") { Text = body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await smtp.ConnectAsync(_settings.Host, _settings.Port, _settings.UseSsl);
                if (!string.IsNullOrWhiteSpace(_settings.User))
                    await smtp.AuthenticateAsync(_settings.User, _settings.Password);
                await smtp.SendAsync(msg);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }

            _logger.LogInformation("Email queued/sent to {Recipient}", to);
        }
    }
}
