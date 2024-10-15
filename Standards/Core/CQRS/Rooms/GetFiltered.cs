using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Filter.Implementations;
using Standards.Infrastructure.Filter.Interfaces;

namespace Standards.Core.CQRS.Rooms
{
    public class GetFiltered
    {
        public class Query(QueryParameters parameters) : IRequest<PaginatedListModel<Room>>
        {
            public QueryParameters Parameters { get; set; } = parameters;
        }

        public class QueryHandler(IQueryBuilder<Room> queryBuilder)
            : IRequestHandler<Query, PaginatedListModel<Room>>
        {
            public async Task<PaginatedListModel<Room>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = queryBuilder.Execute(request.Parameters);

                var rooms = await query.ToListAsync(cancellationToken);

                var result = new PaginatedListModel<Room>(
                    rooms,
                    request.Parameters.PageNumber,
                    rooms.Count,
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

                        filter.RuleFor(_ => _.PageNumber)
                            .GreaterThan(default(int));

                        filter.RuleFor(_ => _.ItemsOnPage)
                            .GreaterThan(default(int));
                    });
            }
        }
    }
}