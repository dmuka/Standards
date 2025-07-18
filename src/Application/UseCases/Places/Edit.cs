using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.Abstractions.Data.Validators;
using Application.UseCases.Common.Attributes;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models.MetrologyControl;
using Domain.Models.MetrologyControl.Contacts;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Places;

[TransactionScope]
public class Edit
{
    public class Command(PlaceDto dto) : IRequest<int>
    {
        public PlaceDto PlaceDto { get; } = dto;
    }

    public class CommandHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Command, int>
    {
        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var contacts = repository.GetQueryable<Contact>()
                .Where(contact => request.PlaceDto.ContactIds.Contains(contact.Id))
                .ToList();

            var place = PlaceDto.ToEntity(request.PlaceDto, contacts);
                
            repository.Update(place);

            var result = await repository.SaveChangesAsync(cancellationToken);
                
            cacheService.Remove(Cache.Places);

            return result;
        }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(command => command.PlaceDto)
                .NotEmpty()
                .ChildRules(place =>
                {
                    place.RuleFor(dto => dto.Id)
                        .GreaterThan(0);
                        
                    place.RuleFor(dto => dto.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);
                    
                    place.RuleFor(dto => dto.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.ShortName);
                    
                    place.RuleFor(dto => dto.Address)
                        .NotEmpty()
                        .MinimumLength(Lengths.AddressMinimum)
                        .MaximumLength(Lengths.AddressMaximum);

                    place.RuleFor(dto => dto.ContactIds)
                        .NotEmpty()
                        .ForEach(id => id.GreaterThan(0));
                });
        }
    }
}