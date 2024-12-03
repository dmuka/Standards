using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Repositories.Interfaces;

namespace Application.CQRS.Sectors
{
    public class GetAll
    {
        public class Query : IRequest<IList<SectorDto>>
        {
        }

        public class QueryHandler(
            IRepository repository, 
            ICacheService cache, 
            IConfigService configService) : IRequestHandler<Query, IList<SectorDto>>
        {
            public async Task<IList<SectorDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
                var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
                var sectors = await cache.GetOrCreateAsync<Sector>(
                    Cache.Sectors,
                    async (token) =>
                    {
                        var result = await repository.GetListAsync<Sector>(
                            query => query
                                .Include(s => s.Workplaces)
                                .Include(s => s.Persons)
                                .Include(s => s.Rooms)
                                .Include(s => s.Department),
                            token);

                        return result;
                    },
                    cancellationToken,
                    TimeSpan.FromMinutes(absoluteExpiration),
                    TimeSpan.FromMinutes(slidingExpiration));

                if (sectors is null) return [];
                
                var dtos = sectors
                    .Select(s => new SectorDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        ShortName = s.ShortName,
                        Comments = s.Comments,
                        DepartmentId = s.Department.Id,
                        RoomIds = s.Rooms.Select(r => r.Id).ToList(),
                        WorkplaceIds = s.Workplaces.Select(wp => wp.Id).ToList(),
                        PersonIds = s.Persons.Select(p => p.Id).ToList()
                    }).ToList();

                return dtos;
            }
        }
    }
}