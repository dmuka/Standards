using Domain.Aggregates.Departments;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Sectors;
using Domain.Aggregates.Workplaces;

namespace Tests.Domain.Aggregates.Sectors;

[TestFixture]
public class SectorTests
{
    private Core.Results.Result<Sector> _sector;
    private DepartmentId _departmentId;
    private PersonId _personId;
    private WorkplaceId _workplaceId;
    
    [SetUp]
    public void Setup()
    {
        _sector = Sector.Create();
        _departmentId = new DepartmentId(Guid.CreateVersion7());
        _personId = new PersonId(Guid.CreateVersion7());
        _workplaceId = new WorkplaceId(Guid.CreateVersion7());
    }
    
    [Test]
    public void CreateSector_ShouldReturnSuccess()
    {
        // Arrange & Act
        var result = _sector;

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Id, Is.Not.Null);
        }
    }
    
    [Test]
    public void CreateSector_WithCertainId_ShouldReturnSuccess()
    {
        // Arrange
        var expectedId = new SectorId(Guid.CreateVersion7());
        
        // Act
        var result = Sector.Create(expectedId);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Id, Is.EqualTo(expectedId));
        }
    }

    [Test]
    public void AddPerson_ShouldAddPersonToSector()
    {
        // Arrange
        var sector = _sector.Value;
        var personId = new PersonId(Guid.NewGuid());

        // Act
        var result = sector.AddPerson(personId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(sector.PersonIds, Contains.Item(personId));
        }
    }

    [Test]
    public void AddPersons_ShouldAddPersonsToSector()
    {
        // Arrange
        var sector = _sector.Value;
        var personId1 = new PersonId(Guid.NewGuid());
        var personId2 = new PersonId(Guid.NewGuid());

        // Act
        var result = sector.AddPersons([personId1, personId2]);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(sector.PersonIds, Has.Count.EqualTo(2));
            Assert.That(sector.PersonIds, Contains.Item(personId1));
            Assert.That(sector.PersonIds, Contains.Item(personId2));
        }
    }

    [Test]
    public void AddPersons_IfSectorContainsOneOfPerson_ShouldReturnFailure()
    {
        // Arrange
        var sector = _sector.Value;
        var personId1 = new PersonId(Guid.NewGuid());
        var personId2 = new PersonId(Guid.NewGuid());
        sector.AddPerson(personId1);

        // Act
        var result = sector.AddPersons([personId1, personId2]);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(SectorErrors.OneOfThePersonAlreadyExist.Code));
            Assert.That(sector.PersonIds, Has.Count.EqualTo(1));
            Assert.That(sector.PersonIds, Contains.Item(personId1));
        }
    }

    [Test]
    public void AddPerson_IfPersonExists_ShouldReturnFailure()
    {
        // Arrange
        var sector = _sector.Value;
        sector.AddPerson(_personId);

        // Act
        var result = sector.AddPerson(_personId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(sector.PersonIds, Has.Count.EqualTo(1));
            Assert.That(sector.PersonIds, Has.Member(_personId));
            Assert.That(result.Error.Code, Is.EqualTo(SectorErrors.PersonAlreadyExist.Code));
        }
    }

    [Test]
    public void RemovePerson_ShouldRemovePersonFromSector()
    {
        // Arrange
        var sector = _sector.Value;
        var personId = new PersonId(Guid.CreateVersion7());
        sector.AddPerson(personId);

        // Act
        var result = sector.RemovePerson(personId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(sector.PersonIds, Does.Not.Contain(personId));
        }
    }

    [Test]
    public void RemovePerson_IfPersonNotExists_ShouldReturnFailure()
    {
        // Arrange
        var sector = _sector.Value;

        // Act
        var result = sector.RemovePerson(_personId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(sector.PersonIds, Has.Count.EqualTo(0));
            Assert.That(result.Error.Code, Is.EqualTo(SectorErrors.PersonNotFound(_personId).Code));
        }
    }

    [Test]
    public void AddWorkplace_ShouldAddWorkplaceToSector()
    {
        // Arrange
        var sector = _sector.Value;
        var workplaceId = new WorkplaceId(Guid.CreateVersion7());

        // Act
        var result = sector.AddWorkplace(workplaceId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(sector.WorkplaceIds, Contains.Item(workplaceId));
        }
    }

    [Test]
    public void AddWorkplaces_ShouldAddWorkplacesToSector()
    {
        // Arrange
        var sector = _sector.Value;
        var workplaceId1 = new WorkplaceId(Guid.CreateVersion7());
        var workplaceId2 = new WorkplaceId(Guid.CreateVersion7());

        // Act
        var result = sector.AddWorkplaces([workplaceId1, workplaceId2]);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(sector.WorkplaceIds, Has.Count.EqualTo(2));
            Assert.That(sector.WorkplaceIds, Contains.Item(workplaceId1));
            Assert.That(sector.WorkplaceIds, Contains.Item(workplaceId2));
        }
    }

    [Test]
    public void AddWorkplaces_IfSectorContainsOneOfWorkplace_ShouldReturnFailure()
    {
        // Arrange
        var sector = _sector.Value;
        var workplaceId1 = new WorkplaceId(Guid.CreateVersion7());
        var workplaceId2 = new WorkplaceId(Guid.CreateVersion7());
        sector.AddWorkplace(workplaceId1);

        // Act
        var result = sector.AddWorkplaces([workplaceId1, workplaceId2]);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(SectorErrors.OneOfTheWorkplaceAlreadyExist.Code));
            Assert.That(sector.WorkplaceIds, Has.Count.EqualTo(1));
            Assert.That(sector.WorkplaceIds, Contains.Item(workplaceId1));
        }
    }

    [Test]
    public void AddWorkplace_IfWorkplaceExists_ShouldReturnFailure()
    {
        // Arrange
        var sector = _sector.Value;
        sector.AddWorkplace(_workplaceId);

        // Act
        var result = sector.AddWorkplace(_workplaceId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(sector.WorkplaceIds, Has.Count.EqualTo(1));
            Assert.That(sector.WorkplaceIds, Has.Member(_workplaceId));
            Assert.That(result.Error.Code, Is.EqualTo(SectorErrors.WorkplaceAlreadyExist.Code));
        }
    }

    [Test]
    public void RemoveWorkplace_ShouldRemoveWorkplaceFromSector()
    {
        // Arrange
        var sector = _sector.Value;
        var workplaceId = new WorkplaceId(Guid.NewGuid());
        sector.AddWorkplace(workplaceId);

        // Act
        var result = sector.RemoveWorkplace(workplaceId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(sector.WorkplaceIds, Does.Not.Contain(workplaceId));
        }
    }

    [Test]
    public void RemoveWorkplace_IfWorkplaceNotExists_ShouldReturnFailure()
    {
        // Arrange
        var sector = _sector.Value;

        // Act
        var result = sector.RemoveWorkplace(_workplaceId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(sector.WorkplaceIds, Has.Count.EqualTo(0));
            Assert.That(result.Error.Code, Is.EqualTo(SectorErrors.WorkplaceNotFound(_workplaceId).Code));
        }
    }

    [Test]
    public void ChangeDepartment_ShouldChangeDepartmentId()
    {
        // Arrange
        var sector = _sector.Value;

        // Act
        var result = sector.ChangeDepartment(_departmentId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(sector.DepartmentId, Is.EqualTo(_departmentId));
        }
    }

    [Test]
    public void ChangeDepartment_IfThisDepartmentAlreadySet_ShouldReturnFailure()
    {
        // Arrange
        var sector = _sector.Value;
        sector.ChangeDepartment(_departmentId);

        // Act
        var result = sector.ChangeDepartment(_departmentId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(sector.DepartmentId, Is.EqualTo(_departmentId));
            Assert.That(result.Error.Code, Is.EqualTo(SectorErrors.ThisDepartmentAlreadySetForThisSector.Code));
        }
    }
}