using Domain.Entities;
using Freezone.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IFuelRepository : IAsyncRepository<Fuel>, IRepository<Fuel>
{
}
