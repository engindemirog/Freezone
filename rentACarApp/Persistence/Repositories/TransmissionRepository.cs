using Application.Services.Repositories;
using Domain.Entities;
using Freezone.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class TransmissionRepository : EfRepositoryBase<Transmission, BaseDbContext>, ITransmissionRepository
{
    public TransmissionRepository(BaseDbContext context) : base(context)
    {
    }
}
