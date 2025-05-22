using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Domain.Constants;
using Domain.Models.DTOs;
using Domain.Models.Standards;
using MediatR;

namespace Application.UseCases.Characteristics;

public class GetAll
{
    public class Query : IRequest<IList<CharacteristicDto>>
    {
    }

    public class QueryHandler( 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<CharacteristicDto>>
    {
        public async Task<IList<CharacteristicDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
            var characteristics = await cache.GetOrCreateAsync<Characteristic>(
                Cache.Characteristics,
                [
                    c => c.Unit,
                    c => c.Grade,
                    c => c.Standard
                ],
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (characteristics is null) return [];
                
            var dtos = characteristics
                .Select(c => new CharacteristicDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                    ShortName = c.ShortName,
                    Comments = c.Comments,
                    UnitId = c.Unit.Id,
                    GradeId = c.Grade.Id,
                    StandardId = c.Standard.Id,
                    RangeStart = c.RangeStart,
                    RangeEnd = c.RangeEnd,
                    GradeValue = c.GradeValue,
                    GradeValueStart = c.GradeValueStart,
                    GradeValueEnd = c.GradeValueEnd
                }).ToList();

            return dtos;
        }
    }
}