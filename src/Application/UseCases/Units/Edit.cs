using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.Abstractions.Data.Validators;
using Application.Exceptions;
using Application.UseCases.Common.Attributes;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models;
using Domain.Models.Departments;
using FluentValidation;
using Infrastructure.Exceptions.Enum;
using MediatR;
using Unit = Domain.Models.Unit;

namespace Application.UseCases.Units;

[TransactionScope]
public class Edit
{
    public class Query(UnitDto unitDto) : IRequest<int>
    {
        public UnitDto UnitDto { get; } = unitDto;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return 0;
            
            var quantity = await repository.GetByIdAsync<Quantity>(request.UnitDto.QuantityId, cancellationToken);
            if (quantity is null)
                throw new StandardsException(StatusCodeByError.InternalServerError, "Every unit must have quantity",
                    "Some error");
            
            var unit = new Unit
            {
                Id = request.UnitDto.Id,
                Quantity = quantity,
                Name = request.UnitDto.Name,
                Symbol = request.UnitDto.Symbol,
                RuName = request.UnitDto.RuName,
                RuSymbol = request.UnitDto.RuSymbol
            };
                
            repository.Update(unit);

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
                    unit.RuleFor(dto => dto.Id)
                        .GreaterThan(0);
                    
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
                        .GreaterThan(0);
                });
        }
    }
}