using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models.Departments;
using MediatR;

namespace Application.UseCases.Workplaces;

public class GetAll
{
    public class Query : IRequest<IList<WorkplaceDto>>
    {
    }

    public class QueryHandler( 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<WorkplaceDto>>
    {
        public async Task<IList<WorkplaceDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
            var workplaces = await cache.GetOrCreateAsync<Workplace>(
                Cache.Workplaces,
                [
                    w => w.Room,
                    w => w.Responsible
                ],
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