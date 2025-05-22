using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Domain.Constants;
using Domain.Models.DTOs;
using Domain.Models.Standards;
using MediatR;

namespace Application.UseCases.Standards;

public class GetAll
{
    public class Query : IRequest<IList<StandardDto>>
    {
    }

    public class QueryHandler( 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<StandardDto>>
    {
        public async Task<IList<StandardDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
            var standards = await cache.GetOrCreateAsync<Standard>(
                Cache.Standards,
                [
                    s => s.Services,
                    s => s.Characteristics,
                    s => s.Workplaces,
                    s => s.Responsible
                ],
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (standards is null) return [];
                
            var dtos = standards.Select(Standard.ToDto).ToList();

            return dtos;
        }
    }
}