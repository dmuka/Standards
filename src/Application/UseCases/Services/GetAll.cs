using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Domain.Constants;
using Domain.Models.DTOs;
using Domain.Models.Services;
using MediatR;

namespace Application.UseCases.Services;

public class GetAll
{
    public class Query : IRequest<IList<ServiceDto>>
    {
    }

    public class QueryHandler( 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<ServiceDto>>
    {
        public async Task<IList<ServiceDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
            var services = await cache.GetOrCreateAsync<Service>(
                Cache.Services,
                [
                    s => s.Materials,
                    s => s.MaterialsQuantities
                ],
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (services is null) return [];
                
            var dtos = services
                .Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    ShortName = s.ShortName,
                    Comments = s.Comments,
                    ServiceTypeId = s.ServiceType.Id,
                    MaterialIds = s.Materials.Select(m => m.Id).ToList(),
                    MaterialsQuantityIds = s.MaterialsQuantities.Select(mq => mq.Id).ToList()
                }).ToList();

            return dtos;
        }
    }
}