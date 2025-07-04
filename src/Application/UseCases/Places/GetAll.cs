﻿using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models.MetrologyControl;
using MediatR;

namespace Application.UseCases.Places;

public class GetAll
{
    public class Query : IRequest<IList<PlaceDto>>
    {
    }

    public class QueryHandler( 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<PlaceDto>>
    {
        public async Task<IList<PlaceDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);

            var places = await cache.GetOrCreateAsync<Place>(
                Cache.Places,
                [p => p.Contacts],
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (places is null) return [];
                
            var dtos = places.Select(PlaceDto.ToDto).ToList();

            return dtos;
        }
    }
}