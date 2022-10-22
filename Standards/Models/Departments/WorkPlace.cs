using Standards.Models.Persons;

namespace Standards.Models.Departments
{
    public class WorkPlace
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public Person Responcible { get; set; } = new Person();
        public string ImagePath { get; set; } = null!;
        public string Comments { get; set; } = null!;
    }
}
