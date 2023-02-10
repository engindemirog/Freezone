using Freezone.Core.Persistence.Repositories;

namespace Freezone.Core.Security.Entities;

public class UserOperationClaim: Entity
{
    public int UserId { get; set; }
    public int OperationClaimId { get; set; }

    public virtual User User { get; set; }
    public virtual OperationClaim OperationClaim { get; set; }
}