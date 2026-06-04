using Domain.Entities.Shared;
using Domain.Interfaces;
using Domain.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                    _emailSettings.DisplayName, _emailSettings.Email
            ));

                message.To.Add(new MailboxAddress("", to));

                message.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };
                message.Body = bodyBuilder.ToMessageBody();

                using (var smtpClient = new SmtpClient())
                {
                    var host = _emailSettings.Host;
                    var port = _emailSettings.Port;

                    var secureOption = port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;

                    await smtpClient.ConnectAsync(host, port, secureOption);

                    await smtpClient.AuthenticateAsync(
                        _emailSettings.Email,
                       _emailSettings.Password
                    );

                    await smtpClient.SendAsync(message);
                    await smtpClient.DisconnectAsync(true);
                }
           
        }
    }
}