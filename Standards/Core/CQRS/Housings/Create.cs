﻿using FluentValidation;
using MediatR;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using System.Data;

namespace Standards.Core.CQRS.Housings
{
    [TransactionScope]
    public class Create
    {
        public class Query : IRequest<int>
        {
            public Query(HousingDto housingDto)
            {
                HousingDto = housingDto;
            }

            public HousingDto HousingDto { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, int>
        {
            private readonly IRepository _repository;

            public QueryHandler(IRepository repository)
            {
                _repository = repository;
            }

            public async Task<int> Handle(Query request, CancellationToken cancellationToken)
            {
                await _repository.AddAsync(request.HousingDto, cancellationToken);

                var result = await _repository.SaveChangesAsync(cancellationToken);

                return result;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleLevelCascadeMode = CascadeMode.Stop;

                RuleFor(query => query.HousingDto)
                    .NotEmpty()
                    .ChildRules(filter =>
                    {
                        filter.RuleFor(_ => _.Name)
                            .NotEmpty();

                        filter.RuleFor(_ => _.ShortName)
                            .NotEmpty();

                        filter.RuleFor(_ => _.FloorsCount)
                            .GreaterThan(default(int));

                        filter.RuleFor(_ => _.Address)
                            .NotEmpty();
                    });
            }
        }

    }
}