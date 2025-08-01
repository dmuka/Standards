﻿using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models.Housings;
using MediatR;

namespace Application.UseCases.Rooms;

public class GetAll
{
    public class Query : IRequest<IEnumerable<RoomDto>>
    {
    }

    public class QueryHandler(
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IEnumerable<RoomDto>>
    {
        public async Task<IEnumerable<RoomDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);

            var rooms = await cache.GetOrCreateAsync<Room>(
                Cache.Rooms,
                [
                    r => r.WorkPlaces,
                    r => r.Persons,
                    r => r.Sector,
                    r => r.Housing
                ],
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (rooms is null) return [];
                
            var dtos = rooms
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    ShortName = r.ShortName,
                    Floor = r.Floor,
                    Length = r.Length,
                    Width = r.Width,
                    Height = r.Height,
                    HousingId = r.Housing!.Id,
                    SectorId = r.Sector!.Id,
                    Comments = r.Comments,
                    WorkplaceIds = r.WorkPlaces.Select(d => d.Id).ToList(),
                    PersonIds = r.Persons.Select(d => d.Id).ToList()
                }).ToList();

            return dtos;
        }
    }
}