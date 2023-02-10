using Application.Services.Repositories;
using Freezone.Core.Persistence.Repositories;
using Freezone.Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class UserOperationClaimRepository : EfRepositoryBase<UserOperationClaim, BaseDbContext>,
                                            IUserOperationClaimRepository
{
    public UserOperationClaimRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<ICollection<OperationClaim>> GetOperationClaimsByUserIdAsync(int userId)
    {
        return await Query().AsNoTracking()
                            .Where(uoc => uoc.UserId == userId)
                            .Include(uoc => uoc.OperationClaim)
                            .Select(uoc => new OperationClaim
                            {
                                Id = uoc.OperationClaimId,
                                Name = uoc.OperationClaim.Name
                            }).ToListAsync();
    }
}