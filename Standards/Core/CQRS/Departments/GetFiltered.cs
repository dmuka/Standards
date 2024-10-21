using FluentValidation;
using MediatR;
using Standards.Core.Models;
using Standards.Core.Models.Departments;
using Standards.Infrastructure.Filter.Implementations;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.QueryableWrapper.Interface;
using Standards.Infrastructure.Validators.Constants;

namespace Standards.Core.CQRS.Departments;

    public class GetFiltered
    {
        public class Query(QueryParameters parameters) : IRequest<PaginatedListModel<Department>>
        {
            public QueryParameters Parameters { get; } = parameters;
        }

        public class QueryHandler(IQueryBuilder<Department> queryBuilder, IQueryableWrapper<Department> queryableWrapper)
            : IRequestHandler<Query, PaginatedListModel<Department>>
        {
            
            public async Task<PaginatedListModel<Department>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = queryBuilder.Execute(request.Parameters);

                var housings = await queryableWrapper.ToListAsync(query, cancellationToken);

                var result = PaginatedListModel<Department>.ApplyPagination(
                    housings, 
                    request.Parameters.PageNumber, 
                    request.Parameters.ItemsOnPage);
                
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