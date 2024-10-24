using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models.Persons
{
    public class Category : BaseEntity, IEntity<int>
    {
        public static string GetCacheKey()
        {
            return Cache.Categories;
        }
    }
}
