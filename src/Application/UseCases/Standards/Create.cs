﻿using Application.Abstractions.Cache;
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
public class Create
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

            var services = repository.GetQueryable<Service>()
                .Where(service => request.StandardDto.ServiceIds.Contains(service.Id))
                .ToList();

            var workplaces = repository.GetQueryable<Workplace>()
                .Where(workplace => request.StandardDto.WorkplaceIds.Contains(workplace.Id))
                .ToList();

            var responsible = await repository.GetByIdAsync<Person>(request.StandardDto.ResponsibleId, cancellationToken);
            
            var standard = StandardDto.ToEntity(request.StandardDto, workplaces, characteristics, services, responsible!);
            
            await repository.AddAsync(standard, cancellationToken);
            
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
                .ChildRules(service =>
                {
                    service.RuleFor(dto => dto.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);

                    service.RuleFor(dto => dto.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.ShortName);
                    
                    service.RuleFor(dto => dto.ResponsibleId)
                        .GreaterThan(0);

                    service.RuleFor(dto => dto.VerificationInterval)
                        .GreaterThan(Domain.Constants.Standards.MinVerificationInterval);
                    
                    service.RuleFor(dto => dto.CalibrationInterval)
                        .GreaterThan(Domain.Constants.Standards.MinCalibrationInterval)
                        .When(dto => dto.CalibrationInterval is not null);

                    service.RuleFor(dto => dto.CharacteristicIds)
                        .NotEmpty()
                        .ForEach(id => id.GreaterThan(0));

                    service.RuleFor(dto => dto.ServiceIds)
                        .NotEmpty()
                        .ForEach(id => id.GreaterThan(0));

                    service.RuleFor(dto => dto.WorkplaceIds)
                        .NotEmpty()
                        .ForEach(id => id.GreaterThan(0));
                });
        }
    }
}