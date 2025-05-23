using Application.Abstractions.Cache;
using Application.UseCases.Common.Attributes;
using Domain.Constants;
using Domain.Models.DTOs;
using Domain.Models.MetrologyControl;
using Domain.Models.Services;
using Domain.Models.Standards;
using FluentValidation;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.VerificationsJournal;

[TransactionScope]
public class Edit
{
    public class Command(VerificationJournalItemDto serviceJournalItemDto) : IRequest<int>
    {
        public VerificationJournalItemDto VerificationJournalItemDto { get; } = serviceJournalItemDto;
    }

    public class CommandHandler(
        IRepository repository, 
        ICacheService cacheService,
        ILogger<Edit> logger) : IRequestHandler<Command, int>
    {
        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var standard = await repository.GetByIdAsync<Standard>(request.VerificationJournalItemDto.StandardId, cancellationToken);
            if (standard is null)
            {
                logger.LogWarning("Invalid standard id {standardId} in the verification journal item", request.VerificationJournalItemDto.StandardId);
                
                return 0;
            }
            
            var place = await repository.GetByIdAsync<Place>(request.VerificationJournalItemDto.PlaceId, cancellationToken);
            if (place is null)
            {
                logger.LogWarning("Invalid place id {placeId} in the verification journal item", request.VerificationJournalItemDto.PlaceId);
                
                return 0;
            }
            
            var verificationJournalItem = VerificationJournalItem.ToEntity(request.VerificationJournalItemDto, place, standard);
                
            repository.Update(verificationJournalItem);

            var result = await repository.SaveChangesAsync(cancellationToken);
                
            cacheService.Remove(Cache.VerificationJournal);

            return result;
        }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.VerificationJournalItemDto)
                .NotEmpty()
                .ChildRules(service =>
                {
                    service.RuleFor(dto => dto.Id)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<Service>(repository));
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