using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Domain.Constants;
using Domain.Models.DTOs;
using Domain.Models.Services;
using MediatR;

namespace Application.UseCases.ServicesJournal;

public class GetAll
{
    public class Query : IRequest<IList<ServiceJournalItemDto>>
    {
    }

    public class QueryHandler( 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<ServiceJournalItemDto>>
    {
        public async Task<IList<ServiceJournalItemDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
            var serviceJournalItems = await cache.GetOrCreateAsync<ServiceJournalItem>(
                Cache.ServiceJournal,
                [
                    si => si.Standard,
                    si => si.Service,
                    si => si.Person
                ],
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (serviceJournalItems is null) return [];
                
            var dtos = serviceJournalItems
                .Select(si => new ServiceJournalItemDto
                {
                    Id = si.Id,
                    Name = si.Name,
                    ShortName = si.ShortName,
                    Comments = si.Comments,
                    ServiceId = si.Service.Id,
                    StandardId = si.Standard.Id,
                    PersonId = si.Person.Id,
                    Date = si.Date
                }).ToList();

            return dtos;
        }
    }
}