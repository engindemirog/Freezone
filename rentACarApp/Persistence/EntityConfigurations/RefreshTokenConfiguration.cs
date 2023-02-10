using Freezone.Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class RefreshTokenConfiguration:IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.Property(p => p.Id).HasColumnName("Id");
        builder.Property(p => p.Token).HasColumnName("Token");
        builder.Property(p => p.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(p => p.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(p => p.ExpiresDate).HasColumnName("ExpiresDate");
        builder.Property(p => p.RevokedDate).HasColumnName("RevokedDate");
        builder.Property(p => p.ReplacedByToken).HasColumnName("ReplacedByToken");
        builder.Property(p => p.RevokedReason).HasColumnName("RevokedReason");
        builder.Property(p => p.CreatedByIp).HasColumnName("CreatedByIp");
        builder.Property(p => p.RevokedByIp).HasColumnName("RevokedByIp");
        builder.Property(p => p.UserId).HasColumnName("UserId");

        builder.HasOne(p => p.User);
    }
}