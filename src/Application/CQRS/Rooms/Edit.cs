using Application.Abstractions.Cache;
using Application.CQRS.Common.Attributes;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.DTOs;
using Domain.Models.Housings;
using Domain.Models.Persons;
using FluentValidation;
using MediatR;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Validators;

namespace Application.CQRS.Rooms
{
    [TransactionScope]
    public class Edit
    {
        public class Query(RoomDto dto) : IRequest<int>
        {
            public RoomDto RoomDto { get; } = dto;
        }

        public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
        {
            public async Task<int> Handle(Query request, CancellationToken cancellationToken)
            {
                var housing = await repository.GetByIdAsync<Housing>(request.RoomDto.HousingId, cancellationToken);
                
                var sector = await repository.GetByIdAsync<Sector>(request.RoomDto.SectorId, cancellationToken);

                var persons = repository.GetQueryable<Person>()
                    .Where(person => request.RoomDto.PersonIds.Contains(person.Id));

                var workplaces = repository.GetQueryable<Workplace>()
                    .Where(workplace => request.RoomDto.WorkplaceIds.Contains(workplace.Room.Id));
                
                var room = new Room
                {
                    Id = request.RoomDto.Id,
                    Name = request.RoomDto.Name,
                    ShortName = request.RoomDto.ShortName,
                    Housing = housing,
                    Sector = sector,
                    Comments = request.RoomDto.Comments,
                    Floor = request.RoomDto.Floor,
                    Height = request.RoomDto.Height,
                    Length = request.RoomDto.Length,
                    Width = request.RoomDto.Width,
                    Persons = persons.ToList(),
                    WorkPlaces = workplaces.ToList()
                };
                
                repository.Update(room);

                var result = await repository.SaveChangesAsync(cancellationToken);
                
                cacheService.Remove(Cache.Rooms);

                return result;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator(IRepository repository)
            {
                RuleLevelCascadeMode = CascadeMode.Stop;

                RuleFor(query => query.RoomDto)
                    .NotEmpty()
                    .ChildRules(room =>
                    {
                        room.RuleFor(dto => dto.Id)
                            .GreaterThan(0)
                            .SetValidator(new IdValidator<Room>(repository));;

                        room.RuleFor(dto => dto.Name)
                            .NotEmpty()
                            .MaximumLength(Lengths.EntityName);

                        room.RuleFor(dto => dto.ShortName)
                            .NotEmpty()
                            .MaximumLength(Lengths.ShortName);

                        room.RuleFor(dto => dto.Floor)
                            .GreaterThan(0);

                        room.RuleFor(dto => dto.Height)
                            .GreaterThan(default(double));

                        room.RuleFor(dto => dto.Length)
                            .GreaterThan(default(double));

                        room.RuleFor(dto => dto.Width)
                            .GreaterThan(default(double));
                        
                        room.RuleFor(dto => dto.HousingId)
                            .GreaterThan(0)
                            .SetValidator(new IdValidator<Housing>(repository));
                        
                        room.RuleFor(dto => dto.SectorId)
                            .GreaterThan(0)
                            .SetValidator(new IdValidator<Sector>(repository));
                        
                        room.RuleFor(dto => dto.PersonIds)
                            .NotEmpty()
                            .ForEach(id => 
                                id.SetValidator(new IdValidator<Person>(repository)));
                        
                        room.RuleFor(dto => dto.WorkplaceIds)
                            .NotEmpty()
                            .ForEach(id => 
                                id.SetValidator(new IdValidator<Workplace>(repository)));
                    });
            }
        }
    }
}
