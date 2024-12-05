using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Repositories.Interfaces;

namespace Application.CQRS.Departments;

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
                [d => d.Sectors],
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