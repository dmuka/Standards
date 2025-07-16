using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.Abstractions.Data.Validators;
using Application.Exceptions;
using Application.UseCases.Common.Attributes;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.Persons;
using Domain.Models.Users;
using FluentValidation;
using Infrastructure.Exceptions;
using Infrastructure.Exceptions.Enum;
using MediatR;

namespace Application.UseCases.Persons;

[TransactionScope]
public class Edit
{
    public class Query(PersonDto personDto) : IRequest<int>
    {
        public PersonDto PersonDto { get; } = personDto;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var position =  await repository.GetByIdAsync<Position>(request.PersonDto.PositionId, cancellationToken);
            
            var category = await repository.GetByIdAsync<Category>(request.PersonDto.CategoryId, cancellationToken);
            
            var sector =  await repository.GetByIdAsync<Sector>(request.PersonDto.SectorId, cancellationToken);
            
            var user =  await repository.GetByIdAsync<User>(request.PersonDto.UserId, cancellationToken);
                
            var person = new Person
            {
                Id = request.PersonDto.Id,
                FirstName = request.PersonDto.FirstName,
                MiddleName = request.PersonDto.MiddleName,
                LastName = request.PersonDto.LastName,
                BirthdayDate = request.PersonDto.BirthdayDate,
                Role = request.PersonDto.Role,
                Category = category!,
                Position = position!,
                Sector = sector!,
                User = user!,
                Comments = request.PersonDto.Comments
            };
                
            repository.Update(person);

            var result = await repository.SaveChangesAsync(cancellationToken);
                
            cacheService.Remove(Cache.Persons);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.PersonDto)
                .NotEmpty()
                .ChildRules(person =>
                {
                    person.RuleFor(dto => dto.Id)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<Person>(repository));

                    person.RuleFor(dto => dto.FirstName)
                        .NotEmpty()
                        .MaximumLength(Lengths.PersonName);

                    person.RuleFor(dto => dto.MiddleName)
                        .NotEmpty()
                        .MaximumLength(Lengths.PersonName);

                    person.RuleFor(dto => dto.LastName)
                        .NotEmpty()
                        .MaximumLength(Lengths.PersonName);

                    person.RuleFor(dto => dto.CategoryId)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<Category>(repository));

                    person.RuleFor(dto => dto.PositionId)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<Position>(repository));

                    person.RuleFor(dto => dto.SectorId)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<Sector>(repository));

                    person.RuleFor(dto => dto.UserId)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<User>(repository));

                    person.RuleFor(dto => dto.BirthdayDate)
                        .NotEmpty();

                    person.RuleFor(dto => dto.Role)
                        .NotEmpty()
                        .MaximumLength(Lengths.Role);
                });
        }
    }
}