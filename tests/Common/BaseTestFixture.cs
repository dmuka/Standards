using Domain.Models;
using Domain.Models.Departments;
using Domain.Models.DTOs;
using Domain.Models.Housings;
using Domain.Models.MetrologyControl;
using Domain.Models.Persons;
using Domain.Models.Services;
using Domain.Models.Standards;
using Domain.Models.Users;
using Tests.Common.Constants;

namespace Tests.Common;

public abstract class BaseTestFixture
{
    protected static List<Room> Rooms { get; private set; } = [];
    protected static List<RoomDto> RoomDtos { get; private set; } = [];
    
    protected static List<Sector> Sectors { get; private set; } = [];
    protected static List<SectorDto> SectorDtos { get; private set; } = [];
    
    protected static List<Category> Categories { get; private set; } = [];
    
    protected static List<Position> Positions { get; private set; } = [];
    
    protected static List<User> Users { get; private set; } = [];
    
    protected static List<Person> Persons { get; private set; } = [];
    protected static List<PersonDto> PersonDtos { get; private set; } = [];
    
    protected static List<string> Roles { get; private set; } = [];
    
    protected static List<Workplace> Workplaces { get; private set; } = [];
    protected static List<WorkplaceDto> WorkplaceDtos { get; private set; } = [];
    
    protected static List<Department> Departments { get; private set; } = [];
    protected static List<DepartmentDto> DepartmentDtos { get; private set; } = [];
    
    protected static List<Grade> Grades { get; private set; } = [];
    
    protected static List<Quantity> Quantities { get; private set; } = [];
    protected static List<QuantityDto> QuantityDtos { get; private set; } = [];
    
    protected static List<Unit> Units { get; private set; } = [];
    protected static List<UnitDto> UnitDtos { get; private set; } = [];
    
    protected static List<Material> Materials { get; private set; } = [];
    protected static List<MaterialDto> MaterialDtos { get; private set; } = [];
    
    protected static List<Characteristic> Characteristics { get; private set; } = [];
    protected static List<CharacteristicDto> CharacteristicsDtos { get; private set; } = [];
    
    protected static List<ServiceType> ServiceTypes { get; private set; } = [];
    
    protected static List<Service> Services { get; private set; } = [];
    protected static List<ServiceDto> ServiceDtos { get; private set; } = [];
    
    protected static List<Standard> Standards { get; private set; } = [];
    protected static List<StandardDto> StandardDtos { get; private set; } = [];
    
    protected static List<ServiceJournalItem> ServiceJournalItems { get; private set; } = [];
    protected static List<ServiceJournalItemDto> ServiceJournalItemDtos { get; private set; } = [];
    
    protected static List<VerificationJournalItem> VerificationJournalItems { get; private set; } = [];
    protected static List<VerificationJournalItemDto> VerificationJournalItemDtos { get; private set; } = [];
    
    protected static List<Place> Places { get; private set; } = [];

    protected static IList<Housing> Housings { get; private set; } = [];
    protected static List<HousingDto> HousingDtos { get; private set; } = [];

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        #region Users
        Users =
        [
            new User
            {
                Id = 1,
                UserName = "UserName1",
                Email = "email1@email.com",
                IsEmailConfirmed = true,
                IsTwoFactorEnabled = true,
                AccessToken = "AccessToken1",
                RefreshToken = "RefreshToken1",
                AccessFailedCount = default,
                PasswordSalt = [new byte()],
                PasswordHash = [new byte()],
                LockOutEnd = null,
                IsLockOutEnabled = false
            },
            new User
            {
                Id = 2,
                UserName = "UserName2",
                Email = "email2@email.com",
                IsEmailConfirmed = true,
                IsTwoFactorEnabled = true,
                AccessToken = "AccessToken2",
                RefreshToken = "RefreshToken2",
                AccessFailedCount = default,
                PasswordSalt = [new byte()],
                PasswordHash = [new byte()],
                LockOutEnd = null,
                IsLockOutEnabled = false
            },
            new User
            {
                Id = 3,
                UserName = "UserName3",
                Email = "email3@email.com",
                IsEmailConfirmed = true,
                IsTwoFactorEnabled = true,
                AccessToken = "AccessToken3",
                RefreshToken = "RefreshToken3",
                AccessFailedCount = default,
                PasswordSalt = [new byte()],
                PasswordHash = [new byte()],
                LockOutEnd = null,
                IsLockOutEnabled = false
            },
            new User
            {
                Id = 4,
                UserName = "UserName4",
                Email = "email4@email.com",
                IsEmailConfirmed = true,
                IsTwoFactorEnabled = true,
                AccessToken = "AccessToken4",
                RefreshToken = "RefreshToken4",
                AccessFailedCount = default,
                PasswordSalt = [new byte()],
                PasswordHash = [new byte()],
                LockOutEnd = null,
                IsLockOutEnabled = false
            },
            new User
            {
                Id = 5,
                UserName = "UserName5",
                Email = "email5@email.com",
                IsEmailConfirmed = true,
                IsTwoFactorEnabled = true,
                AccessToken = "AccessToken5",
                RefreshToken = "RefreshToken5",
                AccessFailedCount = default,
                PasswordSalt = [new byte()],
                PasswordHash = [new byte()],
                LockOutEnd = null,
                IsLockOutEnabled = false
            },
            new User
            {
                Id = 6,
                UserName = "UserName6",
                Email = "email6@email.com",
                IsEmailConfirmed = true,
                IsTwoFactorEnabled = true,
                AccessToken = "AccessToken6",
                RefreshToken = "RefreshToken6",
                AccessFailedCount = default,
                PasswordSalt = [new byte()],
                PasswordHash = [new byte()],
                LockOutEnd = null,
                IsLockOutEnabled = false
            },
            new User
            {
                Id = 7,
                UserName = "UserName7",
                Email = "email7@email.com",
                IsEmailConfirmed = true,
                IsTwoFactorEnabled = true,
                AccessToken = "AccessToken7",
                RefreshToken = "RefreshToken7",
                AccessFailedCount = default,
                PasswordSalt = [new byte()],
                PasswordHash = [new byte()],
                LockOutEnd = null,
                IsLockOutEnabled = false
            },
            new User
            {
                Id = 8,
                UserName = "UserName8",
                Email = "email8@email.com",
                IsEmailConfirmed = true,
                IsTwoFactorEnabled = true,
                AccessToken = "AccessToken8",
                RefreshToken = "RefreshToken8",
                AccessFailedCount = default,
                PasswordSalt = [new byte()],
                PasswordHash = [new byte()],
                LockOutEnd = null,
                IsLockOutEnabled = false
            }
        ];
        #endregion
        
        #region Roles
        Roles =
        [
            "Admin",
            "HeadOfDepartment",
            "HeadOdSector",
            "Engineer",
            "Technician"
        ];
        #endregion
        
