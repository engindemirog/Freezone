using Freezone.Core.Persistence.Repositories;

namespace Freezone.Core.Security.Entities;

public class UserOtpAuthenticator : Entity
{
    public int UserId { get; set; }
    public byte[] SecretKey { get; set; }
    public bool IsVerified { get; set; }

    public virtual User User { get; set; }
}