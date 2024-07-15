using System.Data;
using AdoptAPet.Controllers;
using AdoptAPet.DTOs.Pet;
using AdoptAPet.Models;
using AdoptAPet.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdoptAPetUnitTests.ControllerUnitTests;

[TestFixture]
public class PetControllerUnitTests
{
    private Mock<ILogger<PetController>>? _loggerMock;
    private Mock<IPetRepository>? _repositoryMock;
    private PetController? _controller;
        
    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<PetController>>();
        _repositoryMock = new Mock<IPetRepository>();
        _controller = new PetController(_loggerMock.Object, _repositoryMock.Object);
    }

    [Test]
    public async Task GetAllAsync_ReturnsOkResult_IfRepositoryProvidesData()
    {
        //Arrange
        var mockData = new List<PetDto>
        {
            new PetDto
            {
                Id = 1,
                Birth = new DateTime(2024, 6, 28),
                Description = "Test pet 1",
                Gender = "Female",
                IsNeutered = true,
                Name = "TestPet1",
                OwnerId = "",
                PictureLink = "any",
                Species = "Cat"
            },
            new PetDto
            {
                Id = 2,
                Birth = new DateTime(2024, 6, 27),
                Description = "Test pet 2",
                Gender = "Male",
                IsNeutered = true,
                Name = "TestPet2",
                OwnerId = "123456789",
                PictureLink = "none",
                Species = "Elephant"
            },
            new PetDto
            {
                Id = 3,
                Birth = new DateTime(2024, 6, 26),
                Description = "Test pet 3",
                Gender = "Female",
                IsNeutered = false,
                Name = "TestPet3",
                OwnerId = "",
                PictureLink = "link",
                Species = "Dog"
            }
        };

        _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(mockData);

        //Act
        var result = await _controller.GetAllAsync();

        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(200));
        var returnedData = actionResult.Value as List<PetDto>;
        Assert.IsNotNull(returnedData);
        Assert.That(returnedData.Count, Is.EqualTo(mockData.Count));
        _repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task GetAllAsync_ReturnsInternalServerError_IfExceptionIsThrown()
    {
        //Arrange
        var exceptionMessage = "Test error message";
        _repositoryMock.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new Exception(exceptionMessage));
        
        //Act
        var result = await _controller.GetAllAsync();
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(500));
        _repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsOkResult_IfRepositoryProvidesData()
    {
        //Arrange
        var petId = 8;
        var mockData = new PetDto
        {
            Id = petId,
            Birth = new DateTime(2024, 6, 28),
            Description = "Test pet 1",
            Gender = "Female",
            IsNeutered = true,
            Name = "TestPet1",
            OwnerId = "",
            PictureLink = "any",
            Species = "Cat"
        };
        _repositoryMock.Setup(repo => repo.GetByIdAsync(petId)).ReturnsAsync(mockData);
        
        //Act
        var result = await _controller.GetByIdAsync(petId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(200));
        var returnedData = actionResult.Value as PetDto;
        Assert.IsNotNull(returnedData);
        Assert.That(returnedData.Id, Is.EqualTo(mockData.Id));
        Assert.That(returnedData.Name, Is.EqualTo(mockData.Name));
        Assert.That(returnedData.Birth, Is.EqualTo(mockData.Birth));
        _repositoryMock.Verify(repo => repo.GetByIdAsync(petId), Times.Once);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNotFoundResult_IfIdDoesNotExist()
    {
        //Arrange
        var nonExistentPetId = 99999;
        _repositoryMock.Setup(repo => repo.GetByIdAsync(nonExistentPetId)).ThrowsAsync(new RowNotInTableException());
        
        //Act
        var result = await _controller.GetByIdAsync(nonExistentPetId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(404));
        Assert.That(actionResult.Value.ToString().Contains("The searched pet does not exist."));
        _repositoryMock.Verify(repo => repo.GetByIdAsync(nonExistentPetId), Times.Once);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsInternalServerError_IfUnexpectedExceptionIsThrown()
    {
        //Arrange
        var petId = 1;
        var exceptionMessage = "Test message.";
        _repositoryMock.Setup(repo => repo.GetByIdAsync(petId)).ThrowsAsync(new Exception(exceptionMessage));
        
        //Act
        var result = await _controller.GetByIdAsync(petId);
        
        //Assert
        Assert.IsNotNull(result);
        var objectResult = result.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.That(objectResult.StatusCode, Is.EqualTo(500));
        Assert.That(objectResult.Value.ToString().Contains(exceptionMessage));
        _repositoryMock.Verify(repo => repo.GetByIdAsync(petId), Times.Once);
    }

    [Test]
    public async Task RegisterPetAsync_ReturnsOkResult_IfRepositoryReturnsAPet()
    {
        //Arrange
        var inputData = new CreatePetRequestDto
        {
            Birth = new DateTime(2024, 01, 01),
            Description = "New test pet",
            Gender = Gender.Male,
            IsNeutered = false,
            Name = "Test Pet 22",
            PictureLink = "Test Link",
            Species = Species.Hamster
        };
        var expectedData = new PetDto()
        {
            Id = 33,
            Birth = new DateTime(2024, 01, 01),
            Description = "New test pet",
            Gender = "Male",
            IsNeutered = false,
            Name = "Test Pet 22",
            PictureLink = "Test Link",
            Species = "Hamster",
            OwnerId = ""
        };
        _repositoryMock.Setup(repo => repo.CreateAsync(inputData))
            .ReturnsAsync(new Pet
        {
            Id = 33,
            Advertisements = new List<Advertisement>(),
            Birth = new DateTime(2024, 01, 01),
            Description = "New test pet",
            Gender = Gender.Male,
            IsNeutered = false,
            Name = "Test Pet 22",
            PictureLink = "Test Link",
            Species = Species.Hamster,
            Owner = null
        });
        
        //Act
        var result = await _controller.RegisterPetAsync(inputData);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(201));
        var returnedData = actionResult.Value as PetDto;
        Assert.IsNotNull(returnedData);
        Assert.That(returnedData.Id, Is.EqualTo(expectedData.Id));
        Assert.That(returnedData.Description, Is.EqualTo(expectedData.Description));
        Assert.That(returnedData.Species, Is.EqualTo(expectedData.Species));
        Assert.That(returnedData.IsNeutered, Is.EqualTo(expectedData.IsNeutered));
        Assert.That(returnedData.Birth, Is.EqualTo(expectedData.Birth));
        Assert.That(returnedData.Gender, Is.EqualTo(expectedData.Gender));
        Assert.That(returnedData.Name, Is.EqualTo(expectedData.Name));
        Assert.That(returnedData.OwnerId, Is.EqualTo(expectedData.OwnerId));
        Assert.That(returnedData.PictureLink, Is.EqualTo(expectedData.PictureLink));
        _repositoryMock.Verify(repo => repo.CreateAsync(inputData), Times.Once);
    }

    [Test]
    public async Task RegisterPetAsync_ReturnsBadRequest_IfModelStateIsInvalid()
    {
        //Arrange
        var invalidInput = new CreatePetRequestDto
        {
            Name = "Tooooooooooooooooooo Long Name For Testing Purposes",
            Birth = new DateTime(2024, 6, 28),
            Description = "Test description",
            Gender = Gender.Female,
            IsNeutered = true,
            PictureLink = "Test link",
            Species = Species.Dog
        };
        var exceptionMessage = "The field Name must be a string or array type with a maximum length of '30'.";
        _repositoryMock.Setup(repo => repo.CreateAsync(invalidInput)).ThrowsAsync(new Exception(exceptionMessage));
        
        //Act
        var result = await _controller.RegisterPetAsync(invalidInput);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(400));
        Assert.That(actionResult.Value.ToString().Contains("Error registering the pet, The field Name must be a string or array type with a maximum length of '30'."));
        _repositoryMock.Verify(repo => repo.CreateAsync(invalidInput), Times.Once);
    }
    
    [Test]
    public async Task RegisterPetAsync_ReturnsBadRequest_IfUnexpectedExceptionIsThrown()
    {
        //Arrange
        var inputData = new CreatePetRequestDto
        {
            Name = "Test pet",
            Birth = new DateTime(2024, 6, 28),
            Description = "Test description",
            Gender = Gender.Female,
            IsNeutered = false,
            PictureLink = "Test link",
            Species = Species.Hamster
        };
        _repositoryMock.Setup(repo => repo.CreateAsync(inputData)).ThrowsAsync(new Exception());
        
        //Act
        var result = await _controller.RegisterPetAsync(inputData);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(400));
        Assert.That(actionResult.Value.ToString().Contains("Error registering the pet"));
        _repositoryMock.Verify(repo => repo.CreateAsync(inputData), Times.Once);
    }

    [Test]
    public async Task UpdatePetAsync_ReturnsOk_IfRepositoryReturnsPet()
    {
        //Arrange
        var inputData = new UpdatePetRequestDto
        {
            Description = "Updated test description",
            IsNeutered = true,
            Name = "Updated test name",
            PictureLink = "Updated test picture link"
        };
        var updatedPet = new Pet
        {
            Id = 8,
            Advertisements = new List<Advertisement>(),
            Birth = new DateTime(2024, 01, 01),
            Description = "Updated test description",
            Gender = Gender.Female,
            IsNeutered = true,
            Name = "Updated test name",
            Owner = null,
            PictureLink = "Updated test picture link",
            Species = Species.Rabbit
        };
        var petId = 8;
        _repositoryMock.Setup(repo => repo.UpdateAsync(petId, inputData)).ReturnsAsync(updatedPet);
        
        //Act
        var result = await _controller.UpdatePetAsync(petId, inputData);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(200));
        var returnedData = actionResult.Value as PetDto;
        Assert.IsNotNull(returnedData);
        Assert.That(returnedData.Id, Is.EqualTo(updatedPet.Id));
        Assert.That(returnedData.Name, Is.EqualTo(updatedPet.Name));
        Assert.That(returnedData.Description, Is.EqualTo(updatedPet.Description));
        Assert.That(returnedData.IsNeutered, Is.EqualTo(updatedPet.IsNeutered));
        Assert.That(returnedData.Birth, Is.EqualTo(updatedPet.Birth));
        Assert.That(returnedData.PictureLink, Is.EqualTo(updatedPet.PictureLink));
        Assert.That(returnedData.Gender, Is.EqualTo(Enum.GetName(typeof(Gender), updatedPet.Gender)));
        Assert.That(returnedData.Species, Is.EqualTo(Enum.GetName(typeof(Species), updatedPet.Species)));
        Assert.That(returnedData.OwnerId, Is.EqualTo(""));
        _repositoryMock.Verify(repo => repo.UpdateAsync(petId, inputData), Times.Once);
    }

    [Test]
    public async Task UpdatePetAsync_ReturnsBadRequest_IfPetDoesNotExist()
    {
        //Arrange
        var nonExistentPetId = 999999;
        var inputData = new UpdatePetRequestDto
        {
            Description = "Updated test description",
            IsNeutered = true,
            Name = "Updated test name",
            PictureLink = "Updated test picture link"
        };
        _repositoryMock.Setup(repo => repo.UpdateAsync(nonExistentPetId, inputData))
            .ThrowsAsync(new RowNotInTableException());
        
        //Act
        var result = await _controller.UpdatePetAsync(nonExistentPetId, inputData);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(400));
        _repositoryMock.Verify(repo => repo.UpdateAsync(nonExistentPetId, inputData), Times.Once);
    }

    [Test]
    public async Task UpdatePetAsync_ReturnsInternalServerError_IfUnexpectedExceptionIsThrown()
    {
        //Arrange
        var petId = 44;
        var inputData = new UpdatePetRequestDto
        {
            Description = "Updated test description",
            IsNeutered = true,
            Name = "Updated test name",
            PictureLink = "Updated test picture link"
        };
        _repositoryMock.Setup(repo => repo.UpdateAsync(petId, inputData))
            .ThrowsAsync(new Exception());
        
        //Act
        var result = await _controller.UpdatePetAsync(petId, inputData);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(500));
        Assert.That(actionResult.Value.ToString().Contains("Something went wrong"));
        _repositoryMock.Verify(repo => repo.UpdateAsync(petId, inputData), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_ReturnsOk_IfRepositoryDoesNotThrowException()
    {
        //Arrange
        var petId = 9;
        _repositoryMock.Setup(repo => repo.DeleteAsync(petId)).Returns(Task.CompletedTask);
        
        //Act
        var result = await _controller.DeleteAsync(petId);
        
        //Assert
        Assert.IsNotNull(result);
        var noContentResult = result as NoContentResult;
        Assert.IsNotNull(noContentResult);
        Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
        _repositoryMock.Verify(repo => repo.DeleteAsync(petId), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_ReturnsBadRequest_IfPetDoesNotExist()
    {
        //Arrange
        var nonExistentPetId = 9999;
        _repositoryMock.Setup(repo => repo.DeleteAsync(nonExistentPetId)).ThrowsAsync(new RowNotInTableException());
        
        //Act
        var result = await _controller.DeleteAsync(nonExistentPetId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(400));
        Assert.That(actionResult.Value.ToString().Contains("Pet does not exist."));
        _repositoryMock.Verify(repo => repo.DeleteAsync(nonExistentPetId), Times.Once);
    }
    
    [Test]
    public async Task DeleteAsync_ReturnsInternalServerError_IfUnexpectedExceptionIsThrown()
    {
        //Arrange
        var petId = 78;
        _repositoryMock.Setup(repo => repo.DeleteAsync(petId)).ThrowsAsync(new Exception());
        
        //Act
        var result = await _controller.DeleteAsync(petId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(500));
        Assert.That(actionResult.Value.ToString().Contains("Something went wrong."));
        _repositoryMock.Verify(repo => repo.DeleteAsync(petId), Times.Once);
    }
    
    
}