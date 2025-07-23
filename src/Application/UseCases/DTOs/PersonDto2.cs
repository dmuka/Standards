using Core;
using Domain.Aggregates.Categories;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Positions;
using Domain.Aggregates.Sectors;
using Domain.Aggregates.Users;

namespace Application.UseCases.DTOs;

public class PersonDto2 : Entity
{
    public new required PersonId Id { get; set; }
    public required FirstName FirstName { get; set; }
    public required MiddleName MiddleName { get; set; }
    public required LastName LastName { get; set; }
    public required CategoryId CategoryId { get; set; }
    public required PositionId PositionId { get; set; }
    public required BirthdayDate BirthdayDate { get; set; }
    public required SectorId SectorId { get; set; }
    public required UserId UserId { get; set; }
}