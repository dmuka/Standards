using Domain.Models.Housings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(r => r.Housing).WithMany(h => h.Rooms).OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(r => r.Sector).WithMany(s => s.Rooms).OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(r => r.Persons).WithOne().OnDelete(DeleteBehavior.NoAction);
    }
}