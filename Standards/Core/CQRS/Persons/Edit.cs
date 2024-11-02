using FluentValidation;
using MediatR;
using Standards.Core.Constants;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Persons;
using Standards.Core.Models.Users;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;
using Standards.Infrastructure.Validators;

namespace Standards.Core.CQRS.Persons;

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
                Category = category,
                Position = position,
                Sector = sector,
                User = user,
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
                        .GreaterThan(default(int))
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
                        .GreaterThan(default(int))
                        .SetValidator(new IdValidator<Category>(repository));

                    person.RuleFor(dto => dto.PositionId)
                        .GreaterThan(default(int))
                        .SetValidator(new IdValidator<Position>(repository));

                    person.RuleFor(dto => dto.SectorId)
                        .GreaterThan(default(int))
                        .SetValidator(new IdValidator<Sector>(repository));

                    person.RuleFor(dto => dto.UserId)
                        .GreaterThan(default(int))
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