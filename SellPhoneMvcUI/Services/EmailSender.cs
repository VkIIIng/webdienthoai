using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using SellPhoneMvcUI.Services;
namespace SellPhoneMvcUI.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                       ILogger<EmailSender> logger)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
    }

    public AuthMessageSenderOptions Options { get; } 

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(Options.SmtpPassword) || string.IsNullOrEmpty(Options.SmtpPassword))
        {
            throw new Exception("SMTP credentials are not configured.");
        }
        await Execute(Options.SmtpUsername, Options.SmtpPassword, subject, message, toEmail);
    }

    public async Task Execute(string smtpUsername, string smtpPassword, string subject, string message, string toEmail)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Password Recovery", smtpUsername));
        email.To.Add(new MailboxAddress("", toEmail));
        email.Subject = subject;
        email.Body = new TextPart("html") { Text = message };

        using (var client = new SmtpClient())
        {
            try
            {
                // Connect to Gmail SMTP server
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                // Authenticate with SMTP credentials
                await client.AuthenticateAsync(smtpUsername, smtpPassword);

                // Send the email
                await client.SendAsync(email);

                _logger.LogInformation($"Email to {toEmail} sent successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failure sending email to {toEmail}: {ex.Message}");
                throw;
            }
            finally
            {
                // Disconnect from the SMTP server
                await client.DisconnectAsync(true);
            }
        }
    }
}

