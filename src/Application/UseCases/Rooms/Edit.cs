using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.Abstractions.Data.Validators;
using Application.Exceptions;
using Application.UseCases.Common.Attributes;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.Housings;
using Domain.Models.Persons;
using FluentValidation;
using Infrastructure.Exceptions.Enum;
using MediatR;

namespace Application.UseCases.Rooms
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
                if (cancellationToken.IsCancellationRequested) return 0;
                
                var housing = await repository.GetByIdAsync<Housing>(request.RoomDto.HousingId, cancellationToken);
                if (housing is null) throw new StandardsException(StatusCodeByError.InternalServerError, "Every room must have relation to the housing", "Some error");
                
                var sector = request.RoomDto.SectorId is not null 
                    ? await repository.GetByIdAsync<Sector>(request.RoomDto.SectorId, cancellationToken)
                    : null;

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
                            .GreaterThan(0);;

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
                            .GreaterThan(0);
                        
                        room.RuleFor(dto => dto.SectorId)
                            .GreaterThan(0);
                        
                        room.RuleFor(dto => dto.PersonIds)
                            .NotEmpty()
                            .ForEach(id => id.GreaterThan(0));
                        
                        room.RuleFor(dto => dto.WorkplaceIds)
                            .NotEmpty()
                            .ForEach(id => id.GreaterThan(0));
                    });
            }
        }
    }
}
