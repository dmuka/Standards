using Core;
using Domain.Aggregates.Grades;

namespace Application.UseCases.DTOs;

public class GradeDto2 : Entity
{
    public new required GradeId Id { get; set; }
    public required string GradeName { get; set; }
    public required string GradeShortName { get; set; }
}