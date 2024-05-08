using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Filter.Interfaces;

namespace Standards.Core.CQRS.Housings.Filters
{
    public class ByNameSorting : IQueryBuilderItem<IQueryable<HousingDto>>
    {
        public IQueryable<HousingDto> Execute(IQueryable<HousingDto> query)
        {
            return query.OrderBy(_ => _.Name);
        }
    }
}