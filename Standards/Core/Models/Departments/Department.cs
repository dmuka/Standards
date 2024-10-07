using Standards.Core.Models.Housings;

namespace Standards.Core.Models.Departments
{
    public class Department : BaseEntity
    {
        public IList<Sector> Sectors { get; set; } = new List<Sector>();
        public IList<Housing> Housings { get; set; } = new List<Housing>();
    }
}
