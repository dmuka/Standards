using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Departments;
using Standards.Core.Models.Interfaces;
using Standards.Core.Models.Persons;
using Standards.Core.Models.Services;

namespace Standards.Core.Models.Standards
{
    public class Standard : BaseEntity, IEntity<int>
    {
        public IList<Workplace> Workplaces { get; set; }
        public Person? Responsible { get; set; }
        public IList<Characteristic> Characteristics { get; set; } = new List<Characteristic>();
        public string ImagePath { get; set; } = null!;
        public int VerificationInterval { get; set; }
        public int? CalibrationInterval { get; set; }
        public IList<Service> Services { get; set; } = new List<Service>();
        
        public static string GetCacheKey()
        {
            return Cache.Standards;
        }
    }
}
