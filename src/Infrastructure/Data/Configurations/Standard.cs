using Domain.Models.Standards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class StandardConfiguration : IEntityTypeConfiguration<Standard>
{
    public void Configure(EntityTypeBuilder<Standard> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder.HasOne(s => s.Responsible).WithMany().OnDelete(DeleteBehavior.NoAction);
    }
}