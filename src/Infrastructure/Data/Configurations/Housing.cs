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
                guid => new HousingId(guid));
        
        builder.OwnsOne(housing => housing.Address, a =>
        {
            a.Property(p => p.Value).HasColumnName("Address");
        });        
        
        builder.OwnsOne(housing => housing.HousingName, a =>
        {
            a.Property(p => p.Value).HasColumnName("HousingName");
        });     
        
        builder.OwnsOne(housing => housing.HousingShortName, a =>
        {
            a.Property(p => p.Value).HasColumnName("HousingShortName");
        });
    }
}