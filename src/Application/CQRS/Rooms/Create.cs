using Application.Abstractions.Cache;
using Application.CQRS.Common.Attributes;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.DTOs;
using Domain.Models.Housings;
using Domain.Models.Persons;
using FluentValidation;
using MediatR;
using Standards.Core.Constants;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Validators;

namespace Application.CQRS.Rooms;

[TransactionScope]
public class Create
{
    public class Query(RoomDto room) : IRequest<int>
    {
        public RoomDto Room { get; } = room;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var persons = repository.GetQueryable<Person>()
                .Where(person => person.Sector.Id == request.Room.SectorId)
                .ToList();

            var workplaces = repository.GetQueryable<Workplace>()
                .Where(workplace => workplace.Room.Id == request.Room.Id)
                .ToList();

            var sector = await repository.GetByIdAsync<Sector>(request.Room.SectorId, cancellationToken);
            
            var housing = await repository.GetByIdAsync<Housing>(request.Room.HousingId, cancellationToken);

            var room = new Room
            {
                Name = request.Room.Name,
                ShortName = request.Room.ShortName,
                Floor = request.Room.Floor,
                Height = request.Room.Height,
                Housing = housing,
                Sector = sector,
                Persons = persons,
                WorkPlaces = workplaces,
                Width = request.Room.Width,
                Length = request.Room.Length
            };

            if (request.Room.Comments is not null) room.Comments = request.Room.Comments;
            
            await repository.AddAsync(room, cancellationToken);

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

            RuleFor(query => query.Room)
                .NotEmpty()
                .ChildRules(filter =>
                {
                    filter.RuleFor(room => room.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);
                    
                    filter.RuleFor(room => room.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.ShortName);

                    filter.RuleFor(room => room.HousingId)
                        .GreaterThan(default(int))
                        .SetValidator(new IdValidator<Housing>(repository));

                    filter.RuleFor(room => room.Width)
                        .GreaterThan(default(int));

                    filter.RuleFor(room => room.Length)
                        .GreaterThan(default(int));

                    filter.RuleFor(room => room.Height)
                        .GreaterThan(default(int));

                    filter.RuleFor(room => room.Floor)
                        .GreaterThan(default(int));

                    filter.RuleFor(room => room.SectorId)
                        .GreaterThan(default(int))
                        .SetValidator(new IdValidator<Sector>(repository));
                });
        }
    }
}