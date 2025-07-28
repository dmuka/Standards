using Domain.Aggregates.Categories;
using Domain.Aggregates.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(category => category.Id);       
        
        builder.Property(category => category.Id)
            .HasConversion(
                typedId => typedId.Value,
                guid => new CategoryId(guid))
            .ValueGeneratedNever();
        
        builder.Property(category => category.Name)
            .HasConversion(
                categoryName => categoryName.Value,
                value => Name.Create(value).Value);
        
        builder.Property(category => category.ShortName)
            .HasConversion(
                categoryShortName => categoryShortName.Value,
                value => ShortName.Create(value).Value);
    }
}