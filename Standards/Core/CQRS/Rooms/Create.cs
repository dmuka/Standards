using FluentValidation;
using MediatR;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.Core.Models.Persons;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.Core.CQRS.Rooms;

[TransactionScope]
public class Create
{
    public class Query(RoomDto room) : IRequest<int>
    {
        public RoomDto Room { get; set; } = room;
    }

    public class QueryHandler(IRepository repository) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var persons = repository.GetQueryable<Person>()
                .Where(person => person.Sector.Id == request.Room.SectorId)
                .ToList();

            var workplaces = repository.GetQueryable<WorkPlace>()
                .Where(workplace => workplace.RoomId == request.Room.Id)
                .ToList();

            var room = GetRoom(request.Room, persons, workplaces);
            
            await repository.AddAsync(room, cancellationToken);

            var result = await repository.SaveChangesAsync(cancellationToken);

            return result;
        }

        private Room GetRoom(
            RoomDto roomDto,
            IList<Person> persons,
            IList<WorkPlace> workplaces)
        {
            var room = new Room
            {
                Name = roomDto.Name,
                SectorId = roomDto.SectorId,
                HousingId = roomDto.HousingId,
                Persons = persons,
                WorkPlaces = workplaces,
                Floor = roomDto.Floor,
                Height = roomDto.Height,
                Width = roomDto.Width,
                Length = roomDto.Length
            };

            if (roomDto.Comments is not null) room.Comments = roomDto.Comments;

            return room;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.Room)
                .NotEmpty()
                .ChildRules(filter =>
                {
                    filter.RuleFor(room => room.Name)
                        .NotEmpty();

                    filter.RuleFor(room => room.HousingId)
                        .GreaterThan(default(int));

                    filter.RuleFor(room => room.Width)
                        .GreaterThan(default(int));

                    filter.RuleFor(room => room.Length)
                        .GreaterThan(default(int));

                    filter.RuleFor(room => room.Height)
                        .GreaterThan(default(int));

                    filter.RuleFor(room => room.Floor)
                        .GreaterThan(default(int));

                    filter.RuleFor(room => room.SectorId)
                        .GreaterThan(default(int));
                });
        }
    }
}