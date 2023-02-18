namespace Freezone.Core.Mailing;

public interface IMailService
{
    Task SendAsync(Mail mailData);
}