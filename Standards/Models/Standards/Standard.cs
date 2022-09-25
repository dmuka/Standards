using System.ComponentModel.DataAnnotations;

namespace Standards.Models.Standards
{
    public class Standard
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public int WorkPlaceId { get; set; }
        [Required]
        public int ResponcibleId { get; set; }
        public IList<Characteristic> Characteristics { get; set; } = new List<Characteristic>();
        public string ImagePath { get; set; } = null!;
        public int VerificationInterval { get; set; }
        public int CalibrationInterval { get; set; }
        public IList<Service> Services { get; set; } = new List<Service>();
        public string Comments { get; set; } = null!;
    }
}
