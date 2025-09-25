using ByteLink.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteLink.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
               .HasColumnOrder(1)
               .ValueGeneratedOnAdd()
               .UseMySqlIdentityColumn();

        builder.Property(user => user.UserId);

        builder.Property(user => user.Email)
            .IsRequired();

        builder.HasIndex(user => user.Email)
               .IsUnique();

        builder.Property(user => user.PasswordHash)
            .IsRequired();

        builder.Property(user => user.CreatedAt)
               .HasColumnOrder(2);

        builder.Property(user => user.DatabaseUser)
            .IsRequired();

        builder.HasIndex(user => user.DatabaseUser)
            .IsUnique();

        builder.Property(user => user.DatabasePWD)
            .IsRequired();

        builder.HasIndex(user => user.DatabasePWD)
            .IsUnique();

        builder.Property(user => user.DatabaseName)
            .IsRequired();

        builder.HasIndex(user => user.DatabaseName)
            .IsUnique();
    }
}
