﻿using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.Abstractions.Data.Validators;
using Application.UseCases.Common.Attributes;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models;
using FluentValidation;
using MediatR;
using Unit = Domain.Models.Unit;

namespace Application.UseCases.Quantities;

[TransactionScope]
public class Create
{
    public class Query(QuantityDto quantityDto) : IRequest<int>
    {
        public QuantityDto QuantityDto { get; } = quantityDto;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var units =  repository.GetQueryable<Unit>()
                .Where(unit => request.QuantityDto.UnitIds
                    .Contains(unit.Id))
                .ToList();
                
            var quantity = new Quantity
            {
                Name = request.QuantityDto.Name,
                ShortName = request.QuantityDto.ShortName,
                Units = units
            };
            
            await repository.AddAsync(quantity, cancellationToken);
            
            var result = await repository.SaveChangesAsync(cancellationToken);
            
            cacheService.Remove(Cache.Quantities);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.QuantityDto)
                .NotEmpty()
                .ChildRules(quantity =>
                {
                    quantity.RuleFor(dto => dto.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.QuantityName);;

                    quantity.RuleFor(dto => dto.UnitIds)
                        .NotEmpty()
                        .ForEach(id => id.GreaterThan(0));
                });
        }
    }
}