using System.Data;
using AdoptAPet.Controllers;
using AdoptAPet.DTOs.Application;
using AdoptAPet.Models;
using AdoptAPet.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdoptAPetUnitTests.ControllerUnitTests;

[TestFixture]
public class ApplicationControllerUnitTests
{
    private Mock<ILogger<ApplicationController>> _loggerMock;
    private Mock<IApplicationRepository> _repositoryMock;
    private ApplicationController _controller;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<ApplicationController>>();
        _repositoryMock = new Mock<IApplicationRepository>();
        _controller = new ApplicationController(_loggerMock.Object, _repositoryMock.Object);
    }

    [Test]
    public async Task GetByUserIdAsync_ReturnsOk_IfRepositoryProvidesData()
    {
        //Arrange
        var userId = "Test UserId";
        var mockData = new List<ApplicationDto>
        {
            new ApplicationDto
            {
                Id = 1,
                UserId = "Test UserId",
                AdvertisementId = 21,
                IsAccepted = null
            },
            new ApplicationDto()
            {
                Id = 22,
                UserId = "Test UserId",
                AdvertisementId = 6,
                IsAccepted = false
            }
        };
        _repositoryMock.Setup(repo => repo.GetByUserAsync(userId)).ReturnsAsync(mockData);
        
        //Act
        var result = await _controller.GetByUserIdAsync(userId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(200));
        var returnedData = actionResult.Value as List<ApplicationDto>;
        Assert.IsNotNull(returnedData);
        Assert.That(returnedData.Count, Is.EqualTo(mockData.Count));
        _repositoryMock.Verify(repo => repo.GetByUserAsync(userId), Times.Once);
    }

    [Test]
    public async Task GetByUserAsync_ReturnsBadRequest_IfUserIdIsInvalid()
    {
        //Arrange
        var nonExistentUserId = "nonexistent";
        _repositoryMock.Setup(repo => repo.GetByUserAsync(nonExistentUserId)).ThrowsAsync(new RowNotInTableException());
        
        //Act
        var result = await _controller.GetByUserIdAsync(nonExistentUserId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(400));
        Assert.That(actionResult.Value.ToString().Contains("Invalid user id."));
        _repositoryMock.Verify(repo => repo.GetByUserAsync(nonExistentUserId), Times.Once);
    }

    [Test]
    public async Task GetByUserAsync_ReturnsInternalServerError_IfUnexpectedExceptionIsThrown()
    {
        //Arrange
        var userId = "test userId";
        var exceptionMessage = "Test error message.";
        _repositoryMock.Setup(repo => repo.GetByUserAsync(userId)).ThrowsAsync(new Exception(exceptionMessage));
        
        //Act
        var result = await _controller.GetByUserIdAsync(userId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(500));
        Assert.That(actionResult.Value.ToString().Contains(exceptionMessage));
        _repositoryMock.Verify(repo => repo.GetByUserAsync(userId), Times.Once);
    }
    
    [Test]
    public async Task GetByIdAsync_ReturnsOk_IfRepositoryProvidesData()
    {
        //Arrange
        var applicationId = 8;
        var mockData = new ApplicationDto
        {
                Id = 8,
                UserId = "Test UserId",
                AdvertisementId = 6,
                IsAccepted = null
        };
        _repositoryMock.Setup(repo => repo.GetByIdAsync(applicationId)).ReturnsAsync(mockData);
        
        //Act
        var result = await _controller.GetByIdAsync(applicationId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(200));
        var returnedData = actionResult.Value as ApplicationDto;
        Assert.IsNotNull(returnedData);
        Assert.That(returnedData.UserId, Is.EqualTo(mockData.UserId));
        Assert.That(returnedData.AdvertisementId, Is.EqualTo(mockData.AdvertisementId));
        Assert.That(returnedData.IsAccepted, Is.EqualTo(mockData.IsAccepted));
        _repositoryMock.Verify(repo => repo.GetByIdAsync(applicationId), Times.Once);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsBadRequest_IfApplicationIdIsInvalid()
    {
        //Arrange
        var nonExistentApplicationId = 9999999;
        _repositoryMock.Setup(repo => repo.GetByIdAsync(nonExistentApplicationId)).ThrowsAsync(new RowNotInTableException());
        
        //Act
        var result = await _controller.GetByIdAsync(nonExistentApplicationId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(400));
        Assert.That(actionResult.Value.ToString().Contains("Invalid application id."));
        _repositoryMock.Verify(repo => repo.GetByIdAsync(nonExistentApplicationId), Times.Once);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsInternalServerError_IfUnexpectedExceptionIsThrown()
    {
        //Arrange
        var applicationId = 7;
        var exceptionMessage = "Test error message.";
        _repositoryMock.Setup(repo => repo.GetByIdAsync(applicationId)).ThrowsAsync(new Exception(exceptionMessage));
        
        //Act
        var result = await _controller.GetByIdAsync(applicationId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(500));
        Assert.That(actionResult.Value.ToString().Contains(exceptionMessage));
        _repositoryMock.Verify(repo => repo.GetByIdAsync(applicationId), Times.Once);
    }
    
    [Test]
    public async Task GetByAdvertisementId_ReturnsOk_IfRepositoryProvidesData()
    {
        //Arrange
        var advertisementId = 10;
        var mockData = new List<ApplicationDto>
        {
            new ApplicationDto
            {
                Id = 1,
                UserId = "Test UserId1",
                AdvertisementId = 10,
                IsAccepted = null
            },
            new ApplicationDto()
            {
                Id = 2,
                UserId = "Test UserId2",
                AdvertisementId = 10,
                IsAccepted = false
            },
            new ApplicationDto()
            {
                Id = 3,
                UserId = "Test UserId3",
                AdvertisementId = 10,
                IsAccepted = true
            }
        };
        _repositoryMock.Setup(repo => repo.GetByAdvertisementAsync(advertisementId)).ReturnsAsync(mockData);
        
        //Act
        var result = await _controller.GetByAdvertisementId(advertisementId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(200));
        var returnedData = actionResult.Value as List<ApplicationDto>;
        Assert.IsNotNull(returnedData);
        Assert.That(returnedData.Count, Is.EqualTo(mockData.Count));
        _repositoryMock.Verify(repo => repo.GetByAdvertisementAsync(advertisementId), Times.Once);
    }

    [Test]
    public async Task GetByAdvertisementId_ReturnsBadRequest_IfAdvertisementIdIsInvalid()
    {
        //Arrange
        var nonExistentAdverisementId = 9999999;
        _repositoryMock.Setup(repo => repo.GetByAdvertisementAsync(nonExistentAdverisementId)).ThrowsAsync(new RowNotInTableException());
        
        //Act
        var result = await _controller.GetByAdvertisementId(nonExistentAdverisementId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(400));
        Assert.That(actionResult.Value.ToString().Contains("Invalid advertisement id."));
        _repositoryMock.Verify(repo => repo.GetByAdvertisementAsync(nonExistentAdverisementId), Times.Once);
    }

    [Test]
    public async Task GetByAdvertisementId_ReturnsInternalServerError_IfUnexpectedExceptionIsThrown()
    {
        //Arrange
        var adverisementId = 7;
        var exceptionMessage = "Test error message.";
        _repositoryMock.Setup(repo => repo.GetByAdvertisementAsync(adverisementId)).ThrowsAsync(new Exception(exceptionMessage));
        
        //Act
        var result = await _controller.GetByAdvertisementId(adverisementId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(500));
        Assert.That(actionResult.Value.ToString().Contains(exceptionMessage));
        _repositoryMock.Verify(repo => repo.GetByAdvertisementAsync(adverisementId), Times.Once);
    }
    
    [Test]
    public async Task CreateAsync_ReturnsOkResult_IfRepositoryReturnsAnApplication()
    {
        //Arrange
        var inputData = new CreateApplicationRequestDto
        {
            UserId = "Test userId",
            AdvertisementId = 5
        };
        var expectedData = new ApplicationDto()
        {
            Id = 16,
            AdvertisementId = 5,
            IsAccepted = null,
            UserId = "Test userId"
        };
        _repositoryMock.Setup(repo => repo.CreateAsync(inputData))
            .ReturnsAsync(new Application
        {
            Id = 16,
            UserId = "Test userId",
            User = new User
            {
                Id = "Test userId",
                Email = "test@email.com",
                FirstName = "Test Firstname",
                LastName = "Test Lastname"
            },
            AdvertisementId = 5,
            Advertisement = new Advertisement
            {
                Id = 5,
                CreatedAt = new DateTime(2024,6,29),
                ExpiresAt = new DateTime(2024,7,15),
                PetId = 11,
                Pet = new Pet
                {
                    Id = 11,
                    Name = "Test Pet",
                    Description = "Test pet description",
                    Gender = Gender.Female,
                    Species = Species.Cat,
                    IsNeutered = false,
                    Birth = new DateTime(2023,04,30),
                    Owner = null,
                    PictureLink = "Test link"
                }
            },
            IsAccepted = null
        });
        
        //Act
        var result = await _controller.CreateAsync(inputData);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(201));
        var returnedData = actionResult.Value as ApplicationDto;
        Assert.IsNotNull(returnedData);
        Assert.That(returnedData.Id, Is.EqualTo(expectedData.Id));
        Assert.That(returnedData.AdvertisementId, Is.EqualTo(expectedData.AdvertisementId));
        Assert.That(returnedData.IsAccepted, Is.EqualTo(expectedData.IsAccepted));
        Assert.That(returnedData.UserId, Is.EqualTo(expectedData.UserId));
        _repositoryMock.Verify(repo => repo.CreateAsync(inputData), Times.Once);
    }

    [Test]
    public async Task CreateAsync_ReturnsBadRequest_IfProvidedUserDoesNotExist()
    {
        //Arrange
        var inputDataWithInvalidUserId = new CreateApplicationRequestDto
        {
            UserId = "Invalid userId",
            AdvertisementId = 5
        };
        _repositoryMock.Setup(repo => repo.CreateAsync(inputDataWithInvalidUserId)).ThrowsAsync(new RowNotInTableException());
        
        //Act
        var result = await _controller.CreateAsync(inputDataWithInvalidUserId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(400));
        Assert.That(actionResult.Value.ToString().Contains("Invalid parameters."));
        _repositoryMock.Verify(repo => repo.CreateAsync(inputDataWithInvalidUserId), Times.Once);
    }
    
    [Test]
    public async Task CreateAsync_ReturnsBadRequest_IfProvidedAdvertisementDoesNotExist()
    {
        //Arrange
        var inputDataWithInvalidAdvertisementId = new CreateApplicationRequestDto
        {
            UserId = "Test userId",
            AdvertisementId = 999999
        };
        _repositoryMock.Setup(repo => repo.CreateAsync(inputDataWithInvalidAdvertisementId)).ThrowsAsync(new RowNotInTableException());
        
        //Act
        var result = await _controller.CreateAsync(inputDataWithInvalidAdvertisementId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(400));
        Assert.That(actionResult.Value.ToString().Contains("Invalid parameters."));
        _repositoryMock.Verify(repo => repo.CreateAsync(inputDataWithInvalidAdvertisementId), Times.Once);
    }
    
    [Test]
    public async Task CreateAsync_ReturnsInternalServerError_IfUnexpectedExceptionIsThrown()
    {
        //Arrange
        var inputData = new CreateApplicationRequestDto
        {
            UserId = "Test userId",
            AdvertisementId = 5
        };
        var exceptionMessage = "Test error message";
        _repositoryMock.Setup(repo => repo.CreateAsync(inputData)).ThrowsAsync(new Exception(exceptionMessage));

        //Act
        var result = await _controller.CreateAsync(inputData);

        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(500));
        Assert.That(actionResult.Value.ToString().Contains(exceptionMessage));
        _repositoryMock.Verify(repo => repo.CreateAsync(inputData), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_ReturnsOk_IfRepositoryReturnsAnApplication()
    {
        //Arrange
        var applicationId = 16;
        var inputData = new UpdateApplicationRequestDto
        {
            IsAccepted = false
        };
        var expectedData = new ApplicationDto
        {
            Id = 16,
            AdvertisementId = 5,
            IsAccepted = false,
            UserId = "Test userId"
        };
        _repositoryMock.Setup(repo => repo.UpdateAsync(applicationId, inputData)).ReturnsAsync(new Application
        {
            Id = 16,
            UserId = "Test userId",
            User = new User
            {
                Id = "Test userId",
                Email = "test@email.com",
                FirstName = "Test Firstname",
                LastName = "Test Lastname"
            },
            AdvertisementId = 5,
            Advertisement = new Advertisement
            {
                Id = 5,
                CreatedAt = new DateTime(2024, 6, 29),
                ExpiresAt = new DateTime(2024, 7, 15),
                PetId = 11,
                Pet = new Pet
                {
                    Id = 11,
                    Name = "Test Pet",
                    Description = "Test pet description",
                    Gender = Gender.Male,
                    Species = Species.Dog,
                    IsNeutered = false,
                    Birth = new DateTime(2023, 04, 30),
                    Owner = null,
                    PictureLink = "Test link"
                }
            },
            IsAccepted = false
        });
        
        //Act
        var result = await _controller.UpdateAsync(applicationId, inputData);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(200));
        var returnedData = actionResult.Value as ApplicationDto;
        Assert.IsNotNull(returnedData);
        Assert.That(returnedData.AdvertisementId, Is.EqualTo(expectedData.AdvertisementId));
        Assert.That(returnedData.IsAccepted, Is.EqualTo(expectedData.IsAccepted));
        Assert.That(returnedData.UserId, Is.EqualTo(expectedData.UserId));
        _repositoryMock.Verify(repo => repo.UpdateAsync(applicationId, inputData), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_ReturnsBadRequest_IfProvidedApplicationDoesNotExist()
    {
        //Arrange
        var nonExistentApplicationId = 99999;
        var inputData = new UpdateApplicationRequestDto
        {
            IsAccepted = true
        };
        _repositoryMock.Setup(repo => repo.UpdateAsync(nonExistentApplicationId, inputData))
            .ThrowsAsync(new RowNotInTableException());
        
        //Act
        var result = await _controller.UpdateAsync(nonExistentApplicationId, inputData);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(400));
        Assert.That(actionResult.Value.ToString().Contains("Invalid application id."));
        _repositoryMock.Verify(repo => repo.UpdateAsync(nonExistentApplicationId, inputData), Times.Once);
    }
    
    [Test]
    public async Task UpdateAsync_ReturnsInternalServerError_IfUnexpectedExceptionIsThrown()
    {
        //Arrange
        var aplicationId = 9;
        var inputData = new UpdateApplicationRequestDto
        {
            IsAccepted = true
        };
        var exceptionMessage = "Test error message";
        _repositoryMock.Setup(repo => repo.UpdateAsync(aplicationId, inputData))
            .ThrowsAsync(new Exception(exceptionMessage));
        
        //Act
        var result = await _controller.UpdateAsync(aplicationId, inputData);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(500));
        Assert.That(actionResult.Value.ToString().Contains(exceptionMessage));
        _repositoryMock.Verify(repo => repo.UpdateAsync(aplicationId, inputData), Times.Once);
    }

    [Test]
    public async Task DelateAsync_ReturnsStatusCode204_IfRepositoryDoesNotThrowException()
    {
        //Arrange
        var applicationId = 18;
        _repositoryMock.Setup(repo => repo.DeleteAsync(applicationId)).Returns(Task.CompletedTask);
        
        //Act
        var result = await _controller.DelateAsync(applicationId);
        
        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<NoContentResult>(result);
        var noContentResult = result as NoContentResult;
        Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
        _repositoryMock.Verify(repo => repo.DeleteAsync(applicationId), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_ReturnsBadRequest_IfProvidedApplicationDoesNotExist()
    {
        //Arrange
        var nonExistentApplicationId = 99999;
        _repositoryMock.Setup(repo => repo.DeleteAsync(nonExistentApplicationId))
            .ThrowsAsync(new RowNotInTableException());
        
        //Act
        var result = await _controller.DelateAsync(nonExistentApplicationId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(400));
        Assert.That(actionResult.Value.ToString().Contains("Invalid application id."));
        _repositoryMock.Verify(repo => repo.DeleteAsync(nonExistentApplicationId), Times.Once);
    }
    
    [Test]
    public async Task DeleteAsync_ReturnsInternalServerError_IfUnexpectedExceptionIsThrown()
    {
        //Arrange
        var applicationId = 9;
        var exceptionMessage = "Test error message";
        _repositoryMock.Setup(repo => repo.DeleteAsync(applicationId))
            .ThrowsAsync(new Exception(exceptionMessage));
        
        //Act
        var result = await _controller.DelateAsync(applicationId);
        
        //Assert
        Assert.IsNotNull(result);
        var actionResult = result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.That(actionResult.StatusCode, Is.EqualTo(500));
        Assert.That(actionResult.Value.ToString().Contains(exceptionMessage));
        _repositoryMock.Verify(repo => repo.DeleteAsync(applicationId), Times.Once);
    }
}