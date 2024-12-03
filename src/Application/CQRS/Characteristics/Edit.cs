using Application.Abstractions.Cache;
using Application.CQRS.Common.Attributes;
using Domain.Constants;
using Domain.Models.DTOs;
using Domain.Models.Standards;
using FluentValidation;
using MediatR;
using Standards.Core.Constants;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Validators;
using Unit = Domain.Models.Unit;

namespace Application.CQRS.Characteristics
{
    [TransactionScope]
    public class Edit
    {
        public class Query(CharacteristicDto characteristicDto) : IRequest<int>
        {
            public CharacteristicDto CharacteristicDto { get; } = characteristicDto;
        }

        public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
        {
            public async Task<int> Handle(Query request, CancellationToken cancellationToken)
            {
                var unit = await repository.GetByIdAsync<Unit>(request.CharacteristicDto.UnitId, cancellationToken);
            
                var grade = await repository.GetByIdAsync<Grade>(request.CharacteristicDto.GradeId, cancellationToken);
            
                var standard = await repository.GetByIdAsync<Standard>(request.CharacteristicDto.StandardId, cancellationToken);

                var characteristic = new Characteristic
                {
                    Name = request.CharacteristicDto.Name,
                    ShortName = request.CharacteristicDto.ShortName,
                    RangeStart = request.CharacteristicDto.RangeStart,
                    RangeEnd = request.CharacteristicDto.RangeEnd,
                    Unit = unit,
                    Grade = grade,
                    GradeValue = request.CharacteristicDto.GradeValue,
                    GradeValueStart = request.CharacteristicDto.GradeValueStart,
                    GradeValueEnd = request.CharacteristicDto.GradeValueEnd,
                    Standard = standard
                };
                
                repository.Update(characteristic);

                var result = await repository.SaveChangesAsync(cancellationToken);
                
                cacheService.Remove(Cache.Characteristics);

                return result;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator(IRepository repository)
            {
                RuleLevelCascadeMode = CascadeMode.Stop;

                RuleFor(query => query.CharacteristicDto)
                    .NotEmpty()
                    .ChildRules(dto =>
                    {
                        dto.RuleFor(characteristic => characteristic.Id)
                            .GreaterThan(default(int))
                            .SetValidator(new IdValidator<Unit>(repository));
                        
                        dto.RuleFor(characteristic => characteristic.Name)
                            .NotEmpty()
                            .MaximumLength(Lengths.EntityName);

                        dto.RuleFor(characteristic => characteristic.ShortName)
                            .NotEmpty()
                            .MaximumLength(Lengths.ShortName);

                        dto.RuleFor(characteristic => characteristic.RangeStart)
                            .NotEmpty();

                        dto.RuleFor(characteristic => characteristic.RangeEnd)
                            .NotEmpty();

                        dto.RuleFor(characteristic => characteristic.UnitId)
                            .GreaterThan(default(int))
                            .SetValidator(new IdValidator<Unit>(repository));

                        dto.RuleFor(characteristic => characteristic.GradeId)
                            .GreaterThan(default(int))
                            .SetValidator(new IdValidator<Grade>(repository));

                        dto.RuleFor(characteristic => characteristic.GradeValue)
                            .NotEmpty();

                        dto.RuleFor(characteristic => characteristic.GradeValueStart)
                            .NotEmpty();

                        dto.RuleFor(characteristic => characteristic.GradeValueEnd)
                            .NotEmpty();

                        dto.RuleFor(characteristic => characteristic.StandardId)
                            .GreaterThan(default(int))
                            .SetValidator(new IdValidator<Standard>(repository));
                    });
            }
        }
    }
}
