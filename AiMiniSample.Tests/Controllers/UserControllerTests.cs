using AiMiniSample.Controllers;
using AiMiniSample.Features.Users.Commands;
using AiMiniSample.Features.Users.Queries;
using CSharpFunctionalExtensions;
using GeneratedApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;

namespace AiMiniSample.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly TestController _controller;

    public UserControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new TestController(_mediatorMock.Object);
    }

    [Fact]
    public async Task CreateUser_WithValidRequest_ReturnsOkResult()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Id = "user123",
            Name = "TestUser"
        };
        var expectedResponse = new UserResponse
        {
            Id = request.Id,
            Name = request.Name
        };
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedResponse));

        // Act
        var result = await _controller.CreateUser(request);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task CreateUser_WithInvalidRequest_ReturnsNotFoundResult()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Id = "",
            Name = ""
        };
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<UserResponse>("Invalid user data"));

        // Act
        var result = await _controller.CreateUser(request);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult!.Value.Should().Be("Invalid user data");
    }

    [Fact]
    public async Task DeleteUser_WithValidId_CompletesSuccessfully()
    {
        // Arrange
        var userId = "user123";
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await _controller.DeleteUser(userId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsOkResultWithUserList()
    {
        // Arrange
        var expectedResponse = new List<UserResponse>
        {
            new UserResponse { Id = "user1", Name = "User1" },
            new UserResponse { Id = "user2", Name = "User2" }
        };
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success((IEnumerable<UserResponse>)expectedResponse));

        // Act
        var result = await _controller.GetAllUsers();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetAllUsers_WithEmptyList_ReturnsOkResultWithEmptyList()
    {
        // Arrange
        var expectedResponse = new List<UserResponse>();
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success((IEnumerable<UserResponse>)expectedResponse));

        // Act
        var result = await _controller.GetAllUsers();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetUserById_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var userId = "user123";
        var expectedResponse = new UserResponse
        {
            Id = userId,
            Name = "TestUser"
        };
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedResponse));

        // Act
        var result = await _controller.GetUserById(userId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task GetUserById_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        var userId = "nonexistent";
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<UserResponse>("User not found"));

        // Act
        var result = await _controller.GetUserById(userId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult!.Value.Should().Be("User not found");
    }

    [Fact]
    public async Task UpdateUser_WithValidRequest_ReturnsOkResult()
    {
        // Arrange
        var userId = "user123";
        var request = new GeneratedApi.Models.UpdateUserRequest
        {
            Name = "UpdatedUser"
        };
        var expectedResponse = new UserResponse
        {
            Id = userId,
            Name = request.Name
        };
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedResponse));

        // Act
        var result = await _controller.UpdateUser(userId, request);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task UpdateUser_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        var userId = "nonexistent";
        var request = new GeneratedApi.Models.UpdateUserRequest
        {
            Name = "TestUser"
        };
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<UserResponse>("User not found"));

        // Act
        var result = await _controller.UpdateUser(userId, request);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult!.Value.Should().Be("User not found");
    }
}
