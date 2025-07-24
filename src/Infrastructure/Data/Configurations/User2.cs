using Domain.Aggregates.Categories;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Positions;
using Domain.Aggregates.Sectors;
using Domain.Aggregates.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FirstName = Domain.Aggregates.Users.FirstName;
using LastName = Domain.Aggregates.Users.LastName;

namespace Infrastructure.Data.Configurations;

internal sealed class User2Configuration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(user => user.Id)
            .HasConversion(
                typedId => typedId.Value,
                guid => new UserId(guid))
            .ValueGeneratedNever();
        
        builder.Property(user => user.PersonId)
            .HasConversion(
                typedId => typedId.Value,
                guid => new PersonId(guid))
            .ValueGeneratedNever();
        
        builder.Property(user => user.FirstName)
            .HasConversion(
                firstName => firstName.Value,
                value => FirstName.Create(value).Value);
        
        builder.Property(user => user.LastName)
            .HasConversion(
                lastName => lastName.Value,
                value => LastName.Create(value).Value);
        
        // builder.Property(user => user.MiddleName)
        //     .HasConversion(
        //         middleName => middleName.Value,
        //         value => MiddleName.Create(value).Value);
        
        builder.Property(user => user.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value).Value);
    }
}