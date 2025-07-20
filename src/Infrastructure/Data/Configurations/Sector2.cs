
using Domain.Aggregates.Sectors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class Sector2Configuration : IEntityTypeConfiguration<Sector>
{
    public void Configure(EntityTypeBuilder<Sector> builder)
    {
        builder.HasKey(p => p.Id);       
        
        builder.Property(sector => sector.Id)
            .HasConversion(
                typedId => typedId.Value,
                guid => new SectorId(guid))
            .ValueGeneratedNever();
    }
}