using MediatR;
using Microsoft.EntityFrameworkCore;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.Core.CQRS.Quantities;

public class GetAll
{
    public class Query : IRequest<IList<QuantityDto>>
    {
    }

    public class QueryHandler(
        IRepository repository, 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<QuantityDto>>
    {
        public async Task<IList<QuantityDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
            var quantities = await cache.GetOrCreateAsync<Quantity>(
                Cache.Quantities,
                async (token) =>
                {
                    var result = await repository.GetListAsync<Quantity>(
                        query => query
                            .Include(q => q.Units),
                        token);

                    return result;
                },
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (quantities is null) return [];
                
            var dtos = quantities
                .Select(q => new QuantityDto
                {
                    Id = q.Id,
                    Name = q.Name,
                    UnitIds = q.Units.Select(u => u.Id).ToList()
                }).ToList();

            return dtos;
        }
    }
}