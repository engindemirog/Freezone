using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class ModelConfiguration : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder)
    {
        builder.ToTable("Models");

        builder.Property(p => p.Id).HasColumnName("Id");
        builder.Property(p => p.Name).HasColumnName("Name");
        builder.Property(p => p.BrandId).HasColumnName("BrandId");
        builder.Property(p => p.FuelId).HasColumnName("FuelId");
        builder.Property(p => p.TransmissionId).HasColumnName("TransmissionId");
        builder.Property(p => p.DailyPrice).HasColumnName("DailyPrice");
        builder.Property(p => p.ImageUrl).HasColumnName("ImageUrl");


        builder.HasIndex(p => p.Name, "UK_Models_Name").IsUnique();

        builder.HasOne(p => p.Brand);
        builder.HasMany(p=>p.Cars);

        Model[] modelsSeed = { new(1, 1,1,1,"Series 1", 2000, "series1.jpg")
                             , new(2, 1,1,2,"Series 2", 3000, "series2.jpg")
                             , new(3, 2,2,2,"508", 2500, "508.jpg")
                             };

        builder.HasData(modelsSeed);

    }
}

public class FuelConfiguration : IEntityTypeConfiguration<Fuel>
{
    public void Configure(EntityTypeBuilder<Fuel> builder)
    {
        builder.ToTable("Fuels").HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnName("Id");
        builder.Property(f => f.Name).HasColumnName("Name");
        builder.HasIndex(f => f.Name, "UK_Fuels_Name").IsUnique();
        builder.HasMany(f => f.Models);

        Fuel[] fuelSeeds = { new(1, "Diesel"), new(2, "Electric") };
        builder.HasData(fuelSeeds);
    }
}

public class TransmissionConfiguration : IEntityTypeConfiguration<Transmission>
{
    public void Configure(EntityTypeBuilder<Transmission> builder)
    {
        builder.ToTable("Transmissions").HasKey(k => k.Id);
        builder.Property(p => p.Id).HasColumnName("Id");
        builder.Property(p => p.Name).HasColumnName("Name");
        builder.HasIndex(p => p.Name, "UK_Transmissions_Name").IsUnique();
        builder.HasMany(p => p.Models);

        Transmission[] transmissionsSeeds = { new(1, "Manuel"), new(2, "Automatic") };
        builder.HasData(transmissionsSeeds);
    }
}