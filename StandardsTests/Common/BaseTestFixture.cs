using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.CQRS.Tests.Constants;

namespace Standards.CQRS.Tests.Common;

public abstract class BaseTestFixture
{
    protected static List<Room> Rooms { get; private set; }
    
    protected static List<RoomDto> RoomDtos { get; private set; }
    
    protected static List<Sector> Sectors { get; private set; }
    
    protected static List<Department> Departments { get; private set; }

    protected static IList<Housing> Housings { get; private set; }

    protected static List<HousingDto> HousingDtos { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        Rooms =
        [
            new Room
            {
                Id = 1,
                Name = "RoomName1",
                ShortName = "RoomShortName1",
                Comments = "RoomComments1",
            },
            new Room
            {
                Id = 2,
                Name = "RoomName2",
                ShortName = "RoomShortName2",
                Comments = "RoomComments2"
            },
            new Room
            {
                Id = 3,
                Name = "RoomName3",
                ShortName = "RoomShortName3",
                Comments = "RoomComments3"
            },
            new Room
            {
                Id = 4,
                Name = "RoomName4",
                ShortName = "RoomShortName4",
                Comments = "RoomComments4"
            },
            new Room
            {
                Id = 5,
                Name = "RoomName5",
                ShortName = "RoomShortName5",
                Comments = "RoomComments5"
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
            },
            new RoomDto
            {
                Id = 2,
                Name = "RoomName2",
                ShortName = "RoomShortName2",
                Comments = "RoomComments2"
            },
            new RoomDto
            {
                Id = 3,
                Name = "RoomName3",
                ShortName = "RoomShortName3",
                Comments = "RoomComments3"
            },
            new RoomDto
            {
                Id = 4,
                Name = "RoomName4",
                ShortName = "RoomShortName4",
                Comments = "RoomComments4"
            },
            new RoomDto
            {
                Id = 5,
                Name = "RoomName5",
                ShortName = "RoomShortName5",
                Comments = "RoomComments5"
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