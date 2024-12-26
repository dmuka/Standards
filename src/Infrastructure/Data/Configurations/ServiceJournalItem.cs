using Domain.Models.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class ServiceJournalItemConfiguration : IEntityTypeConfiguration<ServiceJournalItem>
{
    public void Configure(EntityTypeBuilder<ServiceJournalItem> builder)
    {
        builder.HasKey(sji => sji.Id);

        builder.ToTable("ServiceJournal").HasOne(sji => sji.Standard).WithMany().OnDelete(DeleteBehavior.NoAction);
    }
}