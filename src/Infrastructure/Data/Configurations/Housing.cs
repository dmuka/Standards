using Domain.Aggregates.Common.ValueObjects;
using Domain.Aggregates.Housings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class HousingConfiguration : IEntityTypeConfiguration<Housing>
{
    public void Configure(EntityTypeBuilder<Housing> builder)
    {
        builder.HasKey(housing => housing.Id);       
        
        builder.Property(housing => housing.Id)
            .HasConversion(
                typedId => typedId.Value,
                guid => new HousingId(guid))
            .ValueGeneratedNever();
        
        builder.Property(housing => housing.Address)
            .HasConversion(
                address => address.Value,
                value => Address.Create(value).Value);
        
        builder.Property(housing => housing.HousingName)
            .HasConversion(
                housingName => housingName.Value,
                value => HousingName.Create(value).Value);
        
        builder.Property(housing => housing.HousingShortName)
            .HasConversion(
                housingShortName => housingShortName.Value,
                value => HousingShortName.Create(value).Value);
    }
}