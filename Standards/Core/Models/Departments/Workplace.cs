using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Housings;
using Standards.Core.Models.Interfaces;
using Standards.Core.Models.Persons;

namespace Standards.Core.Models.Departments
{
    public class Workplace : Entity, ICacheable
    {
        public required Room Room { get; set; }
        public required Person Responsible { get; set; }
        public string? ImagePath { get; set; }

        public static string GetCacheKey()
        {
            return Cache.Workplaces;
        }
    }
}
