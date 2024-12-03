namespace EmailConnect
{
    public interface IEmailHelper
    {
        Task SendEmailAsync(string to, string subject, string body, string from = null, string[] cc = null, string[] bcc = null);
        Task SendEmailWithAttachmentAsync(string to, string subject, string body, string attachmentPath, string from = null, string[] cc = null, string[] bcc = null);
    }
}
