using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.Abstractions.Data.Validators;
using Application.UseCases.Common.Attributes;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models.Persons;
using Domain.Models.Services;
using Domain.Models.Standards;
using FluentValidation;
using MediatR;

namespace Application.UseCases.ServicesJournal;

[TransactionScope]
public class Create
{
    public class Query(ServiceJournalItemDto serviceJournalItemDto) : IRequest<int>
    {
        public ServiceJournalItemDto ServiceJournalItemDto { get; } = serviceJournalItemDto;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var standard = await repository.GetByIdAsync<Standard>(request.ServiceJournalItemDto.StandardId, cancellationToken);

            var person = await repository.GetByIdAsync<Person>(request.ServiceJournalItemDto.PersonId, cancellationToken);

            var service = await repository.GetByIdAsync<Service>(request.ServiceJournalItemDto.ServiceId, cancellationToken);
            
            var serviceJournalItem = new ServiceJournalItem
            {
                Name = request.ServiceJournalItemDto.Name,
                ShortName = request.ServiceJournalItemDto.ShortName,
                Standard = standard,
                Person = person,
                Service = service,
                Date = request.ServiceJournalItemDto.Date
            };

            if (request.ServiceJournalItemDto.Comments is not null) serviceJournalItem.Comments = request.ServiceJournalItemDto.Comments;
            
            await repository.AddAsync(serviceJournalItem, cancellationToken);
            
            var result = await repository.SaveChangesAsync(cancellationToken);
            
            cacheService.Remove(Cache.ServiceJournal);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.ServiceJournalItemDto)
                .NotEmpty()
                .ChildRules(service =>
                {
                    service.RuleFor(dto => dto.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);

                    service.RuleFor(dto => dto.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.ShortName);
                    
                    service.RuleFor(dto => dto.ServiceId)
                        .GreaterThan(0);
                    
                    service.RuleFor(dto => dto.StandardId)
                        .GreaterThan(0);
                    
                    service.RuleFor(dto => dto.PersonId)
                        .GreaterThan(0);
                    
                    service.RuleFor(dto => dto.Date)
                        .NotEmpty()
                        .GreaterThan(DateTime.UtcNow).WithMessage("Date can't be in the past.");
                });
        }
    }
}