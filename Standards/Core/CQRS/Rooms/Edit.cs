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

namespace Standards.Core.CQRS.Rooms
{
    [TransactionScope]
    public class Edit
    {
        public class Query(RoomDto roomDto) : IRequest<int>
        {
            public RoomDto RoomDto { get; } = roomDto;
        }

        public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
        {
            public async Task<int> Handle(Query request, CancellationToken cancellationToken)
            {
                var housing = await repository.GetByIdAsync<Housing>(request.RoomDto.HousingId, cancellationToken);

                var persons = repository.GetQueryable<Person>()
                    .Where(person => request.RoomDto.PersonIds.Contains(person.Id));

                var workplaces = repository.GetQueryable<WorkPlace>()
                    .Where(workplace => request.RoomDto.WorkplaceIds.Contains(workplace.Room.Id));
                
                var room = new Room
                {
                    Id = request.RoomDto.Id,
                    Name = request.RoomDto.Name,
                    ShortName = request.RoomDto.ShortName,
                    Housing = housing,
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
                        room.RuleFor(roomDto => roomDto.Id)
                            .GreaterThan(default(int))
                            .SetValidator(new IdValidator<Room>(repository));;

                        room.RuleFor(roomDto => roomDto.Name)
                            .NotEmpty();

                        room.RuleFor(roomDto => roomDto.ShortName)
                            .NotEmpty();

                        room.RuleFor(roomDto => roomDto.Floor)
                            .GreaterThan(default(int));

                        room.RuleFor(roomDto => roomDto.Height)
                            .GreaterThan(default(double));

                        room.RuleFor(roomDto => roomDto.Length)
                            .GreaterThan(default(double));

                        room.RuleFor(roomDto => roomDto.Width)
                            .GreaterThan(default(double));
                        
                        room.RuleFor(roomDto => roomDto.HousingId)
                            .GreaterThan(default(int))
                            .SetValidator(new IdValidator<Housing>(repository));
                        
                        room.RuleFor(roomDto => roomDto.SectorId)
                            .GreaterThan(default(int))
                            .SetValidator(new IdValidator<Sector>(repository));
                        
                        room.RuleFor(roomDto => roomDto.PersonIds)
                            .NotEmpty()
                            .ForEach(id => 
                                id.SetValidator(new IdValidator<Person>(repository)));
                        
                        room.RuleFor(roomDto => roomDto.WorkplaceIds)
                            .NotEmpty()
                            .ForEach(id => 
                                id.SetValidator(new IdValidator<WorkPlace>(repository)));
                    });
            }
        }
    }
}
