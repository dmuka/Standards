using Domain;
using Domain.Models;
using Domain.Models.Housings;
using Domain.Models.Interfaces;
using FluentValidation;
using MediatR;
using Infrastructure.Filter.Implementations;
using Infrastructure.Filter.Interfaces;
using Infrastructure.QueryableWrapper.Interface;
using Infrastructure.Validators.Constants;

namespace Application.CQRS.Common.GenericCRUD;

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
                        .GreaterThan(0);

                    q.RuleFor(qp => qp.ItemsOnPage)
                        .GreaterThan(0);
                });
        }
    }
}