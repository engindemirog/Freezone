using Application.Services.Repositories;
using Domain.Entities;
using Freezone.Core.Persistence.Repositories;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories;

public class BrandRepository : EfRepositoryBase<Brand, BaseDbContext>, IBrandRepository
{
    public BrandRepository(BaseDbContext context) : base(context)
    {
    }
}