        #region Categories
        Categories =
        [
            new Category
            {
                Id = 1,
                Name = "CategoryName1",
                ShortName = "CategoryShortName1"
            },
            new Category
            {
                Id = 2,
                Name = "CategoryName2",
                ShortName = "CategoryShortName2"
            },
            new Category
            {
                Id = 3,
                Name = "CategoryName3",
                ShortName = "CategoryShortName3"
            }
        ];
        #endregion
        
        #region Positions
        Positions =
        [
            new Position
            {
                Id = 1,
                Name = "PositionName1",
                ShortName = "PositionShortName1"
            },
            new Position
            {
                Id = 2,
                Name = "PositionName2",
                ShortName = "PositionShortName2"
            },
            new Position
            {
                Id = 3,
                Name = "PositionName3",
                ShortName = "PositionShortName3"
            }
        ];
        #endregion
        
        #region Departments + DepartmentDtos
        Departments =
        [
            new Department
            {
                Id = 1,
                Name = "DepartmentName1",
                ShortName = "DepartmentShortName1",
                Comments = "DepartmentComments1"
            },
            new Department
            {
                Id = 2,
                Name = "DepartmentName2",
                ShortName = "DepartmentShortName2",
                Comments = "DepartmentComments2"
            },
            new Department
            {
                Id = 3,
                Name = "DepartmentName3",
                ShortName = "DepartmentShortName3",
                Comments = "DepartmentComments3"
            }
        ];
        
        DepartmentDtos =
        [
            new DepartmentDto
            {
                Id = Departments[0].Id,
                Name = Departments[0].Name,
                ShortName = Departments[0].ShortName,
                Comments = Departments[0].Comments
            },
            new DepartmentDto
            {
                Id = Departments[1].Id,
                Name = Departments[1].Name,
                ShortName = Departments[1].ShortName,
                Comments = Departments[1].Comments
            },
            new DepartmentDto
            {
                Id = Departments[2].Id,
                Name = Departments[2].Name,
                ShortName = Departments[2].ShortName,
                Comments = Departments[2].Comments
            }
        ];
        #endregion
        
        #region Housings + HousingDtos
        Housings = 
        [
            new Housing
            {
                Id = 1,
                Address = "Address 1",
                Name = "Name 1",
                ShortName = "Short name 1",
                FloorsCount = 2,
                Comments = "Comments 1"
            },
            new Housing
            {
                Id = 2,
                Address = "Address 2",
                Name = "Name 2",
                ShortName = "Short name 2",
                FloorsCount = 1,
                Comments = "Comments 2"
            },
            new Housing
            {
                Id = 3,
                Address = "Address 3",
                Name = "Name 3",
                ShortName = "Short name 3",
                FloorsCount = 2,
                Comments = "Comments 3"
            }
        ];
        
        HousingDtos = 
        [
            new HousingDto
            {
                Id = Housings[0].Id,
                Name = Housings[0].Name,
                ShortName = Housings[0].ShortName,
                FloorsCount = Housings[0].FloorsCount,
                Address = Housings[0].Address,
                Comments = Housings[0].Comments
            },
            new HousingDto
            {
                Id = Housings[1].Id,
                Name = Housings[1].Name,
                ShortName = Housings[1].ShortName,
                FloorsCount = Housings[1].FloorsCount,
                Address = Housings[1].Address,
                Comments = Housings[1].Comments
            },
            new HousingDto
            {
                Id = Housings[2].Id,
                Name = Housings[2].Name,
                ShortName = Housings[2].ShortName,
                FloorsCount = Housings[2].FloorsCount,
                Address = Housings[2].Address,
                Comments = Housings[2].Comments
            }
        ];
        #endregion
        
        #region Sectors + SectorDtos
        Sectors =
        [
            new Sector
            {
                Id = 1,
                Name = "SectorName1",
                ShortName = "SectorShortName1",
                Comments = "SectorComments1",
                Department = Departments[0]
            },
            new Sector
            {
                Id = 2,
                Name = "SectorName2",
                ShortName = "SectorShortName2",
                Comments = "SectorComments2",
                Department = Departments[1]
            },
            new Sector
            {
                Id = 3,
                Name = "SectorName3",
                ShortName = "SectorShortName3",
                Comments = "SectorComments3",
                Department = Departments[2]
            }
        ];
        
        SectorDtos =
        [
            new SectorDto
            {
                Id = Sectors[0].Id,
                Name = Sectors[0].Name,
                ShortName = Sectors[0].ShortName,
                Comments = Sectors[0].Comments,
                DepartmentId = Sectors[0].Department.Id
            },
            new SectorDto
            {
                Id = Sectors[1].Id,
                Name = Sectors[1].Name,
                ShortName = Sectors[1].ShortName,
                Comments = Sectors[1].Comments,
                DepartmentId = Sectors[1].Department.Id
            },
            new SectorDto
            {
                Id = Sectors[2].Id,
                Name = Sectors[2].Name,
                ShortName = Sectors[2].ShortName,
                Comments = Sectors[2].Comments,
                DepartmentId = Sectors[2].Department.Id
            }
        ];
        #endregion

        Departments[0].Sectors = [ Sectors[0] ];
        Departments[1].Sectors = [ Sectors[1] ];
        Departments[2].Sectors = [ Sectors[2] ];

        DepartmentDtos[0].SectorIds = [ Sectors[0].Id ];
        DepartmentDtos[1].SectorIds = [ Sectors[1].Id ];
        DepartmentDtos[2].SectorIds = [ Sectors[2].Id ];
        
        #region Rooms + RoomDtos
        Rooms =
        [
            new Room
            {
                Id = 1,
                Name = "RoomName1",
                ShortName = "RoomShortName1",
                Comments = "RoomComments1",
                Sector = Sectors[0],
                Housing = Housings[2],
                Floor = 1,
                Length = 5d,
                Width = 4d,
                Height = 2.5
            },
            new Room
            {
                Id = 2,
                Name = "RoomName2",
                ShortName = "RoomShortName2",
                Comments = "RoomComments2",
                Sector = Sectors[1],
                Housing = Housings[1],
                Floor = 1,
                Length = 6d,
                Width = 5d,
                Height = 2.5
            },
            new Room
            {
                Id = 3,
                Name = "RoomName3",
                ShortName = "RoomShortName3",
                Comments = "RoomComments3",
                Sector = Sectors[2],
                Housing = Housings[0],
                Floor = 1,
                Length = 7d,
                Width = 6d,
                Height = 2.5
            },
            new Room
            {
                Id = 4,
                Name = "RoomName4",
                ShortName = "RoomShortName4",
                Comments = "RoomComments4",
                Sector = Sectors[1],
                Housing = Housings[1],
                Floor = 1,
                Length = 6d,
                Width = 5d,
                Height = 2.5
            },
            new Room
            {
                Id = 5,
                Name = "RoomName5",
                ShortName = "RoomShortName5",
                Comments = "RoomComments5",
                Sector = Sectors[2],
                Housing = Housings[1],
                Floor = 1,
                Length = 7d,
                Width = 6d,
                Height = 2.5
            }
        ];
        
