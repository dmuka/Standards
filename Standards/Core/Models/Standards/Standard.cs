using System.ComponentModel.DataAnnotations;
using Standards.Core.Models.Departments;
using Standards.Core.Models.Persons;
using Standards.Core.Models.Services;

namespace Standards.Core.Models.Standards
{
    public class Standard
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public WorkPlace WorkPlace { get; set; }
        public int? ResponsibleId { get; set; }
        [Required]
        public Person? Responsible { get; set; }
        public IList<Characteristic> Characteristics { get; set; } = new List<Characteristic>();
        public string ImagePath { get; set; } = null!;
        public int VerificationInterval { get; set; }
        public int CalibrationInterval { get; set; }
        public IList<Service> Services { get; set; } = new List<Service>();
        public string Comments { get; set; } = null!;
    }
}
