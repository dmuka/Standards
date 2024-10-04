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
        public class Query(HousingsFilterDto filter) : IRequest<PaginatedListModel<HousingDto>>
        {
            public HousingsFilterDto Filter { get; set; } = filter;
        }

        public class QueryHandler(
            IQueryBuilder<HousingDto, HousingsFilterDto> queryBuilder,
            IQueryableWrapper<HousingDto> queryableWrapper)
            : IRequestHandler<Query, PaginatedListModel<HousingDto>>
        {
            public async Task<PaginatedListModel<HousingDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = queryBuilder
                    .SetFilter(request.Filter)
                    .Filter()
                    .Sort()
                    .Paginate()
                    .GetQuery();

                var housings = await queryableWrapper.ToListAsync(query, cancellationToken);

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