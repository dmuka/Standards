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
                Id = 1,
                Name = "DepartmentName1",
                ShortName = "DepartmentShortName1",
                Comments = "DepartmentComments1"
            },
            new DepartmentDto
            {
                Id = 2,
                Name = "DepartmentName2",
                ShortName = "DepartmentShortName2",
                Comments = "DepartmentComments2"
            },
            new DepartmentDto
            {
                Id = 3,
                Name = "DepartmentName3",
                ShortName = "DepartmentShortName3",
                Comments = "DepartmentComments3"
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
                Id = 1,
                Name = "Name 1",
                ShortName = "Short name 1",
                FloorsCount = 2,
                Address = "Address 1",
                Comments = "Comments 1"
            },
            new HousingDto
            {
                Id = 2,
                Name = "Name 2",
                ShortName = "Short name 2",
                FloorsCount = 1,
                Address = "Address 2",
                Comments = "Comments 2"
            },
            new HousingDto
            {
                Id = 3,
                Name = "Name 3",
                ShortName = "Short name 3",
                FloorsCount = 2,
                Address = "Address 3",
                Comments = "Comments 3"
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
                Id = 1,
                Name = "SectorName1",
                ShortName = "SectorShortName1",
                Comments = "SectorComments1",
                DepartmentId = 1
            },
            new SectorDto
            {
                Id = 2,
                Name = "SectorName2",
                ShortName = "SectorShortName2",
                Comments = "SectorComments2",
                DepartmentId = 2
            },
            new SectorDto
            {
                Id = 3,
                Name = "SectorName3",
                ShortName = "SectorShortName3",
                Comments = "SectorComments3",
                DepartmentId = 3
            }
        ];
        #endregion

        Sectors[0].Department = Departments[0];
        Sectors[1].Department = Departments[1];
        Sectors[2].Department = Departments[2];

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
                Id = 1,
                Name = "RoomName1",
                ShortName = "RoomShortName1",
                Comments = "RoomComments1",
                SectorId = Rooms[0].Sector.Id,
                Floor = 1,
                Length = 5d,
                Width = 4d,
                Height = 2.5,
                HousingId = 3
            },
            new RoomDto
            {
                Id = 2,
                Name = "RoomName2",
                ShortName = "RoomShortName2",
                Comments = "RoomComments2",
                SectorId = Rooms[1].Sector.Id,
                Floor = 1,
                Length = 6d,
                Width = 5d,
                Height = 2.5,
                HousingId = 2
            },
            new RoomDto
            {
                Id = 3,
                Name = "RoomName3",
                ShortName = "RoomShortName3",
                Comments = "RoomComments3",
                SectorId = Rooms[2].Sector.Id,
                Floor = 1,
                Length = 7d,
                Width = 6d,
                Height = 2.5,
                HousingId = 1
            },
            new RoomDto
            {
                Id = 4,
                Name = "RoomName4",
                ShortName = "RoomShortName4",
                Comments = "RoomComments4",
                SectorId = Rooms[3].Sector.Id,
                Floor = 1,
                Length = 6d,
                Width = 5d,
                Height = 2.5,
                HousingId = 2
            },
            new RoomDto
            {
                Id = 5,
                Name = "RoomName5",
                ShortName = "RoomShortName5",
                Comments = "RoomComments5",
                SectorId = Rooms[4].Sector.Id,
                Floor = 1,
                Length = 7d,
                Width = 6d,
                Height = 2.5,
                HousingId = 2
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
                Id = 1,
                FirstName = "FirstName1",
                MiddleName = "MiddleName1",
                LastName = "LastName1",
                BirthdayDate = new DateTime(2000, 10, 15),
                CategoryId = Categories[0].Id,
                Comments = "PersonsComments1",
                PositionId = Positions[0].Id,
                SectorId = Sectors[0].Id,
                Role = Roles[0],
                UserId = Users[0].Id
            },
            new PersonDto
            {
                Id = 2,
                FirstName = "FirstName2",
                MiddleName = "MiddleName2",
                LastName = "LastName2",
                BirthdayDate = new DateTime(2000, 10, 16),
                CategoryId = Categories[1].Id,
                Comments = "PersonsComments2",
                PositionId = Positions[1].Id,
                SectorId = Sectors[0].Id,
                Role = Roles[1],
                UserId = Users[1].Id
            },
            new PersonDto
            {
                Id = 3,
                FirstName = "FirstName3",
                MiddleName = "MiddleName3",
                LastName = "LastName3",
                BirthdayDate = new DateTime(2000, 10, 17),
                CategoryId = Categories[2].Id,
                Comments = "PersonsComments3",
                PositionId = Positions[2].Id,
                SectorId = Sectors[0].Id,
                Role = Roles[2],
                UserId = Users[2].Id
            },
            new PersonDto
            {
                Id = 4,
                FirstName = "FirstName4",
                MiddleName = "MiddleName4",
                LastName = "LastName4",
                BirthdayDate = new DateTime(2000, 10, 18),
                CategoryId = Categories[2].Id,
                Comments = "PersonsComments4",
                PositionId = Positions[2].Id,
                SectorId = Sectors[1].Id,
                Role = Roles[3],
                UserId = Users[3].Id
            },
            new PersonDto
            {
                Id = 5,
                FirstName = "FirstName5",
                MiddleName = "MiddleName5",
                LastName = "LastName5",
                BirthdayDate = new DateTime(2000, 10, 19),
                CategoryId = Categories[1].Id,
                Comments = "PersonsComments5",
                PositionId = Positions[1].Id,
                SectorId = Sectors[1].Id,
                Role = Roles[3],
                UserId = Users[4].Id
            },
            new PersonDto
            {
                Id = 6,
                FirstName = "FirstName6",
                MiddleName = "MiddleName6",
                LastName = "LastName6",
                BirthdayDate = new DateTime(2000, 10, 20),
                CategoryId = Categories[2].Id,
                Comments = "PersonsComments6",
                PositionId = Positions[2].Id,
                SectorId = Sectors[1].Id,
                Role = Roles[4],
                UserId = Users[5].Id
            },
            new PersonDto
            {
                Id = 7,
                FirstName = "FirstName7",
                MiddleName = "MiddleName7",
                LastName = "LastName7",
                BirthdayDate = new DateTime(2000, 10, 21),
                CategoryId = Categories[1].Id,
                Comments = "PersonsComments4",
                PositionId = Positions[2].Id,
                SectorId = Sectors[2].Id,
                Role = Roles[3],
                UserId = Users[6].Id
            },
            new PersonDto
            {
                Id = 8,
                FirstName = "FirstName8",
                MiddleName = "MiddleName8",
                LastName = "LastName8",
                BirthdayDate = new DateTime(2000, 10, 22),
                CategoryId = Categories[2].Id,
                Comments = "PersonsComments8",
                PositionId = Positions[2].Id,
                SectorId = Sectors[2].Id,
                Role = Roles[4],
                UserId = Users[7].Id
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
                ResponsibleId = Persons[0].Id,
                RoomId = Rooms[4].Id,
                ImagePath = Workplaces[0].ImagePath
            },
            new WorkplaceDto
            {
                Id = Workplaces[1].Id,
                Name = Workplaces[1].Name,
                ShortName = Workplaces[1].ShortName,
                Comments = Workplaces[1].Comments,
                ResponsibleId = Persons[1].Id,
                RoomId = Rooms[4].Id,
                ImagePath = Workplaces[1].ImagePath
            },
            new WorkplaceDto
            {
                Id = Workplaces[2].Id,
                Name = Workplaces[2].Name,
                ShortName = Workplaces[2].ShortName,
                Comments = Workplaces[2].Comments,
                ResponsibleId = Persons[2].Id,
                RoomId = Rooms[2].Id,
                ImagePath = Workplaces[2].ImagePath
            },
            new WorkplaceDto
            {
                Id = Workplaces[3].Id,
                Name = Workplaces[3].Name,
                ShortName = Workplaces[3].ShortName,
                Comments = Workplaces[3].Comments,
                ResponsibleId = Persons[3].Id,
                RoomId = Rooms[3].Id,
                ImagePath = Workplaces[3].ImagePath
            },
            new WorkplaceDto
            {
                Id = Workplaces[4].Id,
                Name = Workplaces[4].Name,
                ShortName = Workplaces[4].ShortName,
                Comments = Workplaces[4].Comments,
                ResponsibleId = Persons[4].Id,
                RoomId = Rooms[0].Id,
                ImagePath = Workplaces[4].ImagePath
            },
            new WorkplaceDto
            {
                Id = Workplaces[5].Id,
                Name = Workplaces[5].Name,
                ShortName = Workplaces[5].ShortName,
                Comments = Workplaces[5].Comments,
                ResponsibleId = Persons[5].Id,
                RoomId = Rooms[1].Id,
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
                Id = 1,
                Name = "QuantityName1"
            },
            new QuantityDto
            { 
                Id = 2,
                Name = "QuantityName2"
            },
            new QuantityDto
            {
                Id = 3,
                Name = "QuantityName3"
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
                Id = 1,
                QuantityId = Units[0].Quantity.Id,
                Name = "UnitName1",
                RuName = "UnitRuName1",
                Symbol = "UnitSymbol1",
                RuSymbol = "UnitRuSymbol1"
            },
            new UnitDto
            {
                Id = 2,
                QuantityId = Units[1].Quantity.Id,
                Name = "UnitName2",
                RuName = "UnitRuName2",
                Symbol = "UnitSymbol2",
                RuSymbol = "UnitRuSymbol2"
            },
            new UnitDto
            {
                Id = 3,
                QuantityId = Units[2].Quantity.Id,
                Name = "UnitName3",
                RuName = "UnitRuName3",
                Symbol = "UnitSymbol3",
                RuSymbol = "UnitRuSymbol3"
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
                Id = 1,
                Name = "MaterialName1",
                ShortName = "MaterialShortName1",
                UnitId = Materials[0].Unit.Id,
                Comments = "Comments1"
            },
            new MaterialDto
            {
                Id = 2,
                Name = "MaterialName2",
                ShortName = "MaterialShortName2",
                UnitId = Materials[1].Unit.Id,
                Comments = "Comments2"
            },
            new MaterialDto
            {
                Id = 3,
                Name = "MaterialName3",
                ShortName = "MaterialShortName3",
                UnitId = Materials[2].Unit.Id,
                Comments = "Comments3"
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
                Id = 1,
                Name = Services[0].Name,
                ShortName = Services[0].ShortName,
                Comments = Services[0].Comments,
                ServiceTypeId = Services[0].ServiceType.Id,
                MaterialIds = Services[0].Materials.Select(m => m.Id).ToList(),
                MaterialsQuantityIds = Services[0].MaterialsQuantities.Select(q => q.Id).ToList()
            },
            new ServiceDto
            {
                Id = 2,
                Name = Services[1].Name,
                ShortName = Services[1].ShortName,
                Comments = Services[1].Comments,
                ServiceTypeId = Services[1].ServiceType.Id,
                MaterialIds = Services[1].Materials.Select(m => m.Id).ToList(),
                MaterialsQuantityIds = Services[1].MaterialsQuantities.Select(q => q.Id).ToList()
            },
            new ServiceDto
            {
                Id = 3,
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
                Id = 1,
                Name = "CharacteristicName1",
                ShortName = "CharacteristicShortName1",
                Comments = "Comments1",
                GradeId = Grades[0].Id,
                GradeValue = 3d,
                GradeValueStart = 0d,
                GradeValueEnd = 7d,
                RangeStart = -10d,
                RangeEnd = 10d,
                UnitId = Units[0].Id
            },
            new CharacteristicDto
            {
                Id = 2,
                Name = "CharacteristicName2",
                ShortName = "CharacteristicShortName2",
                Comments = "Comments2",
                GradeId = Grades[1].Id,
                GradeValue = 1d,
                GradeValueStart = 0d,
                GradeValueEnd = 4d,
                RangeStart = -5d,
                RangeEnd = 10d,
                UnitId = Units[1].Id
            },
            new CharacteristicDto
            {
                Id = 3,
                Name = "CharacteristicName3",
                ShortName = "CharacteristicShortName3",
                Comments = "Comments3",
                GradeId = Grades[2].Id,
                GradeValue = 4d,
                GradeValueStart = 0d,
                GradeValueEnd = 10d,
                RangeStart = 0d,
                RangeEnd = 10d,
                UnitId = Units[2].Id
            }
        ];
        #endregion

        #region WebApi
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
}