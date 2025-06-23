using Domain.Models;
using Domain.Models.Departments;
using Domain.Models.Persons;
using Domain.Models.Services;
using Domain.Models.Standards;

namespace Application.UseCases.DTOs;

public class StandardDto : Entity
{
    public required int ResponsibleId { get; set; }
    public required string? ImagePath { get; set; }
    public int VerificationInterval { get; set; }
    public int? CalibrationInterval { get; set; }
    public IList<int> ServiceIds { get; set; } = [];
    public required IList<int> WorkplaceIds { get; set; } = [];
    public required IList<int> CharacteristicIds { get; set; } = [];

    public static StandardDto ToDto(Standard standard)
    {
        return new StandardDto
        {
            Id = standard.Id,
            Name = standard.Name,
            ShortName = standard.ShortName,
            Comments = standard.Comments,
            ServiceIds = standard.Services.Select(service => service.Id).ToList(),
            CharacteristicIds = standard.Characteristics.Select(characteristic => characteristic.Id).ToList(),
            WorkplaceIds = standard.Workplaces.Select(workplace => workplace.Id).ToList(),
            ResponsibleId = standard.Responsible.Id,
            VerificationInterval = standard.VerificationInterval,
            CalibrationInterval = standard.CalibrationInterval,
            ImagePath = standard.ImagePath
        };
    }

    public static Standard ToEntity(
        StandardDto standardDto,
        IList<Workplace> workplaces,
        IList<Characteristic> characteristics,
        IList<Service> services,
        Person responsible)
    {
        return new Standard
        {
            Name = standardDto.Name,
            ShortName = standardDto.ShortName,
            Comments = standardDto.Comments,
            Services = services,
            Characteristics = characteristics,
            Workplaces = workplaces,
            Responsible = responsible,
            VerificationInterval = standardDto.VerificationInterval,
            CalibrationInterval = standardDto.CalibrationInterval,
            ImagePath = standardDto.ImagePath
        };
    }
}