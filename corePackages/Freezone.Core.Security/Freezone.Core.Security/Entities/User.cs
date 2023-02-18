using Freezone.Core.Persistence.Repositories;
using Freezone.Core.Security.Authenticator;

namespace Freezone.Core.Security.Entities;

public class User:Entity
{
    public string FirstName { get; set; } 
    public string LastName { get; set; } 
    public string Email { get; set; } 
    public byte[] PasswordSalt { get; set; } // iMDuNue7EvE3 (byte[])
    public byte[] PasswordHash { get; set; } //  123456789ASD! (byte[])
    public AuthenticatorType AuthenticatorType { get; set; }
    public bool Status { get; set; }

    public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    public virtual UserEmailAuthenticator UserEmailAuthenticator { get; set; }
    public virtual UserOtpAuthenticator UserOtpAuthenticator { get; set; }

    public User()
    {
        UserOperationClaims = new HashSet<UserOperationClaim>();
        RefreshTokens = new HashSet<RefreshToken>();
    }
}