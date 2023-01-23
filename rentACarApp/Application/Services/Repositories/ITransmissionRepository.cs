using Domain.Entities;
using Freezone.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface ITransmissionRepository : IAsyncRepository<Transmission>, IRepository<Transmission>
{
}