        RoomDtos =
        [
            new RoomDto
            {
                Id = Rooms[0].Id,
                Name = Rooms[0].Name,
                ShortName = Rooms[0].ShortName,
                Comments = Rooms[0].Comments,
                SectorId = Rooms[0].Sector.Id,
                Floor = Rooms[0].Floor,
                Length = Rooms[0].Length,
                Width = Rooms[0].Width,
                Height = Rooms[0].Height,
                HousingId = Rooms[0].Housing.Id
            },
            new RoomDto
            {
                Id = Rooms[1].Id,
                Name = Rooms[1].Name,
                ShortName = Rooms[1].ShortName,
                Comments = Rooms[1].Comments,
                SectorId = Rooms[1].Sector.Id,
                Floor = Rooms[1].Floor,
                Length = Rooms[1].Length,
                Width = Rooms[1].Width,
                Height = Rooms[1].Height,
                HousingId = Rooms[1].Housing.Id
            },
            new RoomDto
            {
                Id = Rooms[2].Id,
                Name = Rooms[2].Name,
                ShortName = Rooms[2].ShortName,
                Comments = Rooms[2].Comments,
                SectorId = Rooms[2].Sector.Id,
                Floor = Rooms[2].Floor,
                Length = Rooms[2].Length,
                Width = Rooms[2].Width,
                Height = Rooms[2].Height,
                HousingId = Rooms[2].Housing.Id
            },
            new RoomDto
            {
                Id = Rooms[3].Id,
                Name = Rooms[3].Name,
                ShortName = Rooms[3].ShortName,
                Comments = Rooms[3].Comments,
                SectorId = Rooms[3].Sector.Id,
                Floor = Rooms[3].Floor,
                Length = Rooms[3].Length,
                Width = Rooms[3].Width,
                Height = Rooms[3].Height,
                HousingId = Rooms[3].Housing.Id
            },
            new RoomDto
            {
                Id = Rooms[4].Id,
                Name = Rooms[4].Name,
                ShortName = Rooms[4].ShortName,
                Comments = Rooms[4].Comments,
                SectorId = Rooms[4].Sector.Id,
                Floor = Rooms[4].Floor,
                Length = Rooms[4].Length,
                Width = Rooms[4].Width,
                Height = Rooms[4].Height,
                HousingId = Rooms[4].Housing.Id
            }
        ];
        #endregion
        
        Housings[0].Rooms = [ Rooms[0], Rooms[1] ];
        Housings[1].Rooms = [ Rooms[2] ];
        Housings[2].Rooms = [ Rooms[3], Rooms[4] ];
        
        HousingDtos[0].RoomIds = Housings[0].Rooms.Select(r => r.Id).ToList();
        HousingDtos[1].RoomIds = Housings[1].Rooms.Select(r => r.Id).ToList();
        HousingDtos[2].RoomIds = Housings[2].Rooms.Select(r => r.Id).ToList();
        
        Sectors[0].Rooms = [ Rooms[0], Rooms[1] ];
        Sectors[1].Rooms = [ Rooms[2], Rooms[3] ];
        Sectors[2].Rooms = [ Rooms[4] ];
        
        SectorDtos[0].RoomIds = Sectors[0].Rooms.Select(r => r.Id).ToList();
        SectorDtos[1].RoomIds = Sectors[1].Rooms.Select(r => r.Id).ToList();
        SectorDtos[2].RoomIds = Sectors[2].Rooms.Select(r => r.Id).ToList();
        
        #region Persons + PersonDtos
        Persons =
        [
            new Person
            {
                Id = 1,
                FirstName = "FirstName1",
                MiddleName = "MiddleName1",
                LastName = "LastName1",
                BirthdayDate = new DateTime(2000, 10, 15),
                Category = Categories[0],
                Comments = "PersonsComments1",
                Position = Positions[0],
                Sector = Sectors[0],
                Role = Roles[0],
                User = Users[0]
            },
            new Person
            {
                Id = 2,
                FirstName = "FirstName2",
                MiddleName = "MiddleName2",
                LastName = "LastName2",
                BirthdayDate = new DateTime(2000, 10, 16),
                Category = Categories[1],
                Comments = "PersonsComments2",
                Position = Positions[1],
                Sector = Sectors[0],
                Role = Roles[1],
                User = Users[1]
            },
            new Person
            {
                Id = 3,
                FirstName = "FirstName3",
                MiddleName = "MiddleName3",
                LastName = "LastName3",
                BirthdayDate = new DateTime(2000, 10, 17),
                Category = Categories[2],
                Comments = "PersonsComments3",
                Position = Positions[2],
                Sector = Sectors[0],
                Role = Roles[2],
                User = Users[2]
            },
            new Person
            {
                Id = 4,
                FirstName = "FirstName4",
                MiddleName = "MiddleName4",
                LastName = "LastName4",
                BirthdayDate = new DateTime(2000, 10, 18),
                Category = Categories[2],
                Comments = "PersonsComments4",
                Position = Positions[2],
                Sector = Sectors[1],
                Role = Roles[3],
                User = Users[3]
            },
            new Person
            {
                Id = 5,
                FirstName = "FirstName5",
                MiddleName = "MiddleName5",
                LastName = "LastName5",
                BirthdayDate = new DateTime(2000, 10, 19),
                Category = Categories[1],
                Comments = "PersonsComments5",
                Position = Positions[1],
                Sector = Sectors[1],
                Role = Roles[3],
                User = Users[4]
            },
            new Person
            {
                Id = 6,
                FirstName = "FirstName6",
                MiddleName = "MiddleName6",
                LastName = "LastName6",
                BirthdayDate = new DateTime(2000, 10, 20),
                Category = Categories[2],
                Comments = "PersonsComments6",
                Position = Positions[2],
                Sector = Sectors[1],
                Role = Roles[4],
                User = Users[5]
            },
            new Person
            {
                Id = 7,
                FirstName = "FirstName7",
                MiddleName = "MiddleName7",
                LastName = "LastName7",
                BirthdayDate = new DateTime(2000, 10, 21),
                Category = Categories[1],
                Comments = "PersonsComments4",
                Position = Positions[2],
                Sector = Sectors[2],
                Role = Roles[3],
                User = Users[6]
            },
            new Person
            {
                Id = 8,
                FirstName = "FirstName8",
                MiddleName = "MiddleName8",
                LastName = "LastName8",
                BirthdayDate = new DateTime(2000, 10, 22),
                Category = Categories[2],
                Comments = "PersonsComments8",
                Position = Positions[2],
                Sector = Sectors[2],
                Role = Roles[4],
                User = Users[7]
            }
        ];
        
