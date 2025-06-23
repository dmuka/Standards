using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models.Persons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Persons;

public class GetAll
{
    public class Query : IRequest<IList<PersonDto>>
    {
    }

    public class QueryHandler( 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<PersonDto>>
    {
        public async Task<IList<PersonDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
            var persons = await cache.GetOrCreateAsync<Person>(
                Cache.Persons,
                [
                    p => p.Category,
                    p => p.Position,
                    p => p.Sector,
                    p => p.User
                ],
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (persons is null) return [];
                
            var dtos = persons
                .Select(p => new PersonDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    MiddleName = p.MiddleName,
                    LastName = p.LastName,
                    CategoryId = p.Category.Id,
                    PositionId = p.Position.Id,
                    BirthdayDate = p.BirthdayDate,
                    SectorId = p.Sector.Id,
                    Role = p.Role,
                    UserId = p.User.Id,
                    Comments = p.Comments
                }).ToList();

            return dtos;
        }
    }
}