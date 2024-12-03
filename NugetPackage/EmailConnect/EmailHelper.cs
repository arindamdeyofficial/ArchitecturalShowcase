using CustomLoggerHelper;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace EmailConnect
{
    public class EmailHelper : IEmailHelper
    {
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;

        // Email configuration (e.g., SMTP server, port, sender email, etc.)
        private string _smtpServer;
        private int _smtpPort;
        private string _smtpUser;
        private string _smtpPassword;
        private bool _enableSsl;

        public EmailHelper(ILoggerHelper logger, IConfiguration configRoot)
        {
            _logger = logger;
            _configRoot = (IConfigurationRoot)configRoot;

            // Load email configuration from appsettings.json or environment variables
            _smtpServer = _configRoot["EmailSettings:SmtpServer"];
            _smtpPort = int.Parse(_configRoot["EmailSettings:SmtpPort"]);
            _smtpUser = _configRoot["EmailSettings:SmtpUser"];
            _smtpPassword = _configRoot["EmailSettings:SmtpPassword"];
            _enableSsl = bool.Parse(_configRoot["EmailSettings:EnableSsl"]);
        }
        public async Task SendEmailAsync(string to, string subject, string body, string from = null, string[] cc = null, string[] bcc = null)
        {
            try
            {
                // Default sender email if none is provided
                from = from ?? _configRoot["EmailSettings:FromEmail"];

                // Create the email message
                var message = new MailMessage
                {
                    From = new MailAddress(from),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true // If your email body is HTML, otherwise set to false
                };

                // Add the recipient, cc, and bcc if provided
                message.To.Add(to);
                if (cc != null)
                {
                    foreach (var email in cc)
                    {
                        message.CC.Add(email);
                    }
                }
                if (bcc != null)
                {
                    foreach (var email in bcc)
                    {
                        message.Bcc.Add(email);
                    }
                }

                // Set up the SMTP client
                using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
                    smtpClient.EnableSsl = _enableSsl;

                    // Send the email
                    await smtpClient.SendMailAsync(message);
                    _logger.LogInformation($"Email sent successfully to {to}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email.");
                throw new ApplicationException($"An error occurred while sending the email: {ex.Message}", ex);
            }
        }

        // Send an email with an attachment
        public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, string attachmentPath, string from = null, string[] cc = null, string[] bcc = null)
        {
            try
            {
                // Default sender email if none is provided
                from = from ?? _configRoot["EmailSettings:FromEmail"];

                // Create the email message
                var message = new MailMessage
                {
                    From = new MailAddress(from),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true // If your email body is HTML, otherwise set to false
                };

                // Add the recipient, cc, and bcc if provided
                message.To.Add(to);
                if (cc != null)
                {
                    foreach (var email in cc)
                    {
                        message.CC.Add(email);
                    }
                }
                if (bcc != null)
                {
                    foreach (var email in bcc)
                    {
                        message.Bcc.Add(email);
                    }
                }

                // Add the attachment
                if (!string.IsNullOrEmpty(attachmentPath) && System.IO.File.Exists(attachmentPath))
                {
                    var attachment = new Attachment(attachmentPath);
                    message.Attachments.Add(attachment);
                }
                else
                {
                    _logger.LogWarning("Attachment not found: " + attachmentPath);
                }

                // Set up the SMTP client
                using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
                    smtpClient.EnableSsl = _enableSsl;

                    // Send the email
                    await smtpClient.SendMailAsync(message);
                    _logger.LogInformation($"Email sent successfully with attachment to {to}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email with attachment.");
                throw new ApplicationException($"An error occurred while sending the email with attachment: {ex.Message}", ex);
            }
        }
    }
}
