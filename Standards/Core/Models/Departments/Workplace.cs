using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Housings;
using Standards.Core.Models.Interfaces;
using Standards.Core.Models.Persons;

namespace Standards.Core.Models.Departments
{
    public class Workplace : BaseEntity, IEntity<int>
    {
        public Room Room { get; set; }
        public Person Responsible { get; set; }
        public string? ImagePath { get; set; }

        public static string GetCacheKey()
        {
            return Cache.Workplaces;
        }
    }
}
