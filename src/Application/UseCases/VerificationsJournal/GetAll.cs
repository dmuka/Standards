using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models.MetrologyControl;
using MediatR;

namespace Application.UseCases.VerificationsJournal;

public class GetAll
{
    public class Query : IRequest<IList<VerificationJournalItemDto>>
    {
    }

    public class QueryHandler( 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query, IList<VerificationJournalItemDto>>
    {
        public async Task<IList<VerificationJournalItemDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
                
            var verificationJournalItems = await cache.GetOrCreateAsync<VerificationJournalItem>(
                Cache.VerificationJournal,
                [
                    vi => vi.Standard,
                    vi => vi.Place
                ],
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (verificationJournalItems is null) return [];
                
            var dtos = verificationJournalItems
                .Select(VerificationJournalItemDto.ToDto).ToList();

            return dtos;
        }
    }
}