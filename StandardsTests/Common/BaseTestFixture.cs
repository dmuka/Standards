using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.Core.Models.Persons;
using Standards.Core.Models.Users;
using Standards.CQRS.Tests.Constants;

namespace Standards.CQRS.Tests.Common;

public abstract class BaseTestFixture
{
    protected static List<Room> Rooms { get; private set; }
    
    protected static List<RoomDto> RoomDtos { get; private set; }
    
    protected static List<Sector> Sectors { get; private set; }
    
    protected static List<Category> Categories { get; private set; }
    
    protected static List<Position> Positions { get; private set; }
    
    protected static List<User> Users { get; private set; }
    
    protected static List<Person> Persons { get; private set; }
    
    protected static List<string> Roles { get; private set; }
    
    protected static List<WorkPlace> Workplaces { get; private set; }
    
    protected static List<Department> Departments { get; private set; }

    protected static IList<Housing> Housings { get; private set; }

    protected static List<HousingDto> HousingDtos { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
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
        
        Roles =
        [
            "Admin",
            "HeadOfDepartment",
            "HeadOdSector",
            "Engineer",
            "Technician"
        ];
        
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
                Role = Roles[1],
                User = Users[1]
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
                Role = Roles[4],
                User = Users[7]
            }
        ];
        
        Workplaces =
        [
            new WorkPlace
            {
                Id = 1,
                Name = "WorkPlaceName1",
                ShortName = "WorkPlaceShortName1",
                Comments = "WorkplaceComments1",
                Responcible = Persons[0],
                ImagePath = null
            },
            new WorkPlace
            {
                Id = 2,
                Name = "WorkPlaceName2",
                ShortName = "WorkPlaceShortName2",
                Comments = "WorkplaceComments2",
                Responcible = Persons[1],
                ImagePath = null
            },
            new WorkPlace
            {
                Id = 3,
                Name = "WorkPlaceName3",
                ShortName = "WorkPlaceShortName3",
                Comments = "WorkplaceComments3",
                Responcible = Persons[2],
                ImagePath = null
            },
            new WorkPlace
            {
                Id = 4,
                Name = "WorkPlaceName4",
                ShortName = "WorkPlaceShortName4",
                Comments = "WorkplaceComments4",
                Responcible = Persons[3],
                ImagePath = null
            },
            new WorkPlace
            {
                Id = 5,
                Name = "WorkPlaceName5",
                ShortName = "WorkPlaceShortName5",
                Comments = "WorkplaceComments5",
                Responcible = Persons[4],
                ImagePath = null
            },
            new WorkPlace
            {
                Id = 6,
                Name = "WorkPlaceName6",
                ShortName = "WorkPlaceShortName6",
                Comments = "WorkplaceComments6",
                Responcible = Persons[5],
                ImagePath = null
            }
        ];
        
        Rooms =
        [
            new Room
            {
                Id = 1,
                Name = "RoomName1",
                ShortName = "RoomShortName1",
                Comments = "RoomComments1",
                Floor = 1,
                Length = 5d,
                Width = 4d,
                Height = 2.5,
                WorkPlaces = new List<WorkPlace> { Workplaces[0], Workplaces[1] },
                Persons = new List<Person> { Persons[0], Persons[1] }
            },
            new Room
            {
                Id = 2,
                Name = "RoomName2",
                ShortName = "RoomShortName2",
                Comments = "RoomComments2",
                Floor = 1,
                Length = 6d,
                Width = 5d,
                Height = 2.5,
                WorkPlaces = new List<WorkPlace> { Workplaces[2] },
                Persons = new List<Person> { Persons[2], Persons[3] }
            },
            new Room
            {
                Id = 3,
                Name = "RoomName3",
                ShortName = "RoomShortName3",
                Comments = "RoomComments3",
                Floor = 1,
                Length = 7d,
                Width = 6d,
                Height = 2.5,
                WorkPlaces = new List<WorkPlace> { Workplaces[3] },
                Persons = new List<Person> { Persons[4] }
            },
            new Room
            {
                Id = 4,
                Name = "RoomName4",
                ShortName = "RoomShortName4",
                Comments = "RoomComments4",
                Floor = 1,
                Length = 6d,
                Width = 5d,
                Height = 2.5,
                WorkPlaces = new List<WorkPlace> { Workplaces[4] },
                Persons = new List<Person> { Persons[5], Persons[6] }
            },
            new Room
            {
                Id = 5,
                Name = "RoomName5",
                ShortName = "RoomShortName5",
                Comments = "RoomComments5",
                Floor = 1,
                Length = 7d,
                Width = 6d,
                Height = 2.5,
                WorkPlaces = new List<WorkPlace> { Workplaces[5] },
                Persons = new List<Person> { Persons[7] }
            }
        ];

        Workplaces[0].Room = Rooms[0];
        Workplaces[1].Room = Rooms[1];
        Workplaces[2].Room = Rooms[2];
        Workplaces[3].Room = Rooms[3];
        
        RoomDtos =
        [
            new RoomDto
            {
                Id = 1,
                Name = "RoomName1",
                ShortName = "RoomShortName1",
                Comments = "RoomComments1",
                Floor = 1,
                Length = 5d,
                Width = 4d,
                Height = 2.5,
                HousingId = 3,
                SectorId = 1,
                WorkplaceIds = new List<int> { 1, 2 },
                PersonIds = new List<int> { 1, 2 }
                
            },
            new RoomDto
            {
                Id = 2,
                Name = "RoomName2",
                ShortName = "RoomShortName2",
                Comments = "RoomComments2",
                Floor = 1,
                Length = 6d,
                Width = 5d,
                Height = 2.5,
                HousingId = 2,
                SectorId = 2,
                WorkplaceIds = new List<int> { 3 },
                PersonIds = new List<int> { 2, 3 }
            },
            new RoomDto
            {
                Id = 3,
                Name = "RoomName3",
                ShortName = "RoomShortName3",
                Comments = "RoomComments3",
                Floor = 1,
                Length = 7d,
                Width = 6d,
                Height = 2.5,
                HousingId = 1,
                SectorId = 3,
                WorkplaceIds = new List<int> { 4 },
                PersonIds = new List<int> { 4 }
            },
            new RoomDto
            {
                Id = 4,
                Name = "RoomName4",
                ShortName = "RoomShortName4",
                Comments = "RoomComments4",
                Floor = 1,
                Length = 6d,
                Width = 5d,
                Height = 2.5,
                HousingId = 2,
                SectorId = 3,
                WorkplaceIds = new List<int> { 5 },
                PersonIds = new List<int> { 5, 6 }
            },
            new RoomDto
            {
                Id = 5,
                Name = "RoomName5",
                ShortName = "RoomShortName5",
                Comments = "RoomComments5",
                Floor = 1,
                Length = 7d,
                Width = 6d,
                Height = 2.5,
                HousingId = 2,
                SectorId = 1,
                WorkplaceIds = new List<int> { 6 },
                PersonIds = new List<int> { 7 }
            }
        ];
        
        Sectors =
        [
            new Sector
            {
                Id = 1,
                Name = "SectorName1",
                ShortName = "SectorShortName1",
                Comments = "SectorComments1",
            },
            new Sector
            {
                Id = 2,
                Name = "SectorName2",
                ShortName = "SectorShortName2",
                Comments = "SectorComments2",
            },
            new Sector
            {
                Id = 3,
                Name = "SectorName3",
                ShortName = "SectorShortName3",
                Comments = "SectorComments3",
            }
        ];

        Persons[0].Sector = Sectors[0];
        Persons[1].Sector = Sectors[1];
        Persons[2].Sector = Sectors[2];
        Persons[3].Sector = Sectors[1];
        Persons[4].Sector = Sectors[0];
        Persons[5].Sector = Sectors[2];
        Persons[6].Sector = Sectors[1];
        Persons[7].Sector = Sectors[2];

        Rooms[0].Sector = Sectors[0];
        Rooms[1].Sector = Sectors[1];
        Rooms[2].Sector = Sectors[2];
        Rooms[3].Sector = Sectors[2];
        Rooms[4].Sector = Sectors[0];
        
        Departments =
        [
            new Department
            {
                Id = 1,
                Name = "DepartmentName1",
                ShortName = "DepartmentShortName1",
                Comments = "DepartmentComments1",
            },
            new Department
            {
                Id = 2,
                Name = "DepartmentName2",
                ShortName = "DepartmentShortName2",
                Comments = "DepartmentComments2",
            },
            new Department
            {
                Id = 3,
                Name = "DepartmentName3",
                ShortName = "DepartmentShortName3",
                Comments = "DepartmentComments3",
            }
        ];
        
        Housings = 
        [
            new Housing
            {
                Id = 1,
                Address = "Address 1",
                Name = "Name 1",
                ShortName = "Short name 1",
                FloorsCount = 2,
                Departments = new List<Department>() { Departments[0] },
                Rooms = new List<Room>() { Rooms[0], Rooms[1] },
                Comments = "Comments 1"
                    
            },
            new Housing
            {
                Id = 2,
                Address = "Address 2",
                Name = "Name 2",
                ShortName = "Short name 2",
                FloorsCount = 1,
                Departments = new List<Department>() { Departments[1] },
                Rooms = new List<Room>() { Rooms[2] },
                Comments = "Comments 2"
                    
            },
            new Housing
            {
                Id = 3,
                Address = "Address 3",
                Name = "Name 3",
                ShortName = "Short name 3",
                FloorsCount = 2,
                Departments = new List<Department>() { Departments[0], Departments[2] },
                Rooms = new List<Room>() { Rooms[3], Rooms[4] },
                Comments = "Comments 3"
                    
            }
        ];
        
        Rooms[0].Housing = Housings[2];
        Rooms[1].Housing = Housings[1];
        Rooms[2].Housing = Housings[0];
        Rooms[3].Housing = Housings[1];
        Rooms[4].Housing = Housings[1];
        
        HousingDtos = 
        [
            new HousingDto
            {
                Id = 1,
                Name = "Name 1",
                ShortName = "Short name 1",
                FloorsCount = 2,
                Address = "Address 1",
                Comments = "Comments 1",
                DepartmentIds = Housings[0].Departments.Select(d => d.Id).ToList(),
                RoomIds = Housings[0].Rooms.Select(r => r.Id).ToList()
            },
            new HousingDto()
            {
                Id = 2,
                Name = "Name 2",
                ShortName = "Short name 2",
                FloorsCount = 1,
                Address = "Address 2",
                Comments = "Comments 2",
                DepartmentIds = Housings[1].Departments.Select(d => d.Id).ToList(),
                RoomIds = Housings[1].Rooms.Select(r => r.Id).ToList()
            },
            new HousingDto()
            {
                Id = 3,
                Name = "Name 3",
                ShortName = "Short name 3",
                FloorsCount = 2,
                Address = "Address 3",
                Comments = "Comments 3",
                DepartmentIds = Housings[2].Departments.Select(d => d.Id).ToList(),
                RoomIds = Housings[2].Rooms.Select(r => r.Id).ToList()
            }
        ];
    }
    
    protected static IEnumerable<TestCaseData> NullOrEmptyString()
    {
        yield return new TestCaseData(Cases.Null);
        yield return new TestCaseData(Cases.EmptyString);
    }
    
    protected static IEnumerable<TestCaseData> ZeroOrNegativeId()
    {
        yield return new TestCaseData(Cases.Zero);
        yield return new TestCaseData(Cases.Negative);
    }
}