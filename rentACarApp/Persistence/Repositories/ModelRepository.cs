using Application.Services.Repositories;
using Domain.Entities;
using Freezone.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ModelRepository : EfRepositoryBase<Model, BaseDbContext>, IModelRepository
{
    public ModelRepository(BaseDbContext context) : base(context)
    {
    }
}
