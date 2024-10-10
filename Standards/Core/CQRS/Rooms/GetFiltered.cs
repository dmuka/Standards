using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Filters;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.Filter.Models;

namespace Standards.Core.CQRS.Rooms
{
    public class GetFiltered
    {
        public class Query(RoomsFilter filter) : IRequest<PaginatedListModel<Room>>
        {
            public RoomsFilter Filter { get; set; } = filter;
        }

        public class QueryHandler(IQueryBuilder<Room> queryBuilder)
            : IRequestHandler<Query, PaginatedListModel<Room>>
        {
            public async Task<PaginatedListModel<Room>> Handle(Query request, CancellationToken cancellationToken)
            {
                var filter = new Filter<Room>(r => r.Name.Contains(request.Filter.SearchQuery))
                {
                    PageNumber = request.Filter.PageNumber,
                    ItemsPerPage = request.Filter.ItemsPerPage
                };
                
                var sorter = new Sorter<Room, string>(h => h.Name)
                {
                    PageNumber = request.Filter.PageNumber,
                    ItemsPerPage = request.Filter.ItemsPerPage
                };
            
                var query = queryBuilder
                    .AddFilter(filter)
                    .AddSorter(sorter)
                    .Filter()
                    .Sort()
                    .Paginate()
                    .GetQuery();

                var rooms = await query.ToListAsync(cancellationToken);

                var result = new PaginatedListModel<Room>(
                    rooms,
                    request.Filter.PageNumber,
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

                        filter.RuleFor(_ => _.PageNumber)
                            .GreaterThan(default(int));

                        filter.RuleFor(_ => _.ItemsPerPage)
                            .GreaterThan(default(int));
                    });
            }
        }
    }
}