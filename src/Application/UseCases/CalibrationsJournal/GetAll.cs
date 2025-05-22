using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Domain.Constants;
using Domain.Models.DTOs;
using Domain.Models.MetrologyControl;
using MediatR;

namespace Application.UseCases.CalibrationsJournal;

public class GetAll
{
    public class Query : IRequest<IList<CalibrationJournalItemDto>>
    {
    }

    public class QueryHandler( 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<CalibrationJournalItemDto>>
    {
        public async Task<IList<CalibrationJournalItemDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
            var calibrationJournalItems = await cache.GetOrCreateAsync<CalibrationJournalItem>(
                Cache.CalibrationJournal,
                [
                    vi => vi.Standard,
                    vi => vi.Place
                ],
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (calibrationJournalItems is null) return [];
                
            var dtos = calibrationJournalItems
                .Select(CalibrationJournalItem.ToDto).ToList();

            return dtos;
        }
    }
}