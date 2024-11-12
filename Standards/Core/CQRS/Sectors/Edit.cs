using FluentValidation;
using MediatR;
using Standards.Core.Constants;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.Core.Models.Persons;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;
using Standards.Infrastructure.Validators;

namespace Standards.Core.CQRS.Sectors;

[TransactionScope]
public class Edit
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
                .Where(room => room.Sector.Id == request.SectorDto.Id)
                .ToList();

            var workplaces = repository.GetQueryable<Workplace>()
                .Where(workplace => rooms
                    .Select(room => room.Id)
                    .Contains(workplace.Room.Id))
                .ToList();

            var persons = repository.GetQueryable<Person>()
                .Where(person => person.Sector.Id == request.SectorDto.Id)
                .ToList();

            var department = await repository.GetByIdAsync<Department>(request.SectorDto.DepartmentId, cancellationToken);
            
            var sector = new Sector
            {
                Name = request.SectorDto.Name,
                ShortName = request.SectorDto.ShortName,
                Department = department,
                Rooms = rooms,
                Workplaces = workplaces,
                Persons = persons,
                Comments = request.SectorDto.Comments
            };
                
            repository.Update(sector);

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
                .ChildRules(sector =>
                {
                    sector.RuleFor(dto => dto.Id)
                        .GreaterThan(default(int))
                        .SetValidator(new IdValidator<Sector>(repository));

                    sector.RuleFor(dto => dto.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);

                    sector.RuleFor(dto => dto.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.ShortName);

                    sector.RuleFor(dto => dto.PersonIds)
                        .NotEmpty()
                        .ForEach(id => 
                            id.SetValidator(new IdValidator<Person>(repository)));

                    sector.RuleFor(dto => dto.RoomIds)
                        .NotEmpty()
                        .ForEach(id => 
                            id.SetValidator(new IdValidator<Room>(repository)));

                    sector.RuleFor(dto => dto.WorkplaceIds)
                        .NotEmpty()
                        .ForEach(id => 
                            id.SetValidator(new IdValidator<Workplace>(repository)));

                    sector.RuleFor(dto => dto.DepartmentId)
                        .GreaterThan(default(int))
                        .SetValidator(new IdValidator<Department>(repository));
                });
        }
    }
}