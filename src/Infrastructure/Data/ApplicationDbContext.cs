using Domain.Aggregates.Categories;
using Domain.Aggregates.Common.ValueObjects;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Rooms;
using Domain.Aggregates.Sectors;
using Domain.Aggregates.Workplaces;
using Domain.Models;
using Domain.Models.Departments;
using Domain.Models.MetrologyControl;
using Domain.Models.MetrologyControl.Contacts;
using Domain.Models.Persons;
using Domain.Models.Services;
using Domain.Models.Standards;
using Domain.Models.Users;
using Infrastructure.Data.ModelBuilderExtensions;
using Infrastructure.Data.Outbox;
using Microsoft.EntityFrameworkCore;
using Category = Domain.Models.Persons.Category;
using Department = Domain.Models.Departments.Department;
using Housing = Domain.Models.Housings.Housing;
using Person = Domain.Models.Persons.Person;
using Room = Domain.Models.Housings.Room;
using Sector = Domain.Models.Departments.Sector;

namespace Infrastructure.Data;

    public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        
        public DbSet<Quantity> Quantities { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Domain.Aggregates.Rooms.Room> Rooms2 { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Workplace> WorkPlaces { get; set; }

        public DbSet<Housing> Housings { get; set; }
        public DbSet<Domain.Aggregates.Housings.Housing> Housings2 { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Domain.Aggregates.Departments.Department> Departments2 { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Domain.Aggregates.Sectors.Sector> Sectors2 { get; set; }

        public DbSet<CalibrationJournalItem> CalibrationsJournal { get; set; }
        public DbSet<VerificationJournalItem> VerificationsJournal { get; set; }
        
        public DbSet<Place> Places { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<SocialProfileIdType> SocialProfileIdTypes { get; set; }
        public DbSet<SocialInfo> SocialInfos { get; set; }
        public DbSet<Social> Socials { get; set; }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Domain.Aggregates.Persons.Person> Persons2 { get; set; }

        public DbSet<User> Users { get; set; }        
        public DbSet<Domain.Aggregates.Users.User> Users2 { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Domain.Aggregates.Categories.Category> Categories2 { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Domain.Aggregates.Positions.Position> Positions2 { get; set; }

        public DbSet<Material> Materials { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceJournalItem> ServicesJournal { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }

        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Domain.Aggregates.Grades.Grade> Grades2 { get; set; }
        public DbSet<Standard> Standards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<RoomId>();   
            modelBuilder.Ignore<FloorId>();
            modelBuilder.Ignore<HousingId>();
            modelBuilder.Ignore<SectorId>();
            modelBuilder.Ignore<DepartmentId>();
            modelBuilder.Ignore<WorkplaceId>();
            modelBuilder.Ignore<PersonId>();
            modelBuilder.Ignore<CategoryId>();
            modelBuilder.Ignore<Name>();
            modelBuilder.Ignore<ShortName>();
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Seeding data
            object[] housings =
            [
                new { Id = 1, Name = "Housing1", ShortName = "h1", FloorsCount = 2, Address = "Address1", Comments = "Comments1" },
                new { Id = 2, Name = "Housing2", ShortName = "h2", FloorsCount = 1, Address = "Address2", Comments = "Comments2" },
                new { Id = 3, Name = "Housing3", ShortName = "h3", FloorsCount = 2, Address = "Address3", Comments = "Comments3" }
            ];
            modelBuilder.Seed<Housing>(housings);
            object[] housings2 =
            [
                new { Id = new HousingId(Guid.Parse("3daa72ed-655d-4672-a6da-8c5977f900a2")), HousingName = HousingName.Create("Housing1").Value, HousingShortName = HousingShortName.Create("h1").Value, FloorsCount = 2, Address = Address.Create("Housing1 Address1").Value, Comments = "Comments1" },
                new { Id = new HousingId(Guid.Parse("e7382e43-067d-4e64-a216-21e008d9f392")), HousingName = HousingName.Create("Housing2").Value, HousingShortName = HousingShortName.Create("h2").Value, FloorsCount = 1, Address = Address.Create("Housing2 Address2").Value, Comments = "Comments2" },
                new { Id = new HousingId(Guid.Parse("45697111-13c0-4e81-ae6d-fbfeab3453f9")), HousingName = HousingName.Create("Housing3").Value, HousingShortName = HousingShortName.Create("h3").Value, FloorsCount = 2, Address = Address.Create("Housing3 Address3").Value, Comments = "Comments3" }
            ];
            modelBuilder.SeedAggregates<Domain.Aggregates.Housings.Housing>(housings2);
            
            object[] floors =
            [
                new { Id = new FloorId(Guid.Parse("1374cbd5-6f40-429f-a120-f8434e7b62c7")), Number = 2, HousingId = ((dynamic)housings2[0]).Id, Comments = "Comments1" },
                new { Id = new FloorId(Guid.Parse("a0126888-2e8b-40dd-8ab9-54bd9b80b41f")), Number = 1, HousingId = ((dynamic)housings2[1]).Id, Comments = "Comments2" },
                new { Id = new FloorId(Guid.Parse("736b2459-8a42-4417-a4df-f7ff93991c1f")), Number = 2, HousingId = ((dynamic)housings2[2]).Id, Comments = "Comments3" }
            ];
            modelBuilder.SeedAggregates<Floor>(floors);

            object[] departments =
            [
                new { Id = 1, Name = "Department1", ShortName = "d1", Comments = "Comments1" },
                new { Id = 2, Name = "Department2", ShortName = "d2", Comments = "Comments2" },
                new { Id = 3, Name = "Department3", ShortName = "d3", Comments = "Comments3" }
            ];
            modelBuilder.Seed<Department>(departments);

            object[] sectors =
            [
                new { Id = 1, Name = "Sector1", ShortName = "s1", Comments = "Comments1", DepartmentId = 1 },
                new { Id = 2, Name = "Sector2", ShortName = "s2", Comments = "Comments2", DepartmentId = 1 },
                new { Id = 3, Name = "Sector3", ShortName = "s3", Comments = "Comments3", DepartmentId = 1 },
                new { Id = 4, Name = "Sector4", ShortName = "s4", Comments = "Comments4", DepartmentId = 2 },
                new { Id = 5, Name = "Sector5", ShortName = "s5", Comments = "Comments5", DepartmentId = 2 },
                new { Id = 6, Name = "Sector6", ShortName = "s6", Comments = "Comments6", DepartmentId = 3 },
                new { Id = 7, Name = "Sector7", ShortName = "s7", Comments = "Comments7", DepartmentId = 3 }
            ];
            modelBuilder.Seed<Sector>(sectors);

            object[] rooms =
            [
                new { Id = 1, Name = "Room1", ShortName = "r1", Floor = 2, HousingId = 1, SectorId = 1, Length = 4.0, Height = 3.0, Width = 5.0, Comments = "Comments1" },
                new { Id = 2, Name = "Room2", ShortName = "r2", Floor = 1, HousingId = 2, SectorId = 1, Length = 5.0, Height = 3.0, Width = 5.0, Comments = "Comments2" },
                new { Id = 3, Name = "Room3", ShortName = "r3", Floor = 2, HousingId = 3, SectorId = 3, Length = 6.0, Height = 3.0, Width = 5.0, Comments = "Comments3" },
                new { Id = 4, Name = "Room4", ShortName = "r4", Floor = 1, HousingId = 1, SectorId = 2, Length = 5.0, Height = 3.0, Width = 5.0, Comments = "Comments4" },
                new { Id = 5, Name = "Room5", ShortName = "r5", Floor = 2, HousingId = 3, SectorId = 3, Length = 6.0, Height = 3.0, Width = 5.0, Comments = "Comments5" },
                new { Id = 6, Name = "Room6", ShortName = "r6", Floor = 1, HousingId = 3, SectorId = 2, Length = 3.0, Height = 3.0, Width = 4.0, Comments = "Comments6" },
                new { Id = 7, Name = "Room7", ShortName = "r7", Floor = 2, HousingId = 1, SectorId = 4, Length = 6.0, Height = 3.0, Width = 5.0, Comments = "Comments7" },
                new { Id = 8, Name = "Room8", ShortName = "r8", Floor = 1, HousingId = 2, SectorId = 5, Length = 3.0, Height = 3.0, Width = 4.0, Comments = "Comments8" },
                new { Id = 9, Name = "Room9", ShortName = "r9", Floor = 1, HousingId = 2, SectorId = 4, Length = 6.0, Height = 3.0, Width = 5.0, Comments = "Comments9" },
                new { Id = 10, Name = "Room10", ShortName = "r10", Floor = 1, HousingId = 2, SectorId = 5, Length = 3.0, Height = 3.0, Width = 4.0, Comments = "Comments10" },
                new { Id = 11, Name = "Room11", ShortName = "r11", Floor = 2, HousingId = 3, SectorId = 6, Length = 6.0, Height = 3.0, Width = 7.0, Comments = "Comments11" },
                new { Id = 12, Name = "Room12", ShortName = "r12", Floor = 2, HousingId = 3, SectorId = 7, Length = 6.0, Height = 3.0, Width = 5.0, Comments = "Comments12" }
            ];
            modelBuilder.Seed<Room>(rooms);

            object[] categories =
            [
                new { Id = 1, Name = "Без категории", ShortName = "", Comments = "Comments1" },
                new { Id = 2, Name = "Вторая квалификационная категория", ShortName = "2 категория", Comments = "Comments1" },
                new { Id = 3, Name = "Первая квалификационная категория", ShortName = "1 категория", Comments = "Comments1" },
                new { Id = 4, Name = "Ведущий", ShortName = "Ведущий", Comments = "Comments1" }
            ];
            modelBuilder.Seed<Category>(categories);

            object[] positions =
            [
                new { Id = 1, Name = "Техник", ShortName = "Техник", Comments = "Comments1" },
                new { Id = 2, Name = "Инженер", ShortName = "Инженер", Comments = "Comments1" },
                new { Id = 3, Name = "Начальник сектора", ShortName = "Нач. сектора", Comments = "Comments1" },
                new { Id = 4, Name = "Начальник отдела", ShortName = "Нач. отдела", Comments = "Comments1" }
            ];
            modelBuilder.Seed<Position>(positions);

            object[] grades =
            [
                new { Id = 1, Name = "Первый разряд", ShortName = "I", Comments = "Comments1" },
                new { Id = 2, Name = "Второй разряд", ShortName = "II", Comments = "Comments1" },
                new { Id = 3, Name = "Третий разряд", ShortName = "III", Comments = "Comments1" }
            ];
            modelBuilder.Seed<Grade>(grades);

            object[] persons =
            [
                new {
                    Id = 1, FirstName = "Антон", MiddleName = "Сергеевич", LastName = "Чехов", BirthdayDate = new DateTime(2000, 10, 11), CategoryId = 2, PositionId = 2, SectorId = 1, UserId = 1,
                    Role = Role.Engineer, Comments = "Comments1", RoomId = 1
                },
                new {
                    Id = 2, FirstName = "Дмитрий", MiddleName = "Анатольевич", LastName = "Тургенев", BirthdayDate = new DateTime(2001, 04, 01), CategoryId = 3, PositionId = 2, SectorId = 1, UserId = 2,
                    Role = Role.Engineer, Comments = "Comments1", RoomId = 2
                },
                new {
                    Id = 3, FirstName = "Сергей", MiddleName = "Романович", LastName = "Толстой", BirthdayDate = new DateTime(1999, 11, 05), CategoryId = 4, PositionId = 3, SectorId = 3, UserId = 3,
                    Role = Role.SectorHead, Comments = "Comments1", RoomId = 3
                },
                new {
                    Id = 4, FirstName = "Петр", MiddleName = "Артемович", LastName = "Достоевский", BirthdayDate = new DateTime(1998, 06, 06), CategoryId = 3, PositionId = 4, SectorId = 2, UserId = 4,
                    Role = Role.DepartmentHead, Comments = "Comments1", RoomId = 4
                },
                new {
                    Id = 5, FirstName = "Иван", MiddleName = "Никодимович", LastName = "Пушкин", BirthdayDate = new DateTime(2002, 01, 02), CategoryId = 4, PositionId = 2, SectorId = 3, UserId = 5,
                    Role = Role.Engineer, Comments = "Comments1", RoomId = 5
                }
            ];
            modelBuilder.Seed<Person>(persons);
            
            object[] users =
            [
                new { Id = 1, UserName = "user1", Email = "user1@email.com", AccessFailedCount = 0, IsEmailConfirmed = false, IsLockOutEnabled = false, IsTwoFactorEnabled = false },
                new { Id = 2, UserName = "user2", Email = "user2@email.com", AccessFailedCount = 0, IsEmailConfirmed = false, IsLockOutEnabled = false, IsTwoFactorEnabled = false },
                new { Id = 3, UserName = "user3", Email = "user3@email.com", AccessFailedCount = 0, IsEmailConfirmed = false, IsLockOutEnabled = false, IsTwoFactorEnabled = false },
                new { Id = 4, UserName = "user4", Email = "user4@email.com", AccessFailedCount = 0, IsEmailConfirmed = false, IsLockOutEnabled = false, IsTwoFactorEnabled = false},
                new { Id = 5, UserName = "user5", Email = "user5@email.com", AccessFailedCount = 0, IsEmailConfirmed = false, IsLockOutEnabled = false, IsTwoFactorEnabled = false }
            ];
            modelBuilder.Seed<User>(users);

            object[] standards =
            [
                new { Id = 1, Name = "Грузопоршневой манометр", ShortName = "", VerificationInterval = 24, ResponsibleId = 1 },
                new { Id = 2, Name = "Амперметр", ShortName = "", VerificationInterval = 12, ResponsibleId = 2 },
                new { Id = 3, Name = "Термогигрометр", ShortName = "", VerificationInterval = 12, ResponsibleId = 3 },
                new { Id = 4, Name = "Весы", ShortName = "", VerificationInterval = 12, ResponsibleId = 4 },
                new { Id = 5, Name = "Весы лабораторные электронные", ShortName = "Весы лабораторные", VerificationInterval = 12, ResponsibleId = 5 }
            ];
            modelBuilder.Seed<Standard>(standards);

            object[] emails =
            [
                new { Id = 1, Value = "email1@domain.com", ContactId = 1 }, 
                new { Id = 2, Value = "email2@domain.com", ContactId = 2 },
                new { Id = 3, Value = "email3@domain.com", ContactId = 3 },
                new { Id = 4, Value = "email4@domain.com", ContactId = 1 }
            ];
            modelBuilder.Seed<Email>(emails);

            object[] phones =
            [
                new { Id = 1, Value = "375296666666", ContactId = 1 },
                new { Id = 2, Value = "375447777777", ContactId = 2 },
                new { Id = 3, Value = "375335555555", ContactId = 3 },
                new { Id = 4, Value = "375292222222", ContactId = 1 }
            ];
            modelBuilder.Seed<Phone>(phones);

            object[] socialProfileIdTypes =
            [
                new { Id = 1, Value = "PhoneNumber" },
                new { Id = 2, Value = "Email" },
                new { Id = 3, Value = "UserName" }
            ];
            modelBuilder.Seed<SocialProfileIdType>(socialProfileIdTypes);

            object[] socialInfos =            
            [
                new { Id = 1, Name = "Instagram",  ShortName = "I", BasePath = "https://www.instagram.com/", ProfileIdTypeId = 3 },
                new { Id = 2, Name = "Twitter",  ShortName = "X", BasePath = "https://twitter.com/", ProfileIdTypeId = 3 },
                new { Id = 3, Name = "Facebook Messenger",  ShortName = "Messenger", BasePath = "https://m.me/", ProfileIdTypeId = 3 },
                new { Id = 4, Name = "LinkedIn",  ShortName = "LI", BasePath = "https://www.linkedin.com/in/", ProfileIdTypeId = 3 },
                new { Id = 5, Name = "Youtube",  ShortName = "Y", BasePath = "https://www.youtube.com/", ProfileIdTypeId = 3 },
                new { Id = 6, Name = "TikTok",  ShortName = "T", BasePath = "https://www.tiktok.com/", ProfileIdTypeId = 3 },
                new { Id = 7, Name = "Reddit",  ShortName = "R", BasePath = "https://www.reddit.com/user/", ProfileIdTypeId = 3 },
                new { Id = 8, Name = "GitHub",  ShortName = "G", BasePath = "https://github.com/", ProfileIdTypeId = 3 },
                new { Id = 9, Name = "SnapChat",  ShortName = "S", BasePath = "https://www.snapchat.com/add/", ProfileIdTypeId = 3 },
                new { Id = 10, Name = "WhatsUp",  ShortName = "W", BasePath = "https://wa.me/", ProfileIdTypeId = 1 },
                new { Id = 11, Name = "Telegram",  ShortName = "Tlg", BasePath = "https://t.me/", ProfileIdTypeId = 3 },
                new { Id = 12, Name = "Viber",  ShortName = "V", BasePath = "viber://chat?", ProfileIdTypeId = 1 }
            ];
            modelBuilder.Seed<SocialInfo>(socialInfos);

            object[] socials =
            [
                new { Id = 1, SocialInfoId = 1,  SocialIdValue = "UserName1I", ContactId = 1 },
                new { Id = 2, SocialInfoId = 11,  SocialIdValue = "UserName1Tlg", ContactId = 1 },
                new { Id = 3, SocialInfoId = 10,  SocialIdValue = "375441111111", ContactId = 1 },
                new { Id = 4, SocialInfoId = 2,  SocialIdValue = "UserName2T", ContactId = 2 },
                new { Id = 5, SocialInfoId = 8,  SocialIdValue = "UserName1G", ContactId = 2 },
                new { Id = 6, SocialInfoId = 12,  SocialIdValue = "375442222222", ContactId = 2 },
                new { Id = 7, SocialInfoId = 4,  SocialIdValue = "UserName3LI", ContactId = 3 },
                new { Id = 8, SocialInfoId = 3,  SocialIdValue = "UserName3F", ContactId = 3 },
                new { Id = 9, SocialInfoId = 10,  SocialIdValue = "375441111111", ContactId = 3 },
            ];
            modelBuilder.Seed<Social>(socials);

            object[] places =
            [
                new { Id = 1, Name = "PlaceName1", ShortName = "PlaceShortName1", Address = "PlaceAddress1" },
                new { Id = 2, Name = "PlaceName2", ShortName = "PlaceShortName2", Address = "PlaceAddress2" },
                new { Id = 3, Name = "PlaceName3", ShortName = "PlaceShortName3", Address = "PlaceAddress3" },
                new { Id = 4, Name = "PlaceName4", ShortName = "PlaceShortName4", Address = "PlaceAddress4" }
            ];
            modelBuilder.Seed<Place>(places);

            object[] contacts =
            [
                new { Id = 1, Name = "ContactName1", ShortName = "ContactShortName1", PlaceId = 1 },
                new { Id = 2, Name = "ContactName2", ShortName = "ContactShortName2", PlaceId = 2 },
                new { Id = 3, Name = "ContactName3", ShortName = "ContactShortName3", PlaceId = 3 }
            ];
            modelBuilder.Seed<Contact>(contacts);
        }
    }