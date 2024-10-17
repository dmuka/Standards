using MediatR;
using Microsoft.EntityFrameworkCore;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.Core.CQRS.Rooms
{
    public class GetAll
    {
        public class Query : IRequest<IEnumerable<Room>>
        {
        }

        public class QueryHandler(
            IRepository repository, 
            ICacheService cache, 
            IConfigService configService) : IRequestHandler<Query, IEnumerable<Room>>
        {
            public async Task<IEnumerable<Room>> Handle(Query request, CancellationToken cancellationToken)
            {
                var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
                var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);

                var rooms = await cache.GetOrCreateAsync(
                    Cache.Rooms,
                    async (token) =>
                    {
                        var result = await repository.GetListAsync<Room>(
                            query => query
                                .Include(r => r.WorkPlaces)
                                .Include(r => r.Persons),
                            token);

                        return result;
                    },
                    cancellationToken,
                    TimeSpan.FromMinutes(absoluteExpiration),
                    TimeSpan.FromMinutes(slidingExpiration));
                
                return rooms is null ? Array.Empty<Room>() : rooms;
            }
        }
    }
}