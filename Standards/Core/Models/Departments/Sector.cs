using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Housings;
using Standards.Core.Models.Interfaces;
using Standards.Core.Models.Persons;

namespace Standards.Core.Models.Departments
{
    public class Sector : BaseEntity, IEntity<int>
    {
        public Department Department { get; set; }
        public IList<Room> Rooms { get; set; } = new List<Room>();
        public IList<Workplace> Workplaces { get; set; } = new List<Workplace>();
        public IList<Person> Persons { get; set; } = new List<Person>();

        public static string GetCacheKey()
        {
            return Cache.Sectors;
        }
    }
}
