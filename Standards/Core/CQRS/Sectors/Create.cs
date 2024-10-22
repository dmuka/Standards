using FluentValidation;
using MediatR;
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
                .Where(room => room.Sector.Id == request.SectorDto.Id)
                .ToList();

            var workplaces = repository.GetQueryable<WorkPlace>()
                .Where(workplace => rooms
                    .Select(room => room.Id)
                    .Contains(workplace.Room.Id))
                .ToList();

            var persons = repository.GetQueryable<Person>()
                .Where(person => person.Sector.Id == request.SectorDto.Id)
                .ToList();

            var department = repository.GetQueryable<Department>()
                .First(department => department.Id == request.SectorDto.DepartmentId);
            
            var sector = new Sector
            {
                Name = request.SectorDto.Name,
                ShortName = request.SectorDto.ShortName,
                Department = department,
                Rooms = rooms,
                WorkPlaces = workplaces,
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
                .ChildRules(filter =>
                {
                    filter.RuleFor(sector => sector.Name)
                        .NotEmpty();

                    filter.RuleFor(sector => sector.ShortName)
                        .NotEmpty();
                    
                    filter.RuleFor(sector => sector.DepartmentId)
                        .GreaterThan(default(int))
                        .SetValidator(new IdValidator<Department>(repository));

                    filter.RuleFor(sector => sector.PersonIds)
                        .NotEmpty();

                    filter.RuleFor(sector => sector.RoomIds)
                        .NotEmpty();

                    filter.RuleFor(sector => sector.WorkPlaceIds)
                        .NotEmpty();
                });
        }
    }

}