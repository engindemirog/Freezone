using Domain.Entities;
using Freezone.Core.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Freezone.Core.Security.Entities;

namespace Persistence.Contexts;

public class BaseDbContext : DbContext
{
    protected IConfiguration Configuration { get; set; }
    public BaseDbContext(DbContextOptions dbContextOptions,
        IConfiguration configuration) : base(dbContextOptions)
    {
        Configuration = configuration;
    }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Fuel> Fuel { get; set; }
    public DbSet<Transmission> Transmissions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<OperationClaim> OperationClaims { get; set; }
    public DbSet<UserOperationClaim> UserOperationClaims { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<EntityEntry<Entity>> datas = ChangeTracker.Entries<Entity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var data in datas)
        {
            _ = data.State switch
            {
                EntityState.Added=> data.Entity.CreatedDate = DateTime.UtcNow,
                EntityState.Modified=>data.Entity.UpdatedDate= DateTime.UtcNow
            };
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}
