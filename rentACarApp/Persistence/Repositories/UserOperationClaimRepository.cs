using Application.Services.Repositories;
using Freezone.Core.Persistence.Repositories;
using Freezone.Core.Security.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class UserOperationClaimRepository : EfRepositoryBase<UserOperationClaim, BaseDbContext>, IUserOperationClaimRepository
{
    public UserOperationClaimRepository(BaseDbContext context) : base(context)
    {
    }
}