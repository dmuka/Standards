using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;

namespace Standards.CQRS.Tests.Common;

public abstract class BaseTestFixture
{
    protected static List<Room> Rooms { get; } =
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
    
    protected static List<Department> Departments { get; } =
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

    protected static List<Housing> Housings { get; } =
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
            Id = 1,
            Address = "Address 2",
            Name = "Name 2",
            ShortName = "Short name 2",
            FloorsCount = 1,
            Departments = new List<Department>() { Departments[1] },
            Rooms = new List<Room>() { Rooms[2] },
            Comments = "Comments 1"
                    
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
            Comments = "Comments 1"
                    
        }
    ];

    protected static List<HousingDto> HousingDtos { get; } =
    [
        new HousingDto
        {
            Id = Housings[0].Id,
            Name = Housings[0].Name,
            ShortName = Housings[0].ShortName,
            FloorsCount = Housings[0].FloorsCount,
            Address = Housings[0].Address,
            Comments = Housings[0].Comments,
            DepartmentIds = Housings[0].Departments.Select(d => d.Id).ToList(),
            RoomIds = Housings[0].Rooms.Select(r => r.Id).ToList()
        },
        new HousingDto()
        {
            Id = Housings[1].Id,
            Name = Housings[1].Name,
            ShortName = Housings[1].ShortName,
            FloorsCount = Housings[1].FloorsCount,
            Address = Housings[1].Address,
            Comments = Housings[1].Comments,
            DepartmentIds = Housings[1].Departments.Select(d => d.Id).ToList(),
            RoomIds = Housings[1].Rooms.Select(r => r.Id).ToList()
        },
        new HousingDto()
        {
            Id = Housings[2].Id,
            Name = Housings[2].Name,
            ShortName = Housings[2].ShortName,
            FloorsCount = Housings[2].FloorsCount,
            Address = Housings[2].Address,
            Comments = Housings[2].Comments,
            DepartmentIds = Housings[2].Departments.Select(d => d.Id).ToList(),
            RoomIds = Housings[2].Rooms.Select(r => r.Id).ToList()
        }
    ];

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
    }
}