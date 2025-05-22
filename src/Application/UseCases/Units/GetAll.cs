using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Domain.Constants;
using Domain.Models.DTOs;
using MediatR;
using Unit = Domain.Models.Unit;

namespace Application.UseCases.Units;

public class GetAll
{
    public class Query : IRequest<IList<UnitDto>>
    {
    }

    public class QueryHandler(
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<UnitDto>>
    {
        public async Task<IList<UnitDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
            var units = await cache.GetOrCreateAsync<Unit>(
                Cache.Units,
                [unit => unit.Quantity],
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (units is null) return [];
                
            var dtos = units
                .Select(unit => new UnitDto
                {
                    Id = unit.Id,
                    QuantityId = unit.Quantity.Id,
                    Name = unit.Name,
                    Symbol = unit.Symbol,
                    RuName = unit.RuName,
                    RuSymbol = unit.RuSymbol
                }).ToList();

            return dtos;
        }
    }
}