using Domain.Aggregates.Categories;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Positions;
using Domain.Aggregates.Sectors;
using Domain.Aggregates.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class Person2Configuration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(person => person.Id)
            .HasConversion(
                typedId => typedId.Value,
                guid => new PersonId(guid))
            .ValueGeneratedNever();
        
        builder.Property(person => person.CategoryId)
            .HasConversion(
                typedId => typedId.Value,
                guid => new CategoryId(guid))
            .ValueGeneratedNever();
        
        builder.Property(person => person.PositionId)
            .HasConversion(
                typedId => typedId.Value,
                guid => new PositionId(guid))
            .ValueGeneratedNever();
        
        builder.Property(person => person.SectorId)
            .HasConversion(
                typedId => typedId.Value,
                guid => new SectorId(guid))
            .ValueGeneratedNever();
        
        builder.Property(person => person.UserId)
            .HasConversion(
                typedId => typedId.Value,
                guid => new UserId(guid))
            .ValueGeneratedNever();
        
        builder.Property(person => person.FirstName)
            .HasConversion(
                firstName => firstName.Value,
                value => Domain.Aggregates.Persons.FirstName.Create(value).Value);
        
        builder.Property(person => person.LastName)
            .HasConversion(
                lastName => lastName.Value,
                value => Domain.Aggregates.Persons.LastName.Create(value).Value);
        
        builder.Property(person => person.MiddleName)
            .HasConversion(
                middleName => middleName.Value,
                value => MiddleName.Create(value).Value);
        
        builder.Property(person => person.BirthdayDate)
            .HasConversion(
                birthDate => birthDate.Value,
                value => BirthdayDate.Create(value).Value);
    }
}