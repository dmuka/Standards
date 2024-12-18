using Domain.Models;
using Domain.Models.Departments;
using Domain.Models.Housings;
using Domain.Models.MetrologyControl;
using Domain.Models.Persons;
using Domain.Models.Services;
using Domain.Models.Standards;
using Domain.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Quantity> Quantities { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Workplace> WorkPlaces { get; set; }

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
        public DbSet<ServiceJournalItem> ServicesJournal { get; set; }
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

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Category)
                .WithMany();

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Position)
                .WithMany();

            modelBuilder.Entity<Person>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<Person>(p => p.UserId)
                .HasPrincipalKey<User>(u => u.Id);

            modelBuilder.Entity<Workplace>()
                .HasOne(wp => wp.Room)
                .WithMany(room => room.WorkPlaces)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ServiceJournalItem>()
                .ToTable("ServiceJournal")
                .HasOne(sj => sj.Standard)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Housing)
                .WithMany(h => h.Rooms)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Sector)
                .WithMany(s => s.Rooms)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Persons)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            // modelBuilder.Entity<Housing>()
            //     .ToTable("Housings")
            //     .HasMany(h => h.Departments)
            //     .WithMany(d => d.Housings)
            //     .UsingEntity(d => d.HasData(
            //     [
            //         new { DepartmentsId = 1, HousingsId = 1 },
            //         new { DepartmentsId = 2, HousingsId = 1 },
            //         new { DepartmentsId = 2, HousingsId = 2 },
            //         new { DepartmentsId = 1, HousingsId = 3 },
            //         new { DepartmentsId = 3, HousingsId = 3 }
            //     ]));

            modelBuilder.Entity<Sector>()
                .HasMany(s => s.Workplaces)
                .WithOne();

            // Seeding data
            var housing1 = new Housing
            {
                Id = 1, Name = "Housing1", ShortName = "h1", FloorsCount = 2, Address = "Address1",
                Comments = "Comments1"
            };
            var housing2 = new Housing
            {
                Id = 2, Name = "Housing2", ShortName = "h2", FloorsCount = 1, Address = "Address2",
                Comments = "Comments2"
            };
            var housing3 = new Housing
            {
                Id = 3, Name = "Housing3", ShortName = "h3", FloorsCount = 2, Address = "Address3",
                Comments = "Comments3"
            };

            modelBuilder.Entity<Housing>().HasData(housing1, housing2, housing3);

            var department1 = new Department { Id = 1, Name = "Department1", ShortName = "d1", Comments = "Comments1" };
            var department2 = new Department { Id = 2, Name = "Department2", ShortName = "d2", Comments = "Comments2" };
            var department3 = new Department { Id = 3, Name = "Department3", ShortName = "d3", Comments = "Comments3" };

            modelBuilder.Entity<Department>().HasData(department1, department2, department3);

            var sector1 = new { Id = 1, Name = "Sector1", ShortName = "s1", Comments = "Comments1", DepartmentId = 1 };
            var sector2 = new { Id = 2, Name = "Sector2", ShortName = "s2", Comments = "Comments2", DepartmentId = 1 };
            var sector3 = new { Id = 3, Name = "Sector3", ShortName = "s3", Comments = "Comments3", DepartmentId = 1 };
            var sector4 = new { Id = 4, Name = "Sector4", ShortName = "s4", Comments = "Comments4", DepartmentId = 2 };
            var sector5 = new { Id = 5, Name = "Sector5", ShortName = "s5", Comments = "Comments5", DepartmentId = 2 };
            var sector6 = new { Id = 6, Name = "Sector6", ShortName = "s6", Comments = "Comments6", DepartmentId = 3 };
            var sector7 = new { Id = 7, Name = "Sector7", ShortName = "s7", Comments = "Comments7", DepartmentId = 3 };

            modelBuilder.Entity<Sector>().HasData(sector1, sector2, sector3, sector4, sector5, sector6, sector7);

            modelBuilder.Entity<Room>().HasData(
                new
                {
                    Id = 1, Name = "Room1", ShortName = "r1", Floor = 2, HousingId = 1, SectorId = 1, Length = 4.0,
                    Height = 3.0, Width = 5.0, Comments = "Comments1"
                },
                new
                {
                    Id = 2, Name = "Room2", ShortName = "r2", Floor = 1, HousingId = 2, SectorId = 1, Length = 5.0,
                    Height = 3.0, Width = 5.0, Comments = "Comments2"
                },
                new
                {
                    Id = 3, Name = "Room3", ShortName = "r3", Floor = 2, HousingId = 3, SectorId = 3, Length = 6.0,
                    Height = 3.0, Width = 5.0, Comments = "Comments3"
                },
                new
                {
                    Id = 4, Name = "Room4", ShortName = "r4", Floor = 1, HousingId = 1, SectorId = 2, Length = 5.0,
                    Height = 3.0, Width = 5.0, Comments = "Comments4"
                },
                new
                {
                    Id = 5, Name = "Room5", ShortName = "r5", Floor = 2, HousingId = 3, SectorId = 3, Length = 6.0,
                    Height = 3.0, Width = 5.0, Comments = "Comments5"
                },
                new
                {
                    Id = 6, Name = "Room6", ShortName = "r6", Floor = 1, HousingId = 3, SectorId = 2, Length = 3.0,
                    Height = 3.0, Width = 4.0, Comments = "Comments6"
                },
                new
                {
                    Id = 7, Name = "Room7", ShortName = "r7", Floor = 2, HousingId = 1, SectorId = 4, Length = 6.0,
                    Height = 3.0, Width = 5.0, Comments = "Comments7"
                },
                new
                {
                    Id = 8, Name = "Room8", ShortName = "r8", Floor = 1, HousingId = 2, SectorId = 5, Length = 3.0,
                    Height = 3.0, Width = 4.0, Comments = "Comments8"
                },
                new
                {
                    Id = 9, Name = "Room9", ShortName = "r9", Floor = 1, HousingId = 2, SectorId = 4, Length = 6.0,
                    Height = 3.0, Width = 5.0, Comments = "Comments9"
                },
                new
                {
                    Id = 10, Name = "Room10", ShortName = "r10", Floor = 1, HousingId = 2, SectorId = 5, Length = 3.0,
                    Height = 3.0, Width = 4.0, Comments = "Comments10"
                },
                new
                {
                    Id = 11, Name = "Room11", ShortName = "r11", Floor = 2, HousingId = 3, SectorId = 6, Length = 6.0,
                    Height = 3.0, Width = 7.0, Comments = "Comments11"
                },
                new
                {
                    Id = 12, Name = "Room12", ShortName = "r12", Floor = 2, HousingId = 3, SectorId = 7, Length = 6.0,
                    Height = 3.0, Width = 5.0, Comments = "Comments12"
                });

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Без категории", ShortName = "", Comments = "Comments1" },
                new Category
                {
                    Id = 2, Name = "Вторая квалификационная категория", ShortName = "2 категория",
                    Comments = "Comments1"
                },
                new Category
                {
                    Id = 3, Name = "Первая квалификационная категория", ShortName = "1 категория",
                    Comments = "Comments1"
                },
                new Category { Id = 4, Name = "Ведущий", ShortName = "Ведущий", Comments = "Comments1" });

            modelBuilder.Entity<Position>().HasData(
                new Category { Id = 1, Name = "Техник", ShortName = "Техник", Comments = "Comments1" },
                new Category { Id = 2, Name = "Инженер", ShortName = "Инженер", Comments = "Comments1" },
                new Category { Id = 3, Name = "Начальник сектора", ShortName = "Нач. сектора", Comments = "Comments1" },
                new Category { Id = 4, Name = "Начальник отдела", ShortName = "Нач. отдела", Comments = "Comments1" });

            modelBuilder.Entity<Person>().HasData(
                new
                {
                    Id = 1, FirstName = "Антон", MiddleName = "Сергеевич", LastName = "Чехов",
                    BirthdayDate = new DateTime(2000, 10, 11), CategoryId = 2, PositionId = 2, SectorId = 1, UserId = 1,
                    Role = Role.Engineer, Comments = "Comments1", RoomId = 1
                },

                new
                {
                    Id = 2, FirstName = "Дмитрий", MiddleName = "Анатольевич", LastName = "Тургенев",
                    BirthdayDate = new DateTime(2001, 04, 01), CategoryId = 3, PositionId = 2, SectorId = 1, UserId = 2,
                    Role = Role.Engineer, Comments = "Comments1", RoomId = 2
                },
                new
                {
                    Id = 3, FirstName = "Сергей", MiddleName = "Романович", LastName = "Толстой",
                    BirthdayDate = new DateTime(1999, 11, 05), CategoryId = 4, PositionId = 3, SectorId = 3, UserId = 3,
                    Role = Role.SectorHead, Comments = "Comments1", RoomId = 3
                },
                new
                {
                    Id = 4, FirstName = "Петр", MiddleName = "Артемович", LastName = "Достоевский",
                    BirthdayDate = new DateTime(1998, 06, 06), CategoryId = 3, PositionId = 4, SectorId = 2, UserId = 4,
                    Role = Role.DepartmentHead, Comments = "Comments1", RoomId = 4
                },
                new
                {
                    Id = 5, FirstName = "Иван", MiddleName = "Никодимович", LastName = "Пушкин",
                    BirthdayDate = new DateTime(2002, 01, 02), CategoryId = 4, PositionId = 2, SectorId = 3, UserId = 5,
                    Role = Role.Engineer, Comments = "Comments1", RoomId = 5
                });
            
             modelBuilder.Entity<User>().HasData(
                 new User { Id = 1, UserName = "user1", Email = "user1@email.com" },
                 new User { Id = 2, UserName = "user2", Email = "user2@email.com" },
                 new User { Id = 3, UserName = "user3", Email = "user3@email.com" },
                 new User { Id = 4, UserName = "user4", Email = "user4@email.com" },
                 new User { Id = 5, UserName = "user5", Email = "user5@email.com" });               
        }
    }