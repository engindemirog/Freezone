namespace Freezone.Core.Mailing;

public class MailSettings // Konfigurasyon için
{
    public string Server { get; set; }
    public int Port { get; set; }
    public string SenderFullName { get; set; }
    public string SenderEmail { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    // ...
}