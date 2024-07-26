using AdoptAPet.Controllers;
using AdoptAPet.DTOs.User;
using AdoptAPet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace AdoptAPetUnitTests.ControllerUnitTests;

[TestFixture]
public class AuthControllerUnitTests
{
    private Mock<UserManager<User>> _userManagerMock;
    private Mock<SignInManager<User>> _signInManagerMock;
    private Mock<ILogger<AuthController>> _loggerMock;
    private AuthController _controller;

    [SetUp]
    public void Setup()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        
        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<User>>();
        var optionsMock = new Mock<IOptions<IdentityOptions>>();
        var loggerMock = new Mock<ILogger<SignInManager<User>>>();
        var schemesMock = new Mock<IAuthenticationSchemeProvider>();
        _signInManagerMock = new Mock<SignInManager<User>>(_userManagerMock.Object, 
            contextAccessorMock.Object, 
            userPrincipalFactoryMock.Object, 
            optionsMock.Object, 
            loggerMock.Object,
            schemesMock.Object,
            null);
        
        _loggerMock = new Mock<ILogger<AuthController>>();
        _controller = new AuthController(_userManagerMock.Object, _signInManagerMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task RegisterAsync_ReturnsStatusCode201_IfRegistrationIsSuccessful()
    {
        //Arrange
        var inputData = new RegisterUserRequestDto
        {
            FirstName = "Test Firstname",
            LastName = "Test Lastname",
            Email = "test@email.com",
            Password = "Password!0"
        };
        var expectedData = new User
        {
            Id = "Test UserId",
            FirstName = "Test Firstname",
            LastName = "Test Lastname",
            Email = "test@email.com",
            UserName = "test@email.com"
        };

        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), inputData.Password))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<User, string>((u, p) => u.Id = expectedData.Id);
        
        //Act
        var result = await _controller.RegisterAsync(inputData);
        
        //Assert
        Assert.IsNotNull(result);
        Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
        var returnedData = (result as CreatedAtActionResult).Value as NewUserDto;
        Assert.IsNotNull(returnedData);
        Assert.That(returnedData.Id, Is.EqualTo(expectedData.Id));
        Assert.That(returnedData.FirstName, Is.EqualTo(expectedData.FirstName));
        Assert.That(returnedData.LastName, Is.EqualTo(expectedData.LastName));
        Assert.That(returnedData.Email, Is.EqualTo(expectedData.Email));
        Assert.That(returnedData.UserName, Is.EqualTo(expectedData.UserName));
        Assert.That(returnedData.Token, Is.EqualTo(string.Empty));
        _userManagerMock.Verify(um => um.CreateAsync(It.IsAny<User>(), inputData.Password), Times.Once);
    }

    [Test]
    public async Task RegisterAsync_ReturnsInternalServerError_IfRegistrationFails()
    {
        //Arrange
        var inputData = new RegisterUserRequestDto
        {
            FirstName = "Test Firstname",
            LastName = "Test Lastname",
            Email = "test@email.com",
            Password = "Password!0"
        };
        var errors = new List<IdentityError> { new IdentityError { Description = "Error creating user." } };
        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), inputData.Password))
            .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));
        
        //Act
        var result = await _controller.RegisterAsync(inputData);
        
        //Assert
        Assert.IsNotNull(result);
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(500));
        Assert.That(objectResult.Value, Is.EqualTo(errors));
        _userManagerMock.Verify(um => um.CreateAsync(It.IsAny<User>(), inputData.Password), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_ReturnsStatusCode204_IfDeletionIsSuccessful()
    {
        //Arrange
        var userId = "testUserId";
        var user = new User { Id = userId };
        _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);
        
        //Act
        var result = await _controller.DeleteAsync(userId);
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
    
    [Test]
    public async Task DeleteAsync_ReturnsNotFound_IfUserDoesNotExist()
    {
        // Arrange
        var userId = "nonexistentUserId";
        _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync((User?)null);

        // Act
        var result = await _controller.DeleteAsync(userId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        var notFoundResult = result as NotFoundObjectResult;
        Assert.That(notFoundResult?.Value, Is.EqualTo("The user does not exist"));
    }
    
    [Test]
    public async Task DeleteAsync_ReturnsStatusCode500_IfDeletionFails()
    {
        // Arrange
        var userId = "testUserId";
        var user = new User { Id = userId };
        _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Deletion failed" }));

        // Act
        var result = await _controller.DeleteAsync(userId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.Multiple(() =>
        {
            Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
            Assert.That(objectResult?.Value, Is.EqualTo("Something went wrong, please try again later."));
        });
    }

    [Test]
    public async Task DeleteAsync_ReturnsStatusCode500_IfExceptionIsThrown()
    {
        // Arrange
        var userId = "testUserId";
        _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _controller.DeleteAsync(userId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.Multiple(() =>
        {
            Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
            Assert.That(objectResult?.Value, Is.EqualTo("Unexpected error"));
        });
    }
}