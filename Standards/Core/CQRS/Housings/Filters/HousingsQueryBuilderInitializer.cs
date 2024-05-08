using Standards.Core.Models.DTOs;
using Standards.Core.Models.DTOs.Filters;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Filter.Interfaces;

namespace Standards.Core.CQRS.Housings.Filters
{
    class HousingsQueryBuilderInitializer : IQueryBuilderInitializer<HousingDto, HousingsFilterDto>
    {
        private readonly IRepository _repository;

        public HousingsQueryBuilderInitializer(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<IQueryBuilderItem<IQueryable<HousingDto>>> GetFilters(HousingsFilterDto filter)
        {
            return new IQueryBuilderItem<IQueryable<HousingDto>>[]
            {
                new SearchFilter(filter)
            };
        }

        public IEnumerable<IQueryBuilderItem<IQueryable<HousingDto>>> GetSortings(HousingsFilterDto filter)
        {
            return new IQueryBuilderItem<IQueryable<HousingDto>>[]
            {
                new ByNameSorting()
            };
        }
    }
}
