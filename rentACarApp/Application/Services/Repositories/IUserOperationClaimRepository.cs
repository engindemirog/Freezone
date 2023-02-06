using Freezone.Core.Persistence.Repositories;
using Freezone.Core.Security.Entities;

namespace Application.Services.Repositories;

public interface IUserOperationClaimRepository : IRepository<UserOperationClaim>, IAsyncRepository<UserOperationClaim>
{
    
}