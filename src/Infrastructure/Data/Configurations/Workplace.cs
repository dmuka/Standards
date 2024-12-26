using Domain.Models.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class WorkplaceConfiguration : IEntityTypeConfiguration<Workplace>
{
    public void Configure(EntityTypeBuilder<Workplace> builder)
    {
        builder.HasKey(wp => wp.Id);

        builder.HasOne(wp => wp.Room).WithMany(room => room.WorkPlaces).OnDelete(DeleteBehavior.NoAction);
    }
}