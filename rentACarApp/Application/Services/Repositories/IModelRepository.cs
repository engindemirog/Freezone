using Domain.Entities;
using Freezone.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IModelRepository : IAsyncRepository<Model>, IRepository<Model>
{
}
