using Freezone.Core.Persistence.Repositories;

namespace Freezone.Core.Security.Entities;

public class OperationClaim: Entity
{
    public string Name { get; set; }
}