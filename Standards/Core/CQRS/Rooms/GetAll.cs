using MediatR;
using Microsoft.EntityFrameworkCore;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.Core.CQRS.Rooms
{
    public class GetAll
    {
        public class Query : IRequest<IEnumerable<RoomDto>>
        {
        }

        public class QueryHandler(
            IRepository repository, 
            ICacheService cache, 
            IConfigService configService) : IRequestHandler<Query, IEnumerable<RoomDto>>
        {
            public async Task<IEnumerable<RoomDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
                var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);

                var rooms = await cache.GetOrCreateAsync<Room>(
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

                if (rooms is null) return [];
                
                var dtos = rooms
                    .Select(r => new RoomDto()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        ShortName = r.ShortName,
                        Floor = r.Floor,
                        Length = r.Length,
                        Width = r.Width,
                        Height = r.Height,
                        HousingId = r.Housing.Id,
                        SectorId = r.Sector.Id,
                        Comments = r.Comments,
                        WorkplaceIds = r.WorkPlaces.Select(d => d.Id).ToList(),
                        PersonIds = r.Persons.Select(d => d.Id).ToList()
                    }).ToList();

                return dtos;
            }
        }
    }
}