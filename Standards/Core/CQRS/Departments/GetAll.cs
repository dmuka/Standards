using MediatR;
using Microsoft.EntityFrameworkCore;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.Core.CQRS.Departments;

public class GetAll
{
    public class Query : IRequest<IList<DepartmentDto>>
    {
    }

    public class QueryHandler(
        IRepository repository, 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<DepartmentDto>>
    {
        public async Task<IList<DepartmentDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
            var departments = await cache.GetOrCreateAsync<Department>(
                Cache.Departments,
                async (token) =>
                {
                    var result = await repository.GetListAsync<Department>(
                        query => query
                            //.Include(d => d.Housings)
                            .Include(d => d.Sectors),
                        token);

                    return result;
                },
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (departments is null) return [];
                
            var dtos = departments
                .Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    Comments = d.Comments,
                    SectorIds = d.Sectors.Select(s => s.Id).ToList()
                }).ToList();

            return dtos;
        }
    }
}