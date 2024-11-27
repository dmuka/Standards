using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Housings;
using Standards.Core.Models.Interfaces;
using Standards.Core.Models.Persons;

namespace Standards.Core.Models.Departments
{
    public class Sector : Entity, ICacheable
    {
        public required Department Department { get; set; }
        public IList<Room> Rooms { get; set; } = [];
        public IList<Workplace> Workplaces { get; set; } = [];
        public IList<Person> Persons { get; set; } = [];

        public static string GetCacheKey()
        {
            return Cache.Sectors;
        }
    }
}
