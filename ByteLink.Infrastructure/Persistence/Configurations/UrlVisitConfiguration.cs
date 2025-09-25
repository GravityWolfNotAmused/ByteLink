using ByteLink.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteLink.Infrastructure.Persistence.Configurations;

public class UrlVisitConfiguration : IEntityTypeConfiguration<UrlVisit>
{
    public void Configure(EntityTypeBuilder<UrlVisit> builder)
    {
        builder.HasKey(visit => visit.Id);

        builder.Property(visit => visit.Id)
            .HasColumnOrder(1)
            .ValueGeneratedOnAdd()
            .UseMySqlIdentityColumn();

        builder.Property(visit => visit.UrlId)
            .HasColumnOrder(3)
            .IsRequired();

        builder.HasOne(visit => visit.Url)
            .WithMany(url => url.Visits)
            .HasForeignKey(visit => visit.UrlId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(visit => visit.CreatedAt)
            .HasColumnOrder(2);
    }
}