
using HotelManagement.Application.Contracts.Service;
using HotelManagement.Application.Exceptions;
using HotelManagement.Application.Models.Notification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;

namespace HotelManagement.Application.Services
{
    public class EmailService(IConfiguration config, ISmtpService smtpService, ILogger<EmailService> logger) : IEmailService
    {
        public async Task<EmailSendResponse> Send(string to, string subject, string body)
        {
            try
            {
                logger.LogInformation("Starting to send email to {Recipient}", to);

                ValidateAddressWhereEmailSent(to);
                logger.LogInformation("Validated recipient email: {Recipient}", to);

                var normalizeSubject = NormalizeSubject(subject);
                logger.LogInformation("Normalized subject: {Subject}", normalizeSubject);

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(config["EmailSettings:Sender"]));
                email.To.Add(MailboxAddress.Parse(to.Trim()));
                email.Subject = normalizeSubject;
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                logger.LogInformation("Connection to SMTP server: {Server}:{Port}", config["EmailSettings:SmtpServer"], config["EmailSettings:Port"]);

                await smtpService.ConnectAsync(
                    config["EmailSettings:SmtpServer"],
                    int.Parse(config["EmailSettings:Port"]),
                    bool.Parse(config["EmailSettings:UseSsl"])
                );

                logger.LogInformation("Authenticating with SMTP server...");

                await smtpService.AuthenticateAsync(
                    config["EmailSettings:Username"],
                    config["EmailSettings:Password"]
                );

                logger.LogInformation("Sending Email to {Recipient}", to);
                await smtpService.SendAsync(email);

                logger.LogInformation("Disconnecting from Smtp server...");
                await smtpService.DisconnectAsync(true);

                logger.LogInformation("Email sent successfully to {Recipient}", to);

                return new EmailSendResponse(true, $"Message sent successfully to: {to}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send email to {Recipient}: {Message}", to, ex.Message);
                return new EmailSendResponse(success: false, message: ex.Message, error: ex);
            }
        }

        //Private Helpers
        private string NormalizeSubject(string subject)
        {
            return string.IsNullOrWhiteSpace(subject) ? string.Empty : subject.Trim();
        }
        private void ValidateAddressWhereEmailSent(string to)
        {
            if (string.IsNullOrWhiteSpace(to))
                throw new BadRequestException("Email address cannot be empty.");

            try
            {
                var mailAddress = new MailAddress(to);
                if (!mailAddress.Address.Contains("@") || !mailAddress.Address.Contains("."))
                    throw new BadRequestException($"Invalid email address format {to}.");
            }
            catch
            {
                throw new BadRequestException($"Sending email {to} must be a valid email address.");
            }
        }
    }
}
