using Core;
using Domain.Aggregates.Categories;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Positions;
using Domain.Aggregates.Sectors;
using Domain.Aggregates.Users;

namespace Application.UseCases.DTOs;

public class PersonDto2 : Entity
{
    public new required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? PositionId { get; set; }
    public required DateOnly? BirthdayDate { get; set; }
    public Guid? SectorId { get; set; }
    public required Guid UserId { get; set; }
}