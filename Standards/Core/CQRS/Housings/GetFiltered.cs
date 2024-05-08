using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.DTOs.Filters;
using Standards.Infrastructure.Filter.Interfaces;

namespace Standards.Core.CQRS.Housings
{
    public class GetFiltered
    {
        public class Query : IRequest<PaginatedListModel<HousingDto>>
        {
            public Query(HousingsFilterDto filter)
            {
                Filter = filter;
            }

            public HousingsFilterDto Filter { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, PaginatedListModel<HousingDto>>
        {
            private readonly IQueryBuilder<HousingDto, HousingsFilterDto> _queryBuilder;

            public QueryHandler(IQueryBuilder<HousingDto, HousingsFilterDto> queryBuilder)
            {
                _queryBuilder = queryBuilder;
            }

            public async Task<PaginatedListModel<HousingDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _queryBuilder
                    .SetFilter(request.Filter)
                    .Filter()
                    .Sort()
                    .Paginate()
                    .GetQuery();

                var housings = await query.ToListAsync(cancellationToken);

                var result = new PaginatedListModel<HousingDto>(
                    housings,
                    request.Filter.PageNumber,
                    housings.Count,
                    request.Filter.ItemsPerPage);

                return result;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(_ => _.Filter)
                    .NotEmpty();
            }
        }

    }
}