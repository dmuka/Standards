using Application.Abstractions.Cache;
using Application.UseCases.Common.Attributes;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models;
using Domain.Models.Departments;
using FluentValidation;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Validators;
using MediatR;
using Unit = Domain.Models.Unit;

namespace Application.UseCases.Units;

[TransactionScope]
public class Create
{
    public class Query(UnitDto dtoDto) : IRequest<int>
    {
        public UnitDto UnitDto { get; } = dtoDto;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var quantity = await repository.GetByIdAsync<Quantity>(request.UnitDto.QuantityId, cancellationToken);
            
            var unit = new Unit
            {
                Quantity = quantity,
                Name = request.UnitDto.Name,
                Symbol = request.UnitDto.Symbol,
                RuName = request.UnitDto.RuName,
                RuSymbol = request.UnitDto.RuSymbol
            };
            
            await repository.AddAsync(unit, cancellationToken);
            
            var result = await repository.SaveChangesAsync(cancellationToken);
            
            cacheService.Remove(Cache.Units);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.UnitDto)
                .NotEmpty()
                .ChildRules(unit =>
                {
                    unit.RuleFor(dto => dto.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.UnitName);
                    
                    unit.RuleFor(dto => dto.Symbol)
                        .NotEmpty()
                        .MaximumLength(Lengths.UnitSymbol);

                    unit.RuleFor(dto => dto.RuName)
                        .NotEmpty()
                        .MaximumLength(Lengths.UnitRuName);

                    unit.RuleFor(dto => dto.RuSymbol)
                        .NotEmpty()
                        .MaximumLength(Lengths.UnitRuSymbol);
                    
                    unit.RuleFor(dto => dto.QuantityId)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<Department>(repository));
                });
        }
    }
}