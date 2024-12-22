using Application.Abstractions.Cache;
using Application.CQRS.Common.Attributes;
using Domain.Constants;
using Domain.Models.DTOs;
using Domain.Models.MetrologyControl;
using Domain.Models.Standards;
using FluentValidation;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Validators;
using MediatR;

namespace Application.CQRS.VerificationsJournal;

[TransactionScope]
public class Create
{
    public class Query(VerificationJournalItemDto verificationJournalItemDto) : IRequest<int>
    {
        public VerificationJournalItemDto VerificationJournalItemDto { get; } = verificationJournalItemDto;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var standard = await repository.GetByIdAsync<Standard>(request.VerificationJournalItemDto.StandardId, cancellationToken);

            var place = await repository.GetByIdAsync<Place>(request.VerificationJournalItemDto.PlaceId, cancellationToken);
            
            var verificationJournalItem = VerificationJournalItem.ToEntity(request.VerificationJournalItemDto, place, standard);
            
            await repository.AddAsync(verificationJournalItem, cancellationToken);
            
            var result = await repository.SaveChangesAsync(cancellationToken);
            
            cacheService.Remove(Cache.VerificationJournal);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.VerificationJournalItemDto)
                .NotEmpty()
                .ChildRules(service =>
                {
                    service.RuleFor(dto => dto.CertificateId)
                        .NotEmpty()
                        .MaximumLength(Lengths.CertificateId);
                    
                    service.RuleFor(dto => dto.StandardId)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<Standard>(repository));
                    
                    service.RuleFor(dto => dto.PlaceId)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<Place>(repository));
                    
                    service.RuleFor(dto => dto.Date)
                        .NotEmpty()
                        .GreaterThan(DateTime.UtcNow).WithMessage("Date can't be in the past.");
                    
                    service.RuleFor(dto => dto.ValidTo)
                        .NotEmpty()
                        .GreaterThan(dto => dto.Date).WithMessage("Valid to date can't be less than date of verification.");
                });
        }
    }
}