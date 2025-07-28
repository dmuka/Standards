using Core;
using Domain.Aggregates.Categories;

namespace Application.UseCases.DTOs;

public class CategoryDto2 : Entity
{
    public new required CategoryId Id { get; set; }
    public required string CategoryName { get; set; }
    public required string CategoryShortName { get; set; }
}