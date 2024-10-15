using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.Filter.Models;

namespace Standards.Core.CQRS.Rooms
{
    public class GetFiltered
    {
        public class Query(FilterDto filter) : IRequest<PaginatedListModel<Room>>
        {
            public FilterDto Filter { get; set; } = filter;
        }

        public class QueryHandler(IQueryBuilder<Room> queryBuilder)
            : IRequestHandler<Query, PaginatedListModel<Room>>
        {
            public async Task<PaginatedListModel<Room>> Handle(Query request, CancellationToken cancellationToken)
            {
                var paginator = new Paginator(request.Filter.Page, request.Filter.ItemsPerPage);

                var filter = new Filter<Room>(r => r.Name.Contains(request.Filter.SearchQuery));

                var sorter = new Sorter<Room, string>(r => r.Name);

                var query = queryBuilder
                    .AddPaginator(paginator)
                    .AddFilter(filter)
                    .AddSorter(sorter)
                    .Execute();

                var rooms = await query.ToListAsync(cancellationToken);

                var result = new PaginatedListModel<Room>(
                    rooms,
                    request.Filter.Page,
                    rooms.Count,
                    request.Filter.ItemsPerPage);

                return result;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(_ => _.Filter)
                    .NotEmpty()
                    .ChildRules(filter =>
                    {
                        filter.RuleFor(_ => _.SearchQuery)
                            .NotNull();

                        filter.RuleFor(_ => _.Page)
                            .GreaterThan(default(int));

                        filter.RuleFor(_ => _.ItemsPerPage)
                            .GreaterThan(default(int));
                    });
            }
        }
    }
}