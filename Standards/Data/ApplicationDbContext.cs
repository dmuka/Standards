using Microsoft.EntityFrameworkCore;
using Standards.Models;
using Standards.Models.Departments;
using Standards.Models.DTOs;
using Standards.Models.Housings;
using Standards.Models.MetrologyControl;
using Standards.Models.Persons;
using Standards.Models.Services;
using Standards.Models.Standards;
using Standards.Models.Users;

namespace Standards.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<HousingDto> Housings { get; set; }
        public DbSet<Quantity> Quantities { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<WorkPlace> WorkPlaces { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Sector> Sectors { get; set; }

        public DbSet<CalibrationJournal> CalibrationsJournal { get; set; }
        public DbSet<VerificationJournal> VerificationsJournal { get; set; }
        public DbSet<Place> Places { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Position> Positions { get; set; }

        public DbSet<Material> Materials { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceJournal> ServicesJournal { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }

        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Standard> Standards { get; set; }
    }
}