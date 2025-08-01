﻿using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.Abstractions.Data.Validators;
using Application.UseCases.Common.Attributes;
using Application.UseCases.DTOs;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.Housings;
using Domain.Models.Persons;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Workplaces;

[TransactionScope]
public class Edit
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
                Responsible = responsible!,
                Comments = request.WorkplaceDto.Comments
            };
                
            repository.Update(workplace);

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
                    workplace.RuleFor(dto => dto.Id)
                        .GreaterThan(0);

                    workplace.RuleFor(dto => dto.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);

                    workplace.RuleFor(dto => dto.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.ShortName);
                    
                    workplace.RuleFor(dto => dto.RoomId)
                        .GreaterThan(0);
                    
                    workplace.RuleFor(dto => dto.ResponsibleId)
                        .GreaterThan(0);
                });
        }
    }
}