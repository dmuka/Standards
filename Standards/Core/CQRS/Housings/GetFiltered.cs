using FluentValidation;
using MediatR;
using Standards.Core.Models;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Filter.Implementations;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.QueryableWrapper.Interface;
using Standards.Infrastructure.Validators.Constants;

namespace Standards.Core.CQRS.Housings;

    public class GetFiltered
    {
        public class Query(QueryParameters parameters) : IRequest<PaginatedListModel<Housing>>
        {
            public QueryParameters Parameters { get; set; } = parameters;
        }

        public class QueryHandler(IQueryBuilder<Housing> queryBuilder, IQueryableWrapper<Housing> queryableWrapper)
            : IRequestHandler<Query, PaginatedListModel<Housing>>
        {
            
            public async Task<PaginatedListModel<Housing>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = queryBuilder.Execute(request.Parameters);

                var housings = await queryableWrapper.ToListAsync(query, cancellationToken);

                PaginatedListModel<Housing> result = null;
                
                if (housings is not null)
                {
                    result = new PaginatedListModel<Housing>(
                        housings,
                        request.Parameters.PageNumber,
                        housings.Count,
                        request.Parameters.ItemsOnPage);
                }

                return result;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(_ => _.Parameters)
                    .NotEmpty()
                    .ChildRules(filter =>
                    {
                        filter.RuleFor(_ => _.SearchString)
                            .NotNull();
                        
                        filter.RuleFor(_ => _.SearchBy)
                            .NotNull().WithMessage(ValidationErrors.WrongEnumValue);
                        
                        filter.RuleFor(_ => _.SortBy)
                            .NotNull().WithMessage(ValidationErrors.WrongEnumValue);

                        filter.RuleFor(_ => _.PageNumber)
                            .GreaterThan(default(int));

                        filter.RuleFor(_ => _.ItemsOnPage)
                            .GreaterThan(default(int));
                    });
            }
        }
    }