        PersonDtos =
        [
            new PersonDto
            {
                Id = Persons[0].Id,
                FirstName = Persons[0].FirstName,
                MiddleName = Persons[0].MiddleName,
                LastName = Persons[0].LastName,
                BirthdayDate = Persons[0].BirthdayDate,
                CategoryId = Persons[0].Category.Id,
                Comments = Persons[0].Comments,
                PositionId = Persons[0].Position.Id,
                SectorId = Persons[0].Sector.Id,
                Role = Persons[0].Role,
                UserId = Persons[0].User.Id
            },
            new PersonDto
            {
                Id = Persons[1].Id,
                FirstName = Persons[1].FirstName,
                MiddleName = Persons[1].MiddleName,
                LastName = Persons[1].LastName,
                BirthdayDate = Persons[1].BirthdayDate,
                CategoryId = Persons[1].Category.Id,
                Comments = Persons[1].Comments,
                PositionId = Persons[1].Position.Id,
                SectorId = Persons[1].Sector.Id,
                Role = Persons[1].Role,
                UserId = Persons[1].User.Id
            },
            new PersonDto
            {
                Id = Persons[2].Id,
                FirstName = Persons[2].FirstName,
                MiddleName = Persons[2].MiddleName,
                LastName = Persons[2].LastName,
                BirthdayDate = Persons[2].BirthdayDate,
                CategoryId = Persons[2].Category.Id,
                Comments = Persons[2].Comments,
                PositionId = Persons[2].Position.Id,
                SectorId = Persons[2].Sector.Id,
                Role = Persons[2].Role,
                UserId = Persons[2].User.Id
            },
            new PersonDto
            {
                Id = Persons[3].Id,
                FirstName = Persons[3].FirstName,
                MiddleName = Persons[3].MiddleName,
                LastName = Persons[3].LastName,
                BirthdayDate = Persons[3].BirthdayDate,
                CategoryId = Persons[3].Category.Id,
                Comments = Persons[3].Comments,
                PositionId = Persons[3].Position.Id,
                SectorId = Persons[3].Sector.Id,
                Role = Persons[3].Role,
                UserId = Persons[3].User.Id
            },
            new PersonDto
            {
                Id = Persons[4].Id,
                FirstName = Persons[4].FirstName,
                MiddleName = Persons[4].MiddleName,
                LastName = Persons[4].LastName,
                BirthdayDate = Persons[4].BirthdayDate,
                CategoryId = Persons[4].Category.Id,
                Comments = Persons[4].Comments,
                PositionId = Persons[4].Position.Id,
                SectorId = Persons[4].Sector.Id,
                Role = Persons[4].Role,
                UserId = Persons[4].User.Id
            },
            new PersonDto
            {
                Id = Persons[5].Id,
                FirstName = Persons[5].FirstName,
                MiddleName = Persons[5].MiddleName,
                LastName = Persons[5].LastName,
                BirthdayDate = Persons[5].BirthdayDate,
                CategoryId = Persons[5].Category.Id,
                Comments = Persons[5].Comments,
                PositionId = Persons[5].Position.Id,
                SectorId = Persons[5].Sector.Id,
                Role = Persons[5].Role,
                UserId = Persons[5].User.Id
            },
            new PersonDto
            {
                Id = Persons[6].Id,
                FirstName = Persons[6].FirstName,
                MiddleName = Persons[6].MiddleName,
                LastName = Persons[6].LastName,
                BirthdayDate = Persons[6].BirthdayDate,
                CategoryId = Persons[6].Category.Id,
                Comments = Persons[6].Comments,
                PositionId = Persons[6].Position.Id,
                SectorId = Persons[6].Sector.Id,
                Role = Persons[6].Role,
                UserId = Persons[6].User.Id
            },
            new PersonDto
            {
                Id = Persons[7].Id,
                FirstName = Persons[7].FirstName,
                MiddleName = Persons[7].MiddleName,
                LastName = Persons[7].LastName,
                BirthdayDate = Persons[7].BirthdayDate,
                CategoryId = Persons[7].Category.Id,
                Comments = Persons[7].Comments,
                PositionId = Persons[7].Position.Id,
                SectorId = Persons[7].Sector.Id,
                Role = Persons[7].Role,
                UserId = Persons[7].User.Id
            }
        ];
        #endregion
        
        Sectors[0].Persons = [ Persons[0], Persons[1], Persons[2] ];
        Sectors[1].Persons = [ Persons[3], Persons[4], Persons[5] ];
        Sectors[2].Persons = [ Persons[6], Persons[7] ];

        SectorDtos[0].PersonIds = Sectors[0].Persons.Select(s => s.Id).ToList();
        SectorDtos[1].PersonIds = Sectors[1].Persons.Select(s => s.Id).ToList();
        SectorDtos[2].PersonIds = Sectors[2].Persons.Select(s => s.Id).ToList();
        
        #region Workplaces + WorkplaceDtos
        Workplaces =
        [
            new Workplace
            {
                Id = 1,
                Name = "WorkPlaceName1",
                ShortName = "WorkPlaceShortName1",
                Comments = "WorkplaceComments1",
                Responsible = Persons[0],
                Room = Rooms[4],
                ImagePath = null
            },
            new Workplace
            {
                Id = 2,
                Name = "WorkPlaceName2",
                ShortName = "WorkPlaceShortName2",
                Comments = "WorkplaceComments2",
                Responsible = Persons[1],
                Room = Rooms[4],
                ImagePath = null
            },
            new Workplace
            {
                Id = 3,
                Name = "WorkPlaceName3",
                ShortName = "WorkPlaceShortName3",
                Comments = "WorkplaceComments3",
                Responsible = Persons[2],
                Room = Rooms[2],
                ImagePath = null
            },
            new Workplace
            {
                Id = 4,
                Name = "WorkPlaceName4",
                ShortName = "WorkPlaceShortName4",
                Comments = "WorkplaceComments4",
                Responsible = Persons[3],
                Room = Rooms[3],
                ImagePath = null
            },
            new Workplace
            {
                Id = 5,
                Name = "WorkPlaceName5",
                ShortName = "WorkPlaceShortName5",
                Comments = "WorkplaceComments5",
                Responsible = Persons[4],
                Room = Rooms[0],
                ImagePath = null
            },
            new Workplace
            {
                Id = 6,
                Name = "WorkPlaceName6",
                ShortName = "WorkPlaceShortName6",
                Comments = "WorkplaceComments6",
                Responsible = Persons[5],
                Room = Rooms[1],
                ImagePath = null
            }
        ];
        
