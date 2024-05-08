using Standards.Core.Models.DTOs;
using Standards.Core.Models.DTOs.Filters;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.Filter.Models;

namespace Standards.Core.CQRS.Housings.Filters
{
    public class SearchFilter : PaginationItem, IQueryBuilderItem<IQueryable<HousingDto>>
    {
        private const int MinLengthToSearch = 3;
        private readonly HousingsFilterDto _filterDto;

        public SearchFilter(HousingsFilterDto filterDto)
        {
            _filterDto = filterDto;
        }

        public IQueryable<HousingDto> Execute(IQueryable<HousingDto> query)
        {
            if (!string.IsNullOrEmpty(_filterDto.SearchQuery) && _filterDto.SearchQuery.Length >= MinLengthToSearch)
            {
                query = query.Where(housing => housing.Name.Contains(_filterDto.SearchQuery));
            }

            return query;
        }
    }
}