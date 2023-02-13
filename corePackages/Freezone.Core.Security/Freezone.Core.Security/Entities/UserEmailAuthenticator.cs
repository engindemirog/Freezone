using Freezone.Core.Persistence.Repositories;

namespace Freezone.Core.Security.Entities;

public class UserEmailAuthenticator : Entity
{
    public int UserId { get; set; }
    public string? Key { get; set; } // ASDASDADADLKlDŞKAS -> Verify // 123456 -> Login
    public bool IsVerified { get; set; } // 0 -> 1

    public virtual User User { get; set; }
}