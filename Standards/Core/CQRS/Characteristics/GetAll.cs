using MediatR;
using Microsoft.EntityFrameworkCore;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Standards;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.Core.CQRS.Characteristics
{
    public class GetAll
    {
        public class Query : IRequest<IList<CharacteristicDto>>
        {
        }

        public class QueryHandler(
            IRepository repository, 
            ICacheService cache, 
            IConfigService configService) : IRequestHandler<Query, IList<CharacteristicDto>>
        {
            public async Task<IList<CharacteristicDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
                var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
                var characteristics = await cache.GetOrCreateAsync<Characteristic>(
                    Cache.Characteristics,
                    async (token) =>
                    {
                        var result = await repository.GetListAsync<Characteristic>(
                            query => query
                                .Include(c => c.Unit)
                                .Include(c => c.Grade)
                                .Include(c => c.Standard),
                            token);

                        return result;
                    },
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
}