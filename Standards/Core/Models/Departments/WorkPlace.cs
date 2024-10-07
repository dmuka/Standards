using Standards.Core.Models.Housings;
using Standards.Core.Models.Persons;

namespace Standards.Core.Models.Departments
{
    public class WorkPlace : BaseEntity
    {
        public Room Room { get; set; }
        public Person Responcible { get; set; }
        public string? ImagePath { get; set; }
    }
}
