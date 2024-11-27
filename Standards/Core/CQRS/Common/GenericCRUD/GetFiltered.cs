using FluentValidation;
using MediatR;
using Standards.Core.Models;
using Standards.Core.Models.Housings;
using Standards.Core.Models.Interfaces;
using Standards.Infrastructure.Filter.Implementations;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.QueryableWrapper.Interface;
using Standards.Infrastructure.Validators.Constants;

namespace Standards.Core.CQRS.Common.GenericCRUD;

public class GetFiltered<T> where T : BaseEntity, ICacheable
{
    public class Query(QueryParameters parameters) : IRequest<PaginatedListModel<T>>
    {
        public QueryParameters Parameters { get; } = parameters;
    }

    public class QueryHandler(IQueryBuilder<T> queryBuilder, IQueryableWrapper<T> queryableWrapper)
        : IRequestHandler<Query, PaginatedListModel<T>>
    {
        public async Task<PaginatedListModel<T>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = queryBuilder.Execute(request.Parameters);

            var rooms = await queryableWrapper.ToListAsync(query, cancellationToken);
            
            var result = PaginatedListModel<Room>.ApplyPagination(
                rooms, 
                request.Parameters.PageNumber, 
                request.Parameters.ItemsOnPage);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleFor(query => query.Parameters)
                .NotEmpty()
                .ChildRules(q =>
                {
                    q.RuleFor(qp => qp.SearchString)
                        .NotNull();
                    
                    q.RuleFor(qp => qp.SearchBy)
                        .NotNull().WithMessage(ValidationErrors.WrongEnumValue);
                    
                    q.RuleFor(qp => qp.SortBy)
                        .NotNull().WithMessage(ValidationErrors.WrongEnumValue);

                    q.RuleFor(qp => qp.PageNumber)
                        .GreaterThan(default(int));

                    q.RuleFor(qp => qp.ItemsOnPage)
                        .GreaterThan(default(int));
                });
        }
    }
}