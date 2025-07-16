using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.Abstractions.Data.Validators;
using Application.UseCases.Common.Attributes;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.Persons;
using Domain.Models.Services;
using Domain.Models.Standards;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Standards;

[TransactionScope]
public class Edit
{
    public class Query(StandardDto standardDto) : IRequest<int>
    {
        public StandardDto StandardDto { get; } = standardDto;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var characteristics = repository.GetQueryable<Characteristic>()
                .Where(characteristic => request.StandardDto.CharacteristicIds.Contains(characteristic.Id))
                .ToList();

            var standards = repository.GetQueryable<Service>()
                .Where(standard => request.StandardDto.ServiceIds.Contains(standard.Id))
                .ToList();

            var workplaces = repository.GetQueryable<Workplace>()
                .Where(workplace => request.StandardDto.WorkplaceIds.Contains(workplace.Id))
                .ToList();

            var responsible = await repository.GetByIdAsync<Person>(request.StandardDto.ResponsibleId, cancellationToken);
            
            var standard = StandardDto.ToEntity(request.StandardDto, workplaces, characteristics, standards, responsible!);
                
            repository.Update(standard);

            var result = await repository.SaveChangesAsync(cancellationToken);
                
            cacheService.Remove(Cache.Standards);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.StandardDto)
                .NotEmpty()
                .ChildRules(standard =>
                {
                    standard.RuleFor(dto => dto.Id)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<Service>(repository));
                    
                    standard.RuleFor(dto => dto.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);

                    standard.RuleFor(dto => dto.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.ShortName);
                    
                    standard.RuleFor(dto => dto.ResponsibleId)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<Person>(repository));

                    standard.RuleFor(dto => dto.VerificationInterval)
                        .GreaterThan(Domain.Constants.Standards.MinVerificationInterval);
                    
                    standard.RuleFor(dto => dto.CalibrationInterval)
                        .GreaterThan(Domain.Constants.Standards.MinCalibrationInterval)
                        .When(dto => dto.CalibrationInterval is not null);

                    standard.RuleFor(dto => dto.CharacteristicIds)
                        .NotEmpty()
                        .ForEach(id => 
                            id.SetValidator(new IdValidator<Characteristic>(repository)));

                    standard.RuleFor(dto => dto.ServiceIds)
                        .NotEmpty()
                        .ForEach(id => 
                            id.SetValidator(new IdValidator<Service>(repository)));

                    standard.RuleFor(dto => dto.WorkplaceIds)
                        .NotEmpty()
                        .ForEach(id => 
                            id.SetValidator(new IdValidator<Workplace>(repository)));
                });
        }
    }
}