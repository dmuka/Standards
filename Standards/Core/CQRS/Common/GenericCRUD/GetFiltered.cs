using FluentValidation;
using MediatR;
using Standards.Core.Models;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Filter.Implementations;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.QueryableWrapper.Interface;
using Standards.Infrastructure.Validators.Constants;

namespace Standards.Core.CQRS.Common.GenericCRUD
{
    public class GetFiltered<T> where T : BaseEntity
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
}