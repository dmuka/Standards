using System.Linq.Expressions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Filters;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.Filter.Models;
using Standards.Infrastructure.QueryableWrapper.Implementation;
using Standards.Infrastructure.QueryableWrapper.Interface;

namespace Standards.Core.CQRS.Housings;

    public class GetFiltered
    {
        public class Query(FilterDto filter) : IRequest<PaginatedListModel<Housing>>
        {
            public FilterDto Filter { get; set; } = filter;
        }

        public class QueryHandler(IQueryBuilder<Housing> queryBuilder, IQueryableWrapper<Housing> queryableWrapper)
            : IRequestHandler<Query, PaginatedListModel<Housing>>
        {
            
            public async Task<PaginatedListModel<Housing>> Handle(Query request, CancellationToken cancellationToken)
            {
                var filterFunc = (Housing h) =>
                {
                    var result = h.Name.Contains(request.Filter.SearchQuery);
                    
                    return result;
                };
                
                Expression<Func<Housing, bool>> expression = h => filterFunc(h);
                
                var filter = new Filter<Housing>(expression)
                {
                    PageNumber = request.Filter.Page,
                    ItemsPerPage = request.Filter.ItemsPerPage
                };
                
                var sorter = new Sorter<Housing, string>(h => h.Name)
                {
                    PageNumber = request.Filter.Page,
                    ItemsPerPage = request.Filter.ItemsPerPage
                };

                var query = queryBuilder
                    .AddFilter(filter)
                    .AddSorter(sorter)
                    .Filter()
                    .Sort()
                    .Paginate()
                    .GetQuery();

                var housings = await queryableWrapper.ToListAsync(query, cancellationToken);

                PaginatedListModel<Housing> result = null;
                
                if (housings is not null)
                {
                    result = new PaginatedListModel<Housing>(
                        housings,
                        request.Filter.Page,
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

                        filter.RuleFor(_ => _.Page)
                            .GreaterThan(default(int));

                        filter.RuleFor(_ => _.ItemsPerPage)
                            .GreaterThan(default(int));
                    });
            }
        }
    }