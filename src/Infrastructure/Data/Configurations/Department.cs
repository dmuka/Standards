using Domain.Aggregates.Common.ValueObjects;
using Domain.Aggregates.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(department => department.Id);       
        
        builder.Property(department => department.Id)
            .HasConversion(
                typedId => typedId.Value,
                guid => new DepartmentId(guid))
            .ValueGeneratedNever();
        
        builder.Property(department => department.Name)
            .HasConversion(
                departmentName => departmentName.Value,
                value => Name.Create(value).Value);
        
        builder.Property(department => department.ShortName)
            .HasConversion(
                departmentShortName => departmentShortName.Value,
                value => ShortName.Create(value).Value);
    }
}