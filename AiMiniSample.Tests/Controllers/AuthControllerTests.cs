using AiMiniSample.Controllers;
using AiMiniSample.Features.Auth.Commands;
using AiMiniSample.Features.Auth.DTOs;
using AiMiniSample.Features.Auth.Queries;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using FluentAssertions;

namespace AiMiniSample.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AuthController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Register_WithValidRequest_ReturnsOkResult()
    {
        // Arrange
        var request = new RegisterRequest("test@example.com", "password123", "TestUser");
        var expectedResponse = new LoginResponse("token123", new AuthUserResponse("user123", "test@example.com", "TestUser", true));
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedResponse));

        // Act
        var result = await _controller.Register(request);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task Register_WithInvalidRequest_ReturnsNotFoundResult()
    {
        // Arrange
        var request = new RegisterRequest("test@example.com", "password123", "TestUser");
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<LoginResponse>("Registration failed"));

        // Act
        var result = await _controller.Register(request);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult!.Value.Should().Be("Registration failed");
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOkResult()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", "password123");
        var expectedResponse = new LoginResponse("token123", new AuthUserResponse("user123", "test@example.com", "TestUser", true));
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedResponse));

        // Act
        var result = await _controller.Login(request);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsNotFoundResult()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", "wrongpassword");
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<LoginResponse>("Invalid credentials"));

        // Act
        var result = await _controller.Login(request);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult!.Value.Should().Be("Invalid credentials");
    }

    [Fact]
    public async Task GetCurrentUser_WithValidUserId_ReturnsOkResult()
    {
        // Arrange
        var userId = "user123";
        var expectedResponse = new AuthUserResponse("user123", "test@example.com", "TestUser", true);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        };
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCurrentUserQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedResponse));

        // Act
        var result = await _controller.GetCurrentUser();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task GetCurrentUser_WithNoUserId_ReturnsUnauthorized()
    {
        // Arrange
        var claims = new List<Claim>();
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        // Act
        var result = await _controller.GetCurrentUser();

        // Assert
        result.Result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task GetCurrentUser_WhenUserNotFound_ReturnsNotFoundResult()
    {
        // Arrange
        var userId = "nonexistent";
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        };
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCurrentUserQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<AuthUserResponse>("User not found"));

        // Act
        var result = await _controller.GetCurrentUser();

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }
}
