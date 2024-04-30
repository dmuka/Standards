using Microsoft.Extensions.Logging;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.DTOs.Filters;

namespace Standards.Core.CQRS.Housings.Filters
{
    public class SearchFilter
    {
        private const int MinLengthToSearch = 3;
        private readonly HousingsFilterDto _filterDto;

        public SearchFilter(HousingsFilterDto filterDto)
        {
            _filterDto = filterDto;
        }

        public IQueryable<HousingDto> Execute(IQueryable<HousingDto> input)
        {
            if (!string.IsNullOrEmpty(_filterDto.SearchQuery) && _filterDto.SearchQuery.Length >= MinLengthToSearch)
            {
                input = input.Where(housing => housing.Name.Contains(_filterDto.SearchQuery));
            }

            return input;
        }
    }
}