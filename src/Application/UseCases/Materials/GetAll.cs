﻿using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models.Services;
using MediatR;

namespace Application.UseCases.Materials;

public class GetAll
{
    public class Query : IRequest<IList<MaterialDto>>
    {
    }

    public class QueryHandler( 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<MaterialDto>>
    {
        public async Task<IList<MaterialDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
            var materials = await cache.GetOrCreateAsync<Material>(
                Cache.Materials,
                [m => m.Unit],
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (materials is null) return [];
                
            var dtos = materials
                .Select(m => new MaterialDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    ShortName = m.ShortName,
                    UnitId = m.Unit.Id,
                    Comments = m.Comments
                }).ToList();

            return dtos;
        }
    }
}