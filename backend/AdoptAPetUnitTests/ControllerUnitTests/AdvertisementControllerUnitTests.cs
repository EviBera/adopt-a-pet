using System.Data;
using AdoptAPet.Controllers;
using AdoptAPet.DTOs.Advertisement;
using AdoptAPet.DTOs.Application;
using AdoptAPet.DTOs.Pet;
using AdoptAPet.Models;
using AdoptAPet.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdoptAPetUnitTests.ControllerUnitTests;

[TestFixture]
public class AdvertisementControllerUnitTests
{
    private Mock<ILogger<AdvertisementController>> _loggerMock;
    private Mock<IAdvertisementRepository> _repositoryMock;
    private AdvertisementController _controller;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<AdvertisementController>>();
        _repositoryMock = new Mock<IAdvertisementRepository>();
        _controller = new AdvertisementController(_loggerMock.Object, _repositoryMock.Object);
    }

    [Test]
    public async Task GetAllAsync_ReturnsOk_IfRepositoryProvidesData()
    {
        //Arrange
        var mockData = new List<AdvertisementDto>
        {
            new AdvertisementDto()
            {
                Id = 1,
                PetDto = new PetDto()
                {
                    Id = 1,
                    Name = "Test Pet1",
                    Description = "Test description 1",
                    Birth = new DateTime(2024, 6, 28),
                    Gender = "Female",
                    IsNeutered = false,
                    OwnerId = "",
                    Species = "Elephant",
                    PictureLink = "Test link 1"
                },
                CreatedAt = new DateTime(2024, 6, 1),
                ExpiresAt = new DateTime(2024, 8, 1),
                Applications = new List<ApplicationDto>()
            },
            new AdvertisementDto()
            {
                Id = 2,
                PetDto = new PetDto()
                {
                    Id = 11,
                    Name = "Test Pet2",
                    Description = "Test description 2",
                    Birth = new DateTime(2024, 6, 28),
                    Gender = "Male",
                    IsNeutered = false,
                    OwnerId = "",
                    Species = "Elephant",
                    PictureLink = "Test link 2"
                },
                CreatedAt = new DateTime(2024, 6, 1),
                ExpiresAt = new DateTime(2024, 8, 1),
                Applications = new List<ApplicationDto>()
            },
            new AdvertisementDto()
            {
                Id = 3,
                PetDto = new PetDto()
                {
                    Id = 14,
                    Name = "Test Pet3",
                    Description = "Test description 3",
                    Birth = new DateTime(2024, 6, 28),
                    Gender = "Female",
                    IsNeutered = false,
                    OwnerId = "1234",
                    Species = "Hamster",
                    PictureLink = "Test link 3"
                },
                CreatedAt = new DateTime(2024, 6, 1),
                ExpiresAt = new DateTime(2024, 8, 1),
                Applications = new List<ApplicationDto>()
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
        var returnedData = actionResult.Value as List<AdvertisementDto>;
        Assert.IsNotNull(returnedData);
        Assert.That(returnedData.Count, Is.EqualTo(mockData.Count));
        _repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task GetAllAsync_ReturnsInternalServerError_IfExceptionIsThrown()
    {
        //Arrange
        var exceptionMessage = "Test error.";
        _repositoryMock.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new Exception(exceptionMessage));

        //Act
        var result = await _controller.GetAllAsync();

        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(500));
        Assert.That(actionResult.Value.ToString().Contains(exceptionMessage));
        _repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsOk_IfRepositoryProvidesData()
    {
        //Arrange
        var advertisementId = 12;
        var mockData = new AdvertisementDto
        {
            Id = 12,
            PetDto = new PetDto()
            {
                Id = 4,
                Name = "Test Pet4",
                Description = "Test description 4",
                Birth = new DateTime(2024, 6, 28),
                Gender = "Female",
                IsNeutered = false,
                OwnerId = "",
                Species = "Hamster",
                PictureLink = "Test link 4"
            },
            CreatedAt = new DateTime(2024, 6, 1),
            ExpiresAt = new DateTime(2024, 8, 1),
            Applications = new List<ApplicationDto>()
        };
        _repositoryMock.Setup(repo => repo.GetByIdAsync(advertisementId)).ReturnsAsync(mockData);

        //Act
        var result = await _controller.GetByIdAsync(advertisementId);

        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(200));
        var returnedData = actionResult.Value as AdvertisementDto;
        Assert.IsNotNull(returnedData);
        Assert.That(returnedData.PetDto, Is.EqualTo(mockData.PetDto));
        Assert.That(returnedData.CreatedAt, Is.EqualTo(mockData.CreatedAt));
        Assert.That(returnedData.ExpiresAt, Is.EqualTo(mockData.ExpiresAt));
        _repositoryMock.Verify(repo => repo.GetByIdAsync(advertisementId), Times.Once);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNotFound_IfAdvertisementDoesNotExist()
    {
        //Arrange
        var nonExistentAdvertisementId = 99999;
        _repositoryMock.Setup(repo => repo.GetByIdAsync(nonExistentAdvertisementId))
            .ThrowsAsync(new RowNotInTableException());
        
        //Act
        var result = await _controller.GetByIdAsync(nonExistentAdvertisementId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(404));
        Assert.That(actionResult.Value.ToString().Contains("The searched advertisement does not exist."));
        _repositoryMock.Verify(repo => repo.GetByIdAsync(nonExistentAdvertisementId), Times.Once);
    }
    
    [Test]
    public async Task GetByIdAsync_ReturnsInternalServerError_IfUnexpectedExceptionIsThrown()
    {
        //Arrange
        var advertisementId = 3;
        var exceptionMessage = "Test error message";
        _repositoryMock.Setup(repo => repo.GetByIdAsync(advertisementId))
            .ThrowsAsync(new Exception(exceptionMessage));
        
        //Act
        var result = await _controller.GetByIdAsync(advertisementId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(500));
        Assert.That(actionResult.Value.ToString().Contains(exceptionMessage));
        _repositoryMock.Verify(repo => repo.GetByIdAsync(advertisementId), Times.Once);
    }

    [Test]
    public async Task GetCurrentAdsAsync_ReturnsOk_IfRepositoryProvidesData()
    {
        var mockData = new List<AdvertisementDto>
        {
            new AdvertisementDto()
            {
                Id = 1,
                PetDto = new PetDto()
                {
                    Id = 1,
                    Name = "Test Pet1",
                    Description = "Test description 1",
                    Birth = new DateTime(2024, 6, 28),
                    Gender = "Female",
                    IsNeutered = false,
                    OwnerId = "",
                    Species = "Elephant",
                    PictureLink = "Test link 1"
                },
                CreatedAt = new DateTime(2024, 6, 1),
                ExpiresAt = new DateTime(2024, 8, 1),
                Applications = new List<ApplicationDto>()
            },
            new AdvertisementDto()
            {
                Id = 2,
                PetDto = new PetDto()
                {
                    Id = 11,
                    Name = "Test Pet2",
                    Description = "Test description 2",
                    Birth = new DateTime(2024, 6, 28),
                    Gender = "Male",
                    IsNeutered = false,
                    OwnerId = "",
                    Species = "Elephant",
                    PictureLink = "Test link 2"
                },
                CreatedAt = new DateTime(2024, 6, 1),
                ExpiresAt = new DateTime(2024, 8, 1),
                Applications = new List<ApplicationDto>()
            }
        };
        _repositoryMock.Setup(repo => repo.GetCurrentAdsAsync()).ReturnsAsync(mockData);
        
        //Act
        var result = await _controller.GetCurrentAdsAsync();

        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(200));
        var returnedData = actionResult.Value as List<AdvertisementDto>;
        Assert.IsNotNull(returnedData);
        Assert.That(returnedData.Count, Is.EqualTo(mockData.Count));
        _repositoryMock.Verify(repo => repo.GetCurrentAdsAsync(), Times.Once);
    }

    [Test]
    public async Task GetCurrentAdsAsync_ReturnsInternalServerError_IfExceptionIsThrown()
    {
        //Arrange
        var exceptionMessage = "Test error message";
        _repositoryMock.Setup(repo => repo.GetCurrentAdsAsync()).ThrowsAsync(new Exception(exceptionMessage));
        
        //Act
        var result = await _controller.GetCurrentAdsAsync();
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(500));
        Assert.That(actionResult.Value.ToString().Contains(exceptionMessage));
        _repositoryMock.Verify(repo => repo.GetCurrentAdsAsync(), Times.Once);
    }

    [Test]
    public async Task CreateAdvertisement_ReturnsStatusCode201_IfRepositoryProvidesData()
    {
        var inputData = new CreateAdvertisementRequestDto
        {
            PetId = 6,
            ExpiresAt = new DateTime(2024, 8, 1)
        };
        var expectedData = new AdvertisementDto
        {
            Id = 8,
            CreatedAt = new DateTime(2028, 6, 28),
            ExpiresAt = new DateTime(2024, 8, 1),
            Applications = new List<ApplicationDto>()
        };
        
        _repositoryMock.Setup(repo => repo.CreateAsync(inputData)).ReturnsAsync( new Advertisement
        {
            Id = 8,
            PetId = 6,
            Pet = new Pet
            {
                Id = 6,
                Name = "Test pet",
                Description = "Test description",
                Birth = new DateTime(2023, 12, 25),
                Gender = Gender.Male,
                Species = Species.Rabbit,
                IsNeutered = false,
                Owner = new User
                {
                    Id = "123456",
                    Email = "test@test.mail",
                    FirstName = "Test Firstname",
                    LastName = "Test Lastname"
                },
                PictureLink = "Test link"
            },
            CreatedAt = new DateTime(2028, 6, 28),
            ExpiresAt = new DateTime(2024, 8, 1),
            Applications = new List<Application>()
        });
        
        //Act
        var result = await _controller.CreateAdvertisement(inputData);
        
        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
        var actionResult = result.Result as CreatedAtActionResult;
        Assert.That(actionResult?.ActionName, Is.EqualTo("GetById"));
        Assert.That(actionResult?.RouteValues["advertisementId"], Is.EqualTo(expectedData.Id));
        var returnedData = actionResult.Value as AdvertisementDto;
        Assert.That(returnedData.ExpiresAt, Is.EqualTo(expectedData.ExpiresAt));
        Assert.That(returnedData.CreatedAt, Is.EqualTo(expectedData.CreatedAt));
        _repositoryMock.Verify(repo => repo.CreateAsync(inputData), Times.Once);
    }

    [Test]
    public async Task CreateAdvertisement_ReturnsBadRequest_IfProvidedPedDoesNotExist()
    {
        //Arrange
        var inputDataWithNonexistentPet = new CreateAdvertisementRequestDto
        {
            PetId = 9999999,
            ExpiresAt = new DateTime(2024, 8, 1)
        };
        _repositoryMock.Setup(repo => repo.CreateAsync(inputDataWithNonexistentPet))
            .ThrowsAsync(new RowNotInTableException());
        
        //Act
        var result = await _controller.CreateAdvertisement(inputDataWithNonexistentPet);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(400));
        Assert.That(actionResult.Value.ToString().Contains("The pet does not exist."));
        _repositoryMock.Verify(repo => repo.CreateAsync(inputDataWithNonexistentPet), Times.Once);
    }

    [Test]
    public async Task CreateAdvertisement_ReturnsInternalServerError_IfUnexpectedExceptionIsThrown()
    {
        //Arrange
        var inputData = new CreateAdvertisementRequestDto
        {
            PetId = 15,
            ExpiresAt = new DateTime(2024, 8, 1)
        };
        var exceptionMessage = "Test error message";
        _repositoryMock.Setup(repo => repo.CreateAsync(inputData))
            .ThrowsAsync(new Exception(exceptionMessage));
        
        //Act
        var result = await _controller.CreateAdvertisement(inputData);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(500));
        Assert.That(actionResult.Value.ToString().Contains(exceptionMessage));
        _repositoryMock.Verify(repo => repo.CreateAsync(inputData), Times.Once);
    }
}