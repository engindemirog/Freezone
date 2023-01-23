using Application.Services.Repositories;
using Domain.Entities;
using Freezone.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class FuelRepository : EfRepositoryBase<Fuel, BaseDbContext>, IFuelRepository
{
    public FuelRepository(BaseDbContext context) : base(context)
    {
    }
}
