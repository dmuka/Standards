using System.ComponentModel.DataAnnotations;
using Standards.Core.Constants;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Departments;
using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models.Housings
{
    public class Housing : BaseEntity, IEntity<int>
    {
        [MaxLength(Lengths.Address)]
        public string Address { get; set; } = null!;
        public int FloorsCount { get; set; }
        public IList<Department> Departments { get; set; } = new List<Department>();
        public IList<Room> Rooms { get; set; } = new List<Room>();

        public static string GetCacheKey()
        {
            return Cache.Housings;
        }
    }
}
