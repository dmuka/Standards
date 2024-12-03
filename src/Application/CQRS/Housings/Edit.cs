using Application.Abstractions.Cache;
using Application.CQRS.Common.Attributes;
using Domain.Constants;
using Domain.Models.DTOs;
using Domain.Models.Housings;
using FluentValidation;
using MediatR;
using Standards.Core.Constants;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Validators;

namespace Application.CQRS.Housings;

[TransactionScope]
public class Edit
{
    public class Query(HousingDto housingDto) : IRequest<int>
    {
        public HousingDto HousingDto { get; } = housingDto;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            // var departments = repository.GetQueryable<Department>()
            //     .Where(department => department.Housings
            //         .SelectMany(h => h.Departments.Select(d => d.Id))
            //         .Intersect(request.HousingDto.DepartmentIds).Any());

            var rooms = repository.GetQueryable<Room>()
                .Where(room => room.Housing.Id == request.HousingDto.Id);
                
            var housing = new Housing
            {
                Id = request.HousingDto.Id,
                Name = request.HousingDto.Name,
                ShortName = request.HousingDto.ShortName,
                FloorsCount = request.HousingDto.FloorsCount,
                Address = request.HousingDto.Address,
                Rooms = rooms.ToList(),
                Comments = request.HousingDto.Comments
            };
                
            repository.Update(housing);

            var result = await repository.SaveChangesAsync(cancellationToken);
                
            cacheService.Remove(Cache.Housings);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.HousingDto)
                .NotEmpty()
                .ChildRules(housing =>
                {
                    housing.RuleFor(housingDto => housingDto.Id)
                        .GreaterThan(default(int))
                        .SetValidator(new IdValidator<Housing>(repository));

                    housing.RuleFor(housingDto => housingDto.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);

                    housing.RuleFor(housingDto => housingDto.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.ShortName);

                    housing.RuleFor(housingDto => housingDto.FloorsCount)
                        .GreaterThan(default(int));

                    housing.RuleFor(housingDto => housingDto.Address)
                        .NotEmpty();

                    // housing.RuleFor(housingDto => housingDto.DepartmentIds)
                    //     .NotEmpty()
                    //     .ForEach(id => 
                    //         id.SetValidator(new IdValidator<Department>(repository)));

                    housing.RuleFor(housingDto => housingDto.RoomIds)
                        .NotEmpty()
                        .ForEach(id => 
                            id.SetValidator(new IdValidator<Room>(repository)));
                });
        }
    }
}