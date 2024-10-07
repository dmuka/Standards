using Microsoft.EntityFrameworkCore;
using Standards.Core.Models;
using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.Core.Models.MetrologyControl;
using Standards.Core.Models.Persons;
using Standards.Core.Models.Services;
using Standards.Core.Models.Standards;
using Standards.Core.Models.Users;

namespace Standards.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
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
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Standard>()
                .HasOne(s => s.Responsible)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Person>()
                .HasOne(p => p.Sector)
                .WithMany(s => s.Persons)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<WorkPlace>()
                .HasOne(wp => wp.Room)
                .WithMany(room => room.WorkPlaces)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<ServiceJournal>()
                .HasOne(sj => sj.Standard)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Housing)
                .WithMany(h => h.Rooms)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Housing>()
                .ToTable("Housings")
                .HasMany(h => h.Departments)
                .WithMany(d => d.Housings);
        }
    }
}