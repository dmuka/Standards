using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Repositories.Interfaces;

namespace Application.CQRS.Workplaces;

public class GetAll
{
    public class Query : IRequest<IList<WorkplaceDto>>
    {
    }

    public class QueryHandler(
        IRepository repository, 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<WorkplaceDto>>
    {
        public async Task<IList<WorkplaceDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
            var workplaces = await cache.GetOrCreateAsync<Workplace>(
                Cache.Workplaces,
                async (token) =>
                {
                    var result = await repository.GetListAsync<Workplace>(
                        query => query
                            .Include(w => w.Room)
                            .Include(s => s.Responsible),
                        token);

                    return result;
                },
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (workplaces is null) return [];
                
            var dtos = workplaces
                .Select(s => new WorkplaceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    ShortName = s.ShortName,
                    Comments = s.Comments,
                    RoomId = s.Room.Id,
                    ResponsibleId = s.Responsible.Id
                }).ToList();

            return dtos;
        }
    }
}