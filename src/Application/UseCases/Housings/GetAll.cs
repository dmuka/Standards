﻿using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models.Housings;
using MediatR;

namespace Application.UseCases.Housings;

public class GetAll
{
    public class Query : IRequest<IList<HousingDto>>
    {
    }

    public class QueryHandler( 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<HousingDto>>
    {
        public async Task<IList<HousingDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
            var housings = await cache.GetOrCreateAsync<Housing>(
                Cache.Housings,
                [housing => housing.Rooms],
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (housings is null) return [];
                
            var dtos = housings
                .Select(h => new HousingDto()
                {
                    Id = h.Id,
                    Name = h.Name,
                    ShortName = h.ShortName,
                    Address = h.Address,
                    FloorsCount = h.FloorsCount,
                    Comments = h.Comments,
                    RoomIds = h.Rooms.Select(r => r.Id).ToList()
                }).ToList();

            return dtos;
        }
    }
}