        WorkplaceDtos =
        [
            new WorkplaceDto
            {
                Id = Workplaces[0].Id,
                Name = Workplaces[0].Name,
                ShortName = Workplaces[0].ShortName,
                Comments = Workplaces[0].Comments,
                ResponsibleId = Workplaces[0].Responsible.Id,
                RoomId = Workplaces[0].Room.Id,
                ImagePath = Workplaces[0].ImagePath
            },
            new WorkplaceDto
            {
                Id = Workplaces[1].Id,
                Name = Workplaces[1].Name,
                ShortName = Workplaces[1].ShortName,
                Comments = Workplaces[1].Comments,
                ResponsibleId = Workplaces[1].Responsible.Id,
                RoomId = Workplaces[1].Room.Id,
                ImagePath = Workplaces[1].ImagePath
            },
            new WorkplaceDto
            {
                Id = Workplaces[2].Id,
                Name = Workplaces[2].Name,
                ShortName = Workplaces[2].ShortName,
                Comments = Workplaces[2].Comments,
                ResponsibleId = Workplaces[2].Responsible.Id,
                RoomId = Workplaces[2].Room.Id,
                ImagePath = Workplaces[2].ImagePath
            },
            new WorkplaceDto
            {
                Id = Workplaces[3].Id,
                Name = Workplaces[3].Name,
                ShortName = Workplaces[3].ShortName,
                Comments = Workplaces[3].Comments,
                ResponsibleId = Workplaces[3].Responsible.Id,
                RoomId = Workplaces[3].Room.Id,
                ImagePath = Workplaces[3].ImagePath
            },
            new WorkplaceDto
            {
                Id = Workplaces[4].Id,
                Name = Workplaces[4].Name,
                ShortName = Workplaces[4].ShortName,
                Comments = Workplaces[4].Comments,
                ResponsibleId = Workplaces[4].Responsible.Id,
                RoomId = Workplaces[4].Room.Id,
                ImagePath = Workplaces[4].ImagePath
            },
            new WorkplaceDto
            {
                Id = Workplaces[5].Id,
                Name = Workplaces[5].Name,
                ShortName = Workplaces[5].ShortName,
                Comments = Workplaces[5].Comments,
                ResponsibleId = Workplaces[5].Responsible.Id,
                RoomId = Workplaces[5].Room.Id,
                ImagePath = Workplaces[5].ImagePath
            }
        ];
        #endregion

        Sectors[0].Workplaces = [ Workplaces[4], Workplaces[5] ];
        Sectors[1].Workplaces = [ Workplaces[2], Workplaces[3] ];
        Sectors[2].Workplaces = [ Workplaces[0], Workplaces[1] ];

        SectorDtos[0].WorkplaceIds = Sectors[0].Workplaces.Select(s => s.Id).ToList();
        SectorDtos[1].WorkplaceIds = Sectors[1].Workplaces.Select(s => s.Id).ToList();
        SectorDtos[2].WorkplaceIds = Sectors[2].Workplaces.Select(s => s.Id).ToList();

        Rooms[0].WorkPlaces = [ Workplaces[0], Workplaces[1] ];
        Rooms[1].WorkPlaces = [ Workplaces[1] ];
        Rooms[2].WorkPlaces = [ Workplaces[2] ];
        Rooms[3].WorkPlaces = [ Workplaces[3] ];
        Rooms[4].WorkPlaces = [ Workplaces[4] ];
        
        RoomDtos[0].WorkplaceIds = [ Workplaces[0].Id, Workplaces[1].Id ];
        RoomDtos[1].WorkplaceIds = [ Workplaces[1].Id ];
        RoomDtos[2].WorkplaceIds = [ Workplaces[2].Id ];
        RoomDtos[3].WorkplaceIds = [ Workplaces[3].Id ];
        RoomDtos[4].WorkplaceIds = [ Workplaces[4].Id ];

        Rooms[0].Persons = [ Persons[0], Persons[1] ];
        Rooms[1].Persons = [ Persons[1], Persons[2] ];
        Rooms[2].Persons = [ Persons[3] ];
        Rooms[3].Persons = [ Persons[4], Persons[5] ];
        Rooms[4].Persons = [ Persons[7] ];
        
        RoomDtos[0].PersonIds = [ Persons[0].Id, Persons[1].Id ];
        RoomDtos[1].PersonIds = [ Persons[1].Id, Persons[2].Id ];
        RoomDtos[2].PersonIds = [ Persons[3].Id ];
        RoomDtos[3].PersonIds = [ Persons[4].Id, Persons[5].Id ];
        RoomDtos[4].PersonIds = [ Persons[7].Id ];
        
        #region ServiceTypes
        ServiceTypes =
        [
            new ServiceType
            {
                Id = 1,
                Name = "ServiceTypeName1",
                ShortName = "ServiceTypeShortName1"
            },
            new ServiceType
            {
                Id = 2,
                Name = "ServiceTypeName2",
                ShortName = "ServiceTypeShortName2"
            },
            new ServiceType
            {
                Id = 3,
                Name = "ServiceTypeName3",
                ShortName = "ServiceTypeShortName3"
            }
        ];
        #endregion
        
        #region Grades
        Grades =
        [
            new Grade
            {
                Id = 1,
                Name = "GradeName1",
                ShortName = "GradeShortName1",
                Comments = "Comments1"
            },
            new Grade
            { 
                Id = 2,
                Name = "GradeName2",
                ShortName = "GradeShortName2",
                Comments = "Comments2"
            },
            new Grade
            {
                Id = 3,
                Name = "GradeName3",
                ShortName = "GradeShortName3",
                Comments = "Comments3"
            }
        ];
        #endregion

        #region Quantities + QuantityDtos
        Quantities =
        [
            new Quantity
            {
                Id = 1,
                Name = "QuantityName1"
            },
            new Quantity
            { 
                Id = 2,
                Name = "QuantityName2"
            },
            new Quantity
            {
                Id = 3,
                Name = "QuantityName3"
            }
        ];
        
        QuantityDtos =
        [
            new QuantityDto
            {
                Id = Quantities[0].Id,
                Name = Quantities[0].Name
            },
            new QuantityDto
            {
                Id = Quantities[1].Id,
                Name = Quantities[1].Name
            },
            new QuantityDto
            {
                Id = Quantities[2].Id,
                Name = Quantities[2].Name
            }
        ];
        #endregion

        #region Units + UnitDtos
        Units =
        [
            new Unit
            {
                Id = 1,
                Quantity = Quantities[0],
                Name = "UnitName1",
                RuName = "UnitRuName1",
                Symbol = "UnitSymbol1",
                RuSymbol = "UnitRuSymbol1"
            },
            new Unit
            {
                Id = 2,
                Quantity = Quantities[1],
                Name = "UnitName2",
                RuName = "UnitRuName2",
                Symbol = "UnitSymbol2",
                RuSymbol = "UnitRuSymbol2"
            },
            new Unit
            {
                Id = 3,
                Quantity = Quantities[2],
                Name = "UnitName3",
                RuName = "UnitRuName3",
                Symbol = "UnitSymbol3",
                RuSymbol = "UnitRuSymbol3"
            }
        ];
        
