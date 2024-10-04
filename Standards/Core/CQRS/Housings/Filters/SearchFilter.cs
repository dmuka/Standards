using Standards.Core.Models.DTOs;
using Standards.Core.Models.DTOs.Filters;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.Filter.Models;

namespace Standards.Core.CQRS.Housings.Filters
{
    public class SearchFilter(HousingsFilterDto filterDto) : PaginationItem, IQueryBuilderItem<IQueryable<HousingDto>>
    {
        private const int MinLengthToSearch = 3;

        public IQueryable<HousingDto> Execute(IQueryable<HousingDto> query)
        {
            if (!string.IsNullOrEmpty(filterDto.SearchQuery) && filterDto.SearchQuery.Length >= MinLengthToSearch)
            {
                query = query.Where(housing => housing.Name.Contains(filterDto.SearchQuery));
            }

            return query;
        }
    }
}