
using Domain.Aggregates.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class Room2Configuration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(room => room.Id);       
        
        builder.Property(room => room.Id)
            .HasConversion(
                typedId => typedId.Value,
                guid => new RoomId(guid))
            .ValueGeneratedNever();
        
        builder.Property(room => room.Length)
            .HasConversion(
                roomLength => roomLength.Value,
                value => Length.Create(value).Value);
        
        builder.Property(room => room.Height)
            .HasConversion(
                roomHeight => roomHeight.Value,
                value => Height.Create(value).Value);
        
        builder.Property(room => room.Width)
            .HasConversion(
                roomWidth => roomWidth.Value,
                value => Width.Create(value).Value);
    }
}