        UnitDtos =
        [
            new UnitDto
            {
                Id = Units[0].Id,
                QuantityId = Units[0].Quantity.Id,
                Name = Units[0].Name,
                RuName = Units[0].RuName,
                Symbol = Units[0].Symbol,
                RuSymbol = Units[0].RuSymbol
            },
            new UnitDto
            {
                Id = Units[1].Id,
                QuantityId = Units[1].Quantity.Id,
                Name = Units[1].Name,
                RuName = Units[1].RuName,
                Symbol = Units[1].Symbol,
                RuSymbol = Units[1].RuSymbol
            },
            new UnitDto
            {
                Id = Units[2].Id,
                QuantityId = Units[2].Quantity.Id,
                Name = Units[2].Name,
                RuName = Units[2].RuName,
                Symbol = Units[2].Symbol,
                RuSymbol = Units[2].RuSymbol
            }
        ];
        #endregion

        Quantities[0].Units = new List<Unit> { Units[0] };
        Quantities[1].Units = new List<Unit> { Units[1] };
        Quantities[2].Units = new List<Unit> { Units[2] };

        QuantityDtos[0].UnitIds = new List<int> { Units[0].Id };
        QuantityDtos[1].UnitIds = new List<int> { Units[1].Id };
        QuantityDtos[2].UnitIds = new List<int> { Units[2].Id };

        #region Materials + MaterialDtos
        Materials =
        [
            new Material
            {
                Id = 1,
                Name = "MaterialName1",
                ShortName = "MaterialShortName1",
                Unit = Units[0],
                Comments = "Comments1"
            },
            new Material
            {
                Id = 2,
                Name = "MaterialName2",
                ShortName = "MaterialShortName2",
                Unit = Units[1],
                Comments = "Comments2"
            },
            new Material
            {
                Id = 3,
                Name = "MaterialName3",
                ShortName = "MaterialShortName3",
                Unit = Units[2],
                Comments = "Comments3"
            }
        ];
        
        MaterialDtos =
        [
            new MaterialDto
            {
                Id = Materials[0].Id,
                Name = Materials[0].Name,
                ShortName = Materials[0].ShortName,
                UnitId = Materials[0].Unit.Id,
                Comments = Materials[0].Comments
            },
            new MaterialDto
            {
                Id = Materials[1].Id,
                Name = Materials[1].Name,
                ShortName = Materials[1].ShortName,
                UnitId = Materials[1].Unit.Id,
                Comments = Materials[1].Comments
            },
            new MaterialDto
            {
                Id = Materials[2].Id,
                Name = Materials[2].Name,
                ShortName = Materials[2].ShortName,
                UnitId = Materials[2].Unit.Id,
                Comments = Materials[2].Comments
            }
        ];
        #endregion

        #region Services + ServiceDtos
        Services =
        [
            new Service
            {
                Id = 1,
                Name = "ServiceName1",
                ShortName = "ServiceShortName1",
                Comments = "ServiceComments1",
                ServiceType = ServiceTypes[0],
                Materials = [Materials[0]],
                MaterialsQuantities = [Quantities[0]]
            },
            new Service
            {
                Id = 2,
                Name = "ServiceName2",
                ShortName = "ServiceShortName2",
                Comments = "ServiceComments2",
                ServiceType = ServiceTypes[1],
                Materials = [Materials[1]],
                MaterialsQuantities = [Quantities[1]]
            },
            new Service
            {
                Id = 3,
                Name = "Service",
                ShortName = "ServiceShortName3",
                Comments = "ServiceComments3",
                ServiceType = ServiceTypes[2],
                Materials = [Materials[2]],
                MaterialsQuantities = [Quantities[2]]
            }
        ];
        
        ServiceDtos =
        [
            new ServiceDto
            {
                Id = Services[0].Id,
                Name = Services[0].Name,
                ShortName = Services[0].ShortName,
                Comments = Services[0].Comments,
                ServiceTypeId = Services[0].ServiceType.Id,
                MaterialIds = Services[0].Materials.Select(m => m.Id).ToList(),
                MaterialsQuantityIds = Services[0].MaterialsQuantities.Select(q => q.Id).ToList()
            },
            new ServiceDto
            {
                Id = Services[1].Id,
                Name = Services[1].Name,
                ShortName = Services[1].ShortName,
                Comments = Services[1].Comments,
                ServiceTypeId = Services[1].ServiceType.Id,
                MaterialIds = Services[1].Materials.Select(m => m.Id).ToList(),
                MaterialsQuantityIds = Services[1].MaterialsQuantities.Select(q => q.Id).ToList()
            },
            new ServiceDto
            {
                Id = Services[2].Id,
                Name = Services[2].Name,
                ShortName = Services[2].ShortName,
                Comments = Services[2].Comments,
                ServiceTypeId = Services[2].ServiceType.Id,
                MaterialIds = Services[2].Materials.Select(m => m.Id).ToList(),
                MaterialsQuantityIds = Services[2].MaterialsQuantities.Select(q => q.Id).ToList()
            }
        ];
        #endregion

        #region Characteristics + CharacteristicDtos
        Characteristics = 
        [
            new Characteristic
            {
                Id = 1,
                Name = "CharacteristicName1",
                ShortName = "CharacteristicShortName1",
                Comments = "Comments1",
                Grade = Grades[0],
                GradeValue = 3d,
                GradeValueStart = 0d,
                GradeValueEnd = 7d,
                RangeStart = -10d,
                RangeEnd = 10d,
                Unit = Units[0]
            },
            new Characteristic
            {
                Id = 2,
                Name = "CharacteristicName2",
                ShortName = "CharacteristicShortName2",
                Comments = "Comments2",
                Grade = Grades[1],
                GradeValue = 1d,
                GradeValueStart = 0d,
                GradeValueEnd = 4d,
                RangeStart = -5d,
                RangeEnd = 10d,
                Unit = Units[1]
            },
            new Characteristic
            {
                Id = 3,
                Name = "CharacteristicName3",
                ShortName = "CharacteristicShortName3",
                Comments = "Comments3",
                Grade = Grades[2],
                GradeValue = 4d,
                GradeValueStart = 0d,
                GradeValueEnd = 10d,
                RangeStart = 0d,
                RangeEnd = 10d,
                Unit = Units[2]
            }
        ];

        CharacteristicsDtos = 
        [
            new CharacteristicDto
            {
                Id = Characteristics[0].Id,
                Name = Characteristics[0].Name,
                ShortName = Characteristics[0].ShortName,
                Comments = Characteristics[0].Comments,
                GradeId = Characteristics[0].Grade.Id,
                GradeValue = Characteristics[0].GradeValue,
                GradeValueStart = Characteristics[0].GradeValueStart,
                GradeValueEnd = Characteristics[0].GradeValueEnd,
                RangeStart = Characteristics[0].RangeStart,
                RangeEnd = Characteristics[0].RangeEnd,
                UnitId = Characteristics[0].Unit.Id
            },
            new CharacteristicDto
            {
                Id = Characteristics[1].Id,
                Name = Characteristics[1].Name,
                ShortName = Characteristics[1].ShortName,
                Comments = Characteristics[1].Comments,
                GradeId = Characteristics[1].Grade.Id,
                GradeValue = Characteristics[1].GradeValue,
                GradeValueStart = Characteristics[1].GradeValueStart,
                GradeValueEnd = Characteristics[1].GradeValueEnd,
                RangeStart = Characteristics[1].RangeStart,
                RangeEnd = Characteristics[1].RangeEnd,
                UnitId = Characteristics[1].Unit.Id
            },
            new CharacteristicDto
            {
                Id = Characteristics[2].Id,
                Name = Characteristics[2].Name,
                ShortName = Characteristics[2].ShortName,
                Comments = Characteristics[2].Comments,
                GradeId = Characteristics[2].Grade.Id,
                GradeValue = Characteristics[2].GradeValue,
                GradeValueStart = Characteristics[2].GradeValueStart,
                GradeValueEnd = Characteristics[2].GradeValueEnd,
                RangeStart = Characteristics[2].RangeStart,
                RangeEnd = Characteristics[2].RangeEnd,
                UnitId = Characteristics[2].Unit.Id
            }
        ];
        #endregion

        #region Standards
        Standards =
        [
            new Standard
            {
                Id = 1,
                Name = "StandardName1",
                ShortName = "StandardShortName1",
                Services = new List<Service>(),
                CalibrationInterval = 24,
                VerificationInterval = 12,
                ImagePath = null,
                Responsible = Persons[0],
                Workplaces = [ Workplaces[0], Workplaces[1] ],
                Characteristics = [ Characteristics[0] ],
                Comments = "Comments1"
            },
            new Standard
            {
                Id = 2,
                Name = "StandardName2",
                ShortName = "StandardShortName2",
                Services = new List<Service>(),
                CalibrationInterval = 36,
                VerificationInterval = 12,
                ImagePath = null,
                Responsible = Persons[1],
                Workplaces = [ Workplaces[2], Workplaces[3] ],
                Characteristics = [ Characteristics[1] ],
                Comments = "Comments2"
            },
            new Standard
            {
                Id = 3,
                Name = "StandardName3",
                ShortName = "StandardShortName3",
                Services = new List<Service>(),
                CalibrationInterval = 12,
                VerificationInterval = 6,
                ImagePath = null,
                Responsible = Persons[2],
                Workplaces = [ Workplaces[4], Workplaces[5] ],
                Characteristics = [ Characteristics[2] ],
                Comments = "Comments3"
            }
        ];
        
        StandardDtos =
        [
            new StandardDto
            {
                Id = Standards[0].Id,
                Name = Standards[0].Name,
                ShortName = Standards[0].ShortName,
                ServiceIds = Standards[0].Services.Select(s => s.Id).ToList(),
                CalibrationInterval = Standards[0].CalibrationInterval,
                VerificationInterval = Standards[0].VerificationInterval,
                ImagePath = Standards[0].ImagePath,
                ResponsibleId = Standards[0].Responsible.Id,
                WorkplaceIds = Standards[0].Workplaces.Select(wp => wp.Id).ToList(),
                CharacteristicIds = Standards[0].Characteristics.Select(c => c.Id).ToList(),
                Comments = Standards[0].Comments
            },
            new StandardDto
            {
                Id = Standards[1].Id,
                Name = Standards[1].Name,
                ShortName = Standards[1].ShortName,
                ServiceIds = Standards[1].Services.Select(s => s.Id).ToList(),
                CalibrationInterval = Standards[1].CalibrationInterval,
                VerificationInterval = Standards[1].VerificationInterval,
                ImagePath = Standards[1].ImagePath,
                ResponsibleId = Standards[1].Responsible.Id,
                WorkplaceIds = Standards[1].Workplaces.Select(wp => wp.Id).ToList(),
                CharacteristicIds = Standards[1].Characteristics.Select(c => c.Id).ToList(),
                Comments = Standards[1].Comments
            },
            new StandardDto
            {
                Id = Standards[2].Id,
                Name = Standards[2].Name,
                ShortName = Standards[2].ShortName,
                ServiceIds = Standards[2].Services.Select(s => s.Id).ToList(),
                CalibrationInterval = Standards[2].CalibrationInterval,
                VerificationInterval = Standards[2].VerificationInterval,
                ImagePath = Standards[2].ImagePath,
                ResponsibleId = Standards[2].Responsible.Id,
                WorkplaceIds = Standards[2].Workplaces.Select(wp => wp.Id).ToList(),
                CharacteristicIds = Standards[2].Characteristics.Select(c => c.Id).ToList(),
                Comments = Standards[2].Comments
            }
        ];
        #endregion

        Characteristics[0].Standard = Standards[0];
        Characteristics[1].Standard = Standards[1];
        Characteristics[2].Standard = Standards[2];

        CharacteristicsDtos[0].StandardId = Characteristics[0].Standard.Id;
        CharacteristicsDtos[1].StandardId = Characteristics[1].Standard.Id;
        CharacteristicsDtos[2].StandardId = Characteristics[2].Standard.Id;

        #region Places
        Places =
        [
            new Place
            {
                Id = 1,
                Name = "PlaceName1",
                ShortName = "PlaceShortName1",
                Comments = "Comments1"
            },
            new Place
            { 
                Id = 2,
                Name = "PlaceName2",
                ShortName = "PlaceShortName2",
                Comments = "Comments2"
            },
            new Place
            {
                Id = 3,
                Name = "PlaceName3",
                ShortName = "PlaceShortName3",
                Comments = "Comments3"
            },
            new Place
            {
                Id = 4,
                Name = "PlaceName4",
                ShortName = "PlaceShortName4",
                Comments = "Comments4"
            },
            new Place
            { 
                Id = 5,
                Name = "PlaceName5",
                ShortName = "PlaceShortName5",
                Comments = "Comments5"
            },
            new Place
            {
                Id = 6,
                Name = "PlaceName6",
                ShortName = "PlaceShortName6",
                Comments = "Comments6"
            }
        ];
        #endregion

        #region ServiceJournalItems + ServiceJournalItemDtos
        ServiceJournalItems =
        [
            new ServiceJournalItem
            {
                Id = 1,
                Name = "ServiceJournalItemName1",
                ShortName = "ServiceJournalItemShortName1",
                Comments = "ServiceJournalItemComments1",
                Service = Services[0],
                Standard = Standards[0],
                Person = Persons[0],
                Date = new DateTime(2024, 12, 10, 0, 0, 0, DateTimeKind.Utc)
            },
            new ServiceJournalItem
            {
                Id = 2,
                Name = "ServiceJournalItemName2",
                ShortName = "ServiceJournalItemShortName2",
                Comments = "ServiceJournalItemComments2",
                Service = Services[1],
                Standard = Standards[1],
                Person = Persons[1],
                Date = new DateTime(2024, 12, 11, 0, 0, 0, DateTimeKind.Utc)
            },
            new ServiceJournalItem
            {
                Id = 3,
                Name = "ServiceJournalItemName3",
                ShortName = "ServiceJournalItemShortName3",
                Comments = "ServiceJournalItemComments3",
                Service = Services[2],
                Standard = Standards[2],
                Person = Persons[2],
                Date = new DateTime(2024, 12, 12, 0, 0, 0, DateTimeKind.Utc)
            }
        ];
        
        ServiceJournalItemDtos =
        [
            new ServiceJournalItemDto
            {
                Id = ServiceJournalItems[0].Id,
                Name = ServiceJournalItems[0].Name,
                ShortName = ServiceJournalItems[0].ShortName,
                Comments = ServiceJournalItems[0].Comments,
                ServiceId = ServiceJournalItems[0].Service.Id,
                StandardId = ServiceJournalItems[0].Standard.Id,
                PersonId = ServiceJournalItems[0].Person.Id,
                Date = ServiceJournalItems[0].Date
            },
            new ServiceJournalItemDto
            {
                Id = ServiceJournalItems[1].Id,
                Name = ServiceJournalItems[1].Name,
                ShortName = ServiceJournalItems[1].ShortName,
                Comments = ServiceJournalItems[1].Comments,
                ServiceId = ServiceJournalItems[1].Service.Id,
                StandardId = ServiceJournalItems[1].Standard.Id,
                PersonId = ServiceJournalItems[1].Person.Id,
                Date = ServiceJournalItems[1].Date
            },
            new ServiceJournalItemDto
            {
                Id = ServiceJournalItems[2].Id,
                Name = ServiceJournalItems[2].Name,
                ShortName = ServiceJournalItems[2].ShortName,
                Comments = ServiceJournalItems[2].Comments,
                ServiceId = ServiceJournalItems[2].Service.Id,
                StandardId = ServiceJournalItems[2].Standard.Id,
                PersonId = ServiceJournalItems[2].Person.Id,
                Date = ServiceJournalItems[2].Date
            }
        ];
        #endregion

        #region VerificationJournalItems + VerificationJournalItemDtos
        VerificationJournalItems =
        [
            new VerificationJournalItem
            {
                Id = 1,
                Standard = Standards[0],
                Place = Places[0],
                Date = new DateTime(2024, 12, 10, 0, 0, 0, DateTimeKind.Utc),
                ValidTo = new DateTime(2025, 12, 10, 0, 0, 0, DateTimeKind.Utc),
                CertificateId = "CertificateId1",
                CertificateImage = "CertificateImage1",
                Comments = "VerificationJournalItemComments1"
            },
            new VerificationJournalItem
            {
                Id = 2,
                Standard = Standards[1],
                Place = Places[1],
                Date = new DateTime(2024, 12, 11, 0, 0, 0, DateTimeKind.Utc),
                ValidTo = new DateTime(2025, 12, 11, 0, 0, 0, DateTimeKind.Utc),
                CertificateId = "CertificateId2",
                CertificateImage = "CertificateImage2",
                Comments = "VerificationJournalItemComments2"
            },
            new VerificationJournalItem
            {
                Id = 3,
                Standard = Standards[2],
                Place = Places[2],
                Date = new DateTime(2024, 12, 12, 0, 0, 0, DateTimeKind.Utc),
                ValidTo = new DateTime(2025, 12, 12, 0, 0, 0, DateTimeKind.Utc),
                CertificateId = "CertificateId3",
                CertificateImage = "CertificateImage3",
                Comments = "VerificationJournalItemComments3"
            }
        ];
        
        VerificationJournalItemDtos =
        [
            new VerificationJournalItemDto
            {
                Id = VerificationJournalItems[0].Id,
                StandardId = VerificationJournalItems[0].Standard.Id,
                PlaceId = VerificationJournalItems[0].Place.Id,
                Comments = VerificationJournalItems[0].Comments,
                Date = VerificationJournalItems[0].Date,
                ValidTo = VerificationJournalItems[0].ValidTo,
                CertificateId = VerificationJournalItems[0].CertificateId,
                CertificateImage = VerificationJournalItems[0].CertificateImage
            },
            new VerificationJournalItemDto
            {
                Id = VerificationJournalItems[1].Id,
                StandardId = VerificationJournalItems[1].Standard.Id,
                PlaceId = VerificationJournalItems[1].Place.Id,
                Comments = VerificationJournalItems[1].Comments,
                Date = VerificationJournalItems[1].Date,
                ValidTo = VerificationJournalItems[1].ValidTo,
                CertificateId = VerificationJournalItems[1].CertificateId,
                CertificateImage = VerificationJournalItems[1].CertificateImage
            },
            new VerificationJournalItemDto
            {
                Id = VerificationJournalItems[2].Id,
                StandardId = VerificationJournalItems[2].Standard.Id,
                PlaceId = VerificationJournalItems[2].Place.Id,
                Comments = VerificationJournalItems[2].Comments,
                Date = VerificationJournalItems[2].Date,
                ValidTo = VerificationJournalItems[2].ValidTo,
                CertificateId = VerificationJournalItems[2].CertificateId,
                CertificateImage = VerificationJournalItems[2].CertificateImage
            }
        ];
        #endregion
    }
    
    protected static IEnumerable<TestCaseData> NullOrEmptyString()
    {
        yield return new TestCaseData(Cases.Null);
        yield return new TestCaseData(Cases.EmptyString);
    }
    
    protected static IEnumerable<TestCaseData> NullDate()
    {
        yield return new TestCaseData(Cases.Null);
    }
    
    protected static IEnumerable<TestCaseData> ZeroOrNegativeId()
    {
        yield return new TestCaseData(Cases.Zero);
        yield return new TestCaseData(Cases.Negative);
    }
    
    protected static IEnumerable<TestCaseData> MinOrInPast()
    {
        yield return new TestCaseData(Cases.MinDateTime);
        yield return new TestCaseData(Cases.DateTimeInPast);
    }
}