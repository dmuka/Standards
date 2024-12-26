using Domain.Models.Persons;
using Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.Sector).WithMany(s => s.Persons).OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.Category).WithMany();

        builder.HasOne(p => p.Position).WithMany();

        builder.HasOne(p => p.User).WithOne().HasForeignKey<Person>(p => p.UserId).HasPrincipalKey<User>(u => u.Id);
    }
}