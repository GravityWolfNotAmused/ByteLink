using ByteLink.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteLink.Infrastructure.Persistence.Configurations;

public class UrlConfiguration : IEntityTypeConfiguration<Url>
{
    public void Configure(EntityTypeBuilder<Url> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasColumnOrder(1)
               .ValueGeneratedOnAdd()
               .UseMySqlIdentityColumn();

        builder.Property(u => u.TotalVisits)
                .IsRequired()
                .HasDefaultValue(0);

        builder.Property(url => url.UserId)
            .IsRequired();

        builder.Property(u => u.SourceUrl)
               .IsRequired()
               .HasMaxLength(2048);

        builder.Property(u => u.ShortCode)
               .IsRequired()
               .HasMaxLength(10)
               .UseCollation("utf8mb4_bin");

        builder.HasIndex(u => new { u.ShortCode, u.UserId})
               .IsUnique();

        builder.Property(u => u.CreatedAt)
               .HasColumnOrder(2);
    }
}
