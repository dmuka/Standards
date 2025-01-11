using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.DTOs;
using Domain.Models.Interfaces;
using Domain.Models.Persons;
using Domain.Models.Services;

namespace Domain.Models.Standards;

public class Standard : Entity, ICacheable
{
    public required Person? Responsible { get; set; }
    public required string? ImagePath { get; set; }
    public int VerificationInterval { get; set; }
    public int? CalibrationInterval { get; set; }
    public IList<Service> Services { get; set; } = [];
    public required IList<Workplace> Workplaces { get; set; } = [];
    public required IList<Characteristic> Characteristics { get; set; } = [];

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
        Person? responsible)
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
        
    public static string GetCacheKey()
    {
        return Cache.Standards;
    }
}