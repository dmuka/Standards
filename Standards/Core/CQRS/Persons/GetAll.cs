using MediatR;
using Microsoft.EntityFrameworkCore;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Persons;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.Core.CQRS.Persons;

public class GetAll
{
    public class Query : IRequest<IList<PersonDto>>
    {
    }

    public class QueryHandler(
        IRepository repository, 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<PersonDto>>
    {
        public async Task<IList<PersonDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
            var persons = await cache.GetOrCreateAsync<Person>(
                Cache.Persons,
                async (token) =>
                {
                    var result = await repository.GetListAsync<Person>(
                        query => query
                            .Include(p => p.Category)
                            .Include(p => p.Position)
                            .Include(p => p.Sector)
                            .Include(p => p.User),
                        token);

                    return result;
                },
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