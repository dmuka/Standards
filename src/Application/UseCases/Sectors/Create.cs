using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.Abstractions.Data.Validators;
using Application.UseCases.Common.Attributes;
using Application.UseCases.DTOs;
using Domain.Aggregates.Persons;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.Housings;
using Domain.Models.Persons;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Sectors;

[TransactionScope]
public class Create
{
    public class Query(SectorDto sectorDto) : IRequest<int>
    {
        public SectorDto SectorDto { get; } = sectorDto;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var rooms = repository.GetQueryable<Room>()
                .Where(room => room.Sector!.Id == request.SectorDto.Id)
                .ToList();

            var workplaces = repository.GetQueryable<Workplace>()
                .Where(workplace => rooms
                    .Select(room => room.Id)
                    .Contains(workplace.Room.Id))
                .ToList();

            var persons = repository.GetQueryable<Person>()
                .Where(person => person.Sector.Id == request.SectorDto.Id)
                .ToList();

            var department = request.SectorDto.DepartmentId is not null 
                ? await repository.GetByIdAsync<Department>(request.SectorDto.DepartmentId, cancellationToken) 
                : null;
            
            var sector = new Sector
            {
                Name = request.SectorDto.Name,
                ShortName = request.SectorDto.ShortName,
                Department = department,
                Rooms = rooms,
                Workplaces = workplaces,
                Persons = persons
            };

            if (request.SectorDto.Comments is not null) sector.Comments = request.SectorDto.Comments;
            
            await repository.AddAsync(sector, cancellationToken);
            
            var result = await repository.SaveChangesAsync(cancellationToken);
            
            cacheService.Remove(Cache.Sectors);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.SectorDto)
                .NotEmpty()
                .ChildRules(sectorDto =>
                {
                    sectorDto.RuleFor(sector => sector.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);

                    sectorDto.RuleFor(sector => sector.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.ShortName);
                    
                    sectorDto.RuleFor(sector => sector.DepartmentId)
                        .GreaterThan(0)
                        .WithMessage("Id value must be valid.")
                        .When(s => s.DepartmentId != null);

                    sectorDto.RuleForEach(sector => sector.PersonIds)
                        .GreaterThan(0)
                        .WithMessage("Id value must be valid.");

                    sectorDto.RuleForEach(sector => sector.RoomIds)
                        .GreaterThan(0)
                        .WithMessage("Id value must be valid.");

                    sectorDto.RuleForEach(sector => sector.WorkplaceIds)
                        .GreaterThan(0)
                        .WithMessage("Id value must be valid.");
                });
        }
    }

}