using System.ComponentModel.DataAnnotations;
using Standards.Core.Constants;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Departments;
using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models.Housings
{
    public class Housing : Entity, ICacheable
    {
        [MaxLength(Lengths.Address)]
        public required string Address { get; set; } = null!;
        public required int FloorsCount { get; set; }
        public IList<Room> Rooms { get; set; } = [];

        public static string GetCacheKey()
        {
            return Cache.Housings;
        }
    }
}
