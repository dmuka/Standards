using FluentValidation;
using MediatR;
using Standards.Core.Models;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.DTOs.Filters;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.QueryableWrapper.Interface;

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
            private readonly IQueryableWrapper<HousingDto> _queryableWrapper;

            public QueryHandler(IQueryBuilder<HousingDto, HousingsFilterDto> queryBuilder, IQueryableWrapper<HousingDto> queryableWrapper)
            {
                _queryBuilder = queryBuilder;
                _queryableWrapper = queryableWrapper;
            }

            public async Task<PaginatedListModel<HousingDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _queryBuilder
                    .SetFilter(request.Filter)
                    .Filter()
                    .Sort()
                    .Paginate()
                    .GetQuery();

                var housings = await _queryableWrapper.ToListAsync(query, cancellationToken);

                PaginatedListModel<HousingDto> result = null;

                if (housings is not null)
                {
                    result = new PaginatedListModel<HousingDto>(
                        housings,
                        request.Filter.PageNumber,
                        housings.Count,
                        request.Filter.ItemsPerPage);
                }

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