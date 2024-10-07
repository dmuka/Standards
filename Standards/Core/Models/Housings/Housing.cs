using System.ComponentModel.DataAnnotations;
using Standards.Core.Constants;
using Standards.Core.Models.Departments;

namespace Standards.Core.Models.Housings
{
    public class Housing : BaseEntity
    {
        [MaxLength(Lengths.Address)]
        public string Address { get; set; } = null!;
        public int FloorsCount { get; set; }
        public IList<Department> Departments { get; set; } = new List<Department>();
        public IList<Room> Rooms { get; set; } = new List<Room>();
    }
}
