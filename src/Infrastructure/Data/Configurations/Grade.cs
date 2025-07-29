using Domain.Aggregates.Common.ValueObjects;
using Domain.Aggregates.Grades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.HasKey(category => category.Id);       
        
        builder.Property(category => category.Id)
            .HasConversion(
                typedId => typedId.Value,
                guid => new GradeId(guid))
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