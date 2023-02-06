using Freezone.Core.Persistence.Repositories;

namespace Freezone.Core.Security.Entities;

public class User:Entity
{
    public string FirstName { get; set; } 
    public string LastName { get; set; } 
    public string Email { get; set; } 
    public byte[] PasswordSalt { get; set; }
    // iMDuNue7EvE3
    public byte[] PasswordHash { get; set; }
    //  123456789ASD!
    public bool Status { get; set; }

    public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; }

    public User()
    {
        UserOperationClaims = new HashSet<UserOperationClaim>();
    }
}