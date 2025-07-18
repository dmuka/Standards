using Domain.Aggregates.Rooms;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Workplaces;
using Domain.Aggregates.Sectors;

namespace Tests.Domain.Aggregates.Rooms;

[TestFixture]
public class RoomTests
{
    private Length _length;
    private Width _width;
    private Height _height;
    private Core.Results.Result<Room> _room;
    private SectorId _sectorId;
    private PersonId _personId;
    private WorkplaceId _workplaceId;
    
    [SetUp]
    public void Setup()
    {
        _length = Length.Create(10).Value;
        _width = Width.Create(15).Value;
        _height = Height.Create(8).Value;
        _room = Room.Create(_length, _height, _width);
        _sectorId = new SectorId(Guid.CreateVersion7());
        _personId = new PersonId(Guid.CreateVersion7());
        _workplaceId = new WorkplaceId(Guid.CreateVersion7());
    }
    
    [Test]
    public void CreateRoom_ShouldReturnSuccess()
    {
        // Arrange & Act
        var result = _room;

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Length, Is.EqualTo(_length));
            Assert.That(result.Value.Width, Is.EqualTo(_width));
            Assert.That(result.Value.Height, Is.EqualTo(_height));
            Assert.That(result.Value.Id, Is.Not.Null);
        }
    }
    
    [Test]
    public void CreateRoom_WithCertainId_ShouldReturnSuccess()
    {
        // Arrange
        var expectedId = new RoomId(Guid.CreateVersion7());
        
        // Act
        var result = Room.Create(_length, _height, _width, expectedId);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Length, Is.EqualTo(_length));
            Assert.That(result.Value.Width, Is.EqualTo(_width));
            Assert.That(result.Value.Height, Is.EqualTo(_height));
            Assert.That(result.Value.Id, Is.EqualTo(expectedId));
        }
    }

    [Test]
    public void UpdateRoom_ShouldUpdateProperties()
    {
        // Arrange
        var roomId = _room.Value.Id;
        var room = _room.Value;
        var newLength = Length.Create(12).Value;
        var newWidth = Width.Create(18).Value;
        var newHeight = Height.Create(9).Value;

        // Act
        var result = room.Update(newLength, newWidth, newHeight);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(room.Length, Is.EqualTo(newLength));
            Assert.That(room.Width, Is.EqualTo(newWidth));
            Assert.That(room.Height, Is.EqualTo(newHeight));
            Assert.That(room.Id, Is.EqualTo(roomId));
        }
    }

    [Test]
    public void AddPerson_ShouldAddPersonToRoom()
    {
        // Arrange
        var room = _room.Value;
        var personId = new PersonId(Guid.NewGuid());

        // Act
        var result = room.AddPerson(personId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(room.PersonIds, Contains.Item(personId));
        }
    }

    [Test]
    public void AddPersons_ShouldAddPersonsToRoom()
    {
        // Arrange
        var room = _room.Value;
        var personId1 = new PersonId(Guid.NewGuid());
        var personId2 = new PersonId(Guid.NewGuid());

        // Act
        var result = room.AddPersons([personId1, personId2]);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(room.PersonIds, Has.Count.EqualTo(2));
            Assert.That(room.PersonIds, Contains.Item(personId1));
            Assert.That(room.PersonIds, Contains.Item(personId2));
        }
    }

    [Test]
    public void AddPersons_IfRoomContainsOneOfPerson_ShouldReturnFailure()
    {
        // Arrange
        var room = _room.Value;
        var personId1 = new PersonId(Guid.NewGuid());
        var personId2 = new PersonId(Guid.NewGuid());
        room.AddPerson(personId1);

        // Act
        var result = room.AddPersons([personId1, personId2]);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(RoomErrors.OneOfThePersonAlreadyExist.Code));
            Assert.That(room.PersonIds, Has.Count.EqualTo(1));
            Assert.That(room.PersonIds, Contains.Item(personId1));
        }
    }

    [Test]
    public void AddPerson_IfPersonExists_ShouldReturnFailure()
    {
        // Arrange
        var room = _room.Value;
        room.AddPerson(_personId);

        // Act
        var result = room.AddPerson(_personId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(room.PersonIds, Has.Count.EqualTo(1));
            Assert.That(room.PersonIds, Has.Member(_personId));
            Assert.That(result.Error.Code, Is.EqualTo(RoomErrors.PersonAlreadyExist.Code));
        }
    }

    [Test]
    public void RemovePerson_ShouldRemovePersonFromRoom()
    {
        // Arrange
        var room = _room.Value;
        var personId = new PersonId(Guid.CreateVersion7());
        room.AddPerson(personId);

        // Act
        var result = room.RemovePerson(personId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(room.PersonIds, Does.Not.Contain(personId));
        }
    }

    [Test]
    public void RemovePerson_IfPersonNotExists_ShouldReturnFailure()
    {
        // Arrange
        var room = _room.Value;

        // Act
        var result = room.RemovePerson(_personId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(room.PersonIds, Has.Count.EqualTo(0));
            Assert.That(result.Error.Code, Is.EqualTo(RoomErrors.PersonNotFound(_personId).Code));
        }
    }

    [Test]
    public void AddWorkplace_ShouldAddWorkplaceToRoom()
    {
        // Arrange
        var room = _room.Value;
        var workplaceId = new WorkplaceId(Guid.CreateVersion7());

        // Act
        var result = room.AddWorkplace(workplaceId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(room.WorkplaceIds, Contains.Item(workplaceId));
        }
    }

    [Test]
    public void AddWorkplaces_ShouldAddWorkplacesToRoom()
    {
        // Arrange
        var room = _room.Value;
        var workplaceId1 = new WorkplaceId(Guid.CreateVersion7());
        var workplaceId2 = new WorkplaceId(Guid.CreateVersion7());

        // Act
        var result = room.AddWorkplaces([workplaceId1, workplaceId2]);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(room.WorkplaceIds, Has.Count.EqualTo(2));
            Assert.That(room.WorkplaceIds, Contains.Item(workplaceId1));
            Assert.That(room.WorkplaceIds, Contains.Item(workplaceId2));
        }
    }

    [Test]
    public void AddWorkplaces_IfRoomContainsOneOfWorkplace_ShouldReturnFailure()
    {
        // Arrange
        var room = _room.Value;
        var workplaceId1 = new WorkplaceId(Guid.CreateVersion7());
        var workplaceId2 = new WorkplaceId(Guid.CreateVersion7());
        room.AddWorkplace(workplaceId1);

        // Act
        var result = room.AddWorkplaces([workplaceId1, workplaceId2]);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(RoomErrors.OneOfTheWorkplaceAlreadyExist.Code));
            Assert.That(room.WorkplaceIds, Has.Count.EqualTo(1));
            Assert.That(room.WorkplaceIds, Contains.Item(workplaceId1));
        }
    }

    [Test]
    public void AddWorkplace_IfWorkplaceExists_ShouldReturnFailure()
    {
        // Arrange
        var room = _room.Value;
        room.AddWorkplace(_workplaceId);

        // Act
        var result = room.AddWorkplace(_workplaceId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(room.WorkplaceIds, Has.Count.EqualTo(1));
            Assert.That(room.WorkplaceIds, Has.Member(_workplaceId));
            Assert.That(result.Error.Code, Is.EqualTo(RoomErrors.WorkplaceAlreadyExist.Code));
        }
    }

    [Test]
    public void RemoveWorkplace_ShouldRemoveWorkplaceFromRoom()
    {
        // Arrange
        var room = _room.Value;
        var workplaceId = new WorkplaceId(Guid.NewGuid());
        room.AddWorkplace(workplaceId);

        // Act
        var result = room.RemoveWorkplace(workplaceId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(room.WorkplaceIds, Does.Not.Contain(workplaceId));
        }
    }

    [Test]
    public void RemoveWorkplace_IfWorkplaceNotExists_ShouldReturnFailure()
    {
        // Arrange
        var room = _room.Value;

        // Act
        var result = room.RemoveWorkplace(_workplaceId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(room.WorkplaceIds, Has.Count.EqualTo(0));
            Assert.That(result.Error.Code, Is.EqualTo(RoomErrors.WorkplaceNotFound(_workplaceId).Code));
        }
    }

    [Test]
    public void ChangeSector_ShouldChangeSectorId()
    {
        // Arrange
        var room = _room.Value;

        // Act
        var result = room.ChangeSector(_sectorId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(room.SectorId, Is.EqualTo(_sectorId));
        }
    }

    [Test]
    public void ChangeSector_IfThisSectorAlreadySet_ShouldReturnFailure()
    {
        // Arrange
        var room = _room.Value;
        room.ChangeSector(_sectorId);

        // Act
        var result = room.ChangeSector(_sectorId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(room.SectorId, Is.EqualTo(_sectorId));
            Assert.That(result.Error.Code, Is.EqualTo(RoomErrors.ThisSectorAlreadySetForThisRoom.Code));
        }
    }
}