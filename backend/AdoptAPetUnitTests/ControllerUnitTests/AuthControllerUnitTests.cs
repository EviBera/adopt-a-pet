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
    public async Task RegisterAsync_ReturnsOk_IfRegistrationIsSuccessful()
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
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var returnedData = (result as OkObjectResult).Value as NewUserDto;
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
}