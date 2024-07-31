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
    private ILogger<AuthController> _logger;
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
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddConsole()
                .AddDebug();
        });
        _logger = loggerFactory.CreateLogger<AuthController>();
        
        _controller = new AuthController(_userManagerMock.Object, _signInManagerMock.Object, _logger)
        {
        };
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
            Password = "Password!0",
            IsStaff = false
        };
        var expectedData = new User
        {
            Id = "Test UserId",
            FirstName = "Test Firstname",
            LastName = "Test Lastname",
            Email = "test@email.com",
            UserName = "test@email.com"
        };
        
        var expectedRoles = new List<string> { "User" };

        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), inputData.Password))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<User, string>((u, p) => u.Id = expectedData.Id);
        
        _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        
        _userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(expectedRoles);
        
        _controller.ModelState.Clear();
        
        //Act
        var result = await _controller.RegisterAsync(inputData);
        
        //Assert
        Assert.IsNotNull(result);
        Assert.That(result, Is.TypeOf<CreatedAtActionResult>(), $"Expected CreatedAtActionResult, but got {result.GetType()}");
        var createdAtActionResult = result as CreatedAtActionResult;
        var returnedData = createdAtActionResult?.Value as NewUserDto;
        Assert.IsNotNull(returnedData, "Returned data is null");
        Assert.That(returnedData.Id, Is.EqualTo(expectedData.Id));
        Assert.That(returnedData.FirstName, Is.EqualTo(expectedData.FirstName));
        Assert.That(returnedData.LastName, Is.EqualTo(expectedData.LastName));
        Assert.That(returnedData.Email, Is.EqualTo(expectedData.Email));
        Assert.That(returnedData.UserName, Is.EqualTo(expectedData.UserName));
        Assert.That(returnedData.Role, Is.EqualTo(expectedRoles[0]));
        _userManagerMock.Verify(um => um.CreateAsync(It.IsAny<User>(), inputData.Password), Times.Once);
        _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        _userManagerMock.Verify(um => um.GetRolesAsync(It.IsAny<User>()), Times.Once);
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
    public async Task RegisterAsync_ReturnsBadRequest_IfModelStateIsInvalid()
    {
        // Arrange
        var inputData = new RegisterUserRequestDto
        {
            FirstName = "Test Firstname",
            LastName = "Test Lastname",
            Email = "test@email.com",
            Password = "Password!0",
            IsStaff = false
        };

        _controller.ModelState.AddModelError("Email", "Email is required");

        // Act
        var result = await _controller.RegisterAsync(inputData);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>(), $"Expected BadRequestObjectResult, but got {result.GetType()}");

        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult?.Value, "BadRequest result value is null");

        var modelState = badRequestResult?.Value as SerializableError;
        Assert.IsTrue(modelState?.ContainsKey("Email"), "ModelState does not contain expected error key");
    }

    [Test]
    public async Task RegisterAsync_ReturnsInternalServerError_IfRoleAssignmentFails()
    {
        // Arrange
        var inputData = new RegisterUserRequestDto
        {
            FirstName = "Test Firstname",
            LastName = "Test Lastname",
            Email = "test@email.com",
            Password = "Password!0",
            IsStaff = false
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

        _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Role assignment failed" }));

        _controller.ModelState.Clear();

        // Act
        var result = await _controller.RegisterAsync(inputData);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result, Is.TypeOf<ObjectResult>(), $"Expected ObjectResult, but got {result.GetType()}");

        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500), "Expected status code 500");
        Assert.IsNotNull(objectResult?.Value, "ObjectResult value is null");

        var errors = objectResult?.Value as IEnumerable<IdentityError>;
        Assert.IsNotNull(errors, "Errors should not be null");
        Assert.IsTrue(errors?.Any(e => e.Description == "Role assignment failed"), "Expected error description not found");
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
    
    [Test]
    public async Task GetByIdAsync_ReturnsUser_IfUserExists()
    {
        // Arrange
        var userId = "testUserId";
        var user = new User { Id = userId, UserName = "testUser" };
        _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await _controller.GetByIdAsync(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult?.Value);
        Assert.That(okResult?.Value, Is.EqualTo(user));
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNotFound_IfUserDoesNotExist()
    {
        // Arrange
        var userId = "nonexistentUserId";
        _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync((User?)null);

        // Act
        var result = await _controller.GetByIdAsync(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GetByIdAsync_ReturnsStatusCode500_IfExceptionIsThrown()
    {
        // Arrange
        var userId = "testUserId";
        _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _controller.GetByIdAsync(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Result, Is.TypeOf<ObjectResult>());
        var objectResult = result.Result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
        Assert.That(objectResult?.Value, Is.EqualTo("Unexpected error"));
    }
    
    [Test]
    public async Task LogoutAsync_ReturnsOk_IfLogoutIsSuccessful()
    {
        // Act
        var result = await _controller.LogoutAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result, Is.TypeOf<OkResult>());
        _signInManagerMock.Verify(sm => sm.SignOutAsync(), Times.Once);
    }
}