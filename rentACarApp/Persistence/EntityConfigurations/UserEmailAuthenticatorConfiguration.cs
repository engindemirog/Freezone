using Freezone.Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class UserEmailAuthenticatorConfiguration:IEntityTypeConfiguration<UserEmailAuthenticator>
{
    public void Configure(EntityTypeBuilder<UserEmailAuthenticator> builder)
    {
        builder.ToTable("UserEmailAuthenticators");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("Id");
        builder.Property(x => x.UserId).HasColumnName("UserId");
        builder.Property(x => x.Key).HasColumnName("Key");
        builder.Property(x => x.IsVerified).HasColumnName("IsVerified");

        builder.HasOne(x => x.User);
    }
}