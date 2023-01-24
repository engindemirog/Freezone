using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Persistence.EntityConfigurations;

public class CarConfigurations: IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Cars");

        builder.Property(p => p.Id).HasColumnName("Id");
        builder.Property(p => p.MinFindeksCreditRate).HasColumnName("MinFindeksCreditRate");
        builder.Property(p => p.ModelYear).HasColumnName("ModelYear");
        builder.Property(p => p.ModelId).HasColumnName("ModelId");
        builder.Property(p => p.Kilometer).HasColumnName("Kilometer");
        builder.Property(p => p.Plate).HasColumnName("Plate");
        builder.Property(p => p.CarState).HasColumnName("CarState").HasDefaultValue(CarState.Available);

        builder.HasOne(p => p.Model);

        //Car[] brandsSeed = { new(1, "BMW"), new(2, "Peugeout") };

        //builder.HasData(brandsSeed);

    }
}
