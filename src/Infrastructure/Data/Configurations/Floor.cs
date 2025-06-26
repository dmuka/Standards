using Domain.Aggregates.Floors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class FloorConfiguration : IEntityTypeConfiguration<Floor>
{
    public void Configure(EntityTypeBuilder<Floor> builder)
    {
        builder.HasKey(floor => floor.Id);       
        
        builder.Property(floor => floor.Id)
            .HasConversion(
                typedId => typedId.Value,
                guid => new FloorId(guid))
            .ValueGeneratedNever();
    }
}