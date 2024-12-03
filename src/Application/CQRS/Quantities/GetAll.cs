using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Domain.Constants;
using Domain.Models;
using Domain.Models.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Repositories.Interfaces;

namespace Application.CQRS.Quantities;

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
                    ShortName = q.ShortName,
                    UnitIds = q.Units.Select(u => u.Id).ToList()
                }).ToList();

            return dtos;
        }
    }
}