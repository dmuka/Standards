using Domain;
using Domain.Models;
using Domain.Models.Interfaces;
using FluentValidation;
using Infrastructure.Filter.Implementations;
using Infrastructure.Filter.Interfaces;
using Infrastructure.QueryableWrapper.Interface;
using Infrastructure.Validators.Constants;
using MediatR;

namespace Application.UseCases.Common.GenericCRUD;

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

            var entities = await queryableWrapper.ToListAsync(query, cancellationToken) ?? [];
            
            var result = PaginatedListModel<T>.ApplyPagination(
                entities, 
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