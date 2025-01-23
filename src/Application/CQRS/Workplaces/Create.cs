using Application.Abstractions.Cache;
using Application.CQRS.Common.Attributes;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.DTOs;
using Domain.Models.Housings;
using Domain.Models.Persons;
using FluentValidation;
using MediatR;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Validators;

namespace Application.CQRS.Workplaces;

[TransactionScope]
public class Create
{
    public class Query(WorkplaceDto workplaceDto) : IRequest<int>
    {
        public WorkplaceDto WorkplaceDto { get; } = workplaceDto;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var room = await repository.GetByIdAsync<Room>(request.WorkplaceDto.RoomId, cancellationToken);
            
            var responsible = await repository.GetByIdAsync<Person>(request.WorkplaceDto.ResponsibleId, cancellationToken);

            var workplace = new Workplace
            {
                Name = request.WorkplaceDto.Name,
                ShortName = request.WorkplaceDto.ShortName,
                Room = room!,
                Responsible = responsible!
            };

            if (request.WorkplaceDto.Comments is not null) workplace.Comments = request.WorkplaceDto.Comments;
            
            await repository.AddAsync(workplace, cancellationToken);
            
            var result = await repository.SaveChangesAsync(cancellationToken);
            
            cacheService.Remove(Cache.Workplaces);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.WorkplaceDto)
                .NotEmpty()
                .ChildRules(workplace =>
                {
                    workplace.RuleFor(dto => dto.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);

                    workplace.RuleFor(dto => dto.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.ShortName);
                    
                    workplace.RuleFor(dto => dto.RoomId)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<Room>(repository));
                    
                    workplace.RuleFor(dto => dto.ResponsibleId)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<Person>(repository));
                });
        }
    }

}