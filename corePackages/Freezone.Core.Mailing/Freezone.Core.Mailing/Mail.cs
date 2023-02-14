using MimeKit;

namespace Freezone.Core.Mailing;

public class Mail // MailService için
{
    public string Subject { get; set; }
    public string TextBody { get; set; }
    public string HtmlBody { get; set; }
    public AttachmentCollection? Attachments { get; set; }
    public string ToFullName { get; set; }
    public string ToEmail { get; set; }
    // ...
}

