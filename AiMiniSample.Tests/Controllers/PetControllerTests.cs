using AiMiniSample.Controllers;
using AiMiniSample.Features.Pets.Commands;
using AiMiniSample.Features.Pets.Queries;
using CSharpFunctionalExtensions;
using GeneratedApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;

namespace AiMiniSample.Tests.Controllers;

public class PetControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PetController _controller;

    public PetControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new PetController(_mediatorMock.Object);
    }

    [Fact]
    public async Task AssignPetToUser_WithValidIds_ReturnsOkResult()
    {
        // Arrange
        var userId = "user123";
        var petId = 1;
        var expectedResponse = new UserResponse
        {
            Id = userId,
            Name = "TestUser"
        };
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AssignPetToUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedResponse));

        // Act
        var result = await _controller.AssignPetToUser(userId, petId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task AssignPetToUser_WithInvalidIds_ReturnsNotFoundResult()
    {
        // Arrange
        var userId = "nonexistent";
        var petId = 999;
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AssignPetToUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<UserResponse>("User or pet not found"));

        // Act
        var result = await _controller.AssignPetToUser(userId, petId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult!.Value.Should().Be("User or pet not found");
    }

    [Fact]
    public async Task GetPetsOfUser_WithValidUserId_ReturnsOkResult()
    {
        // Arrange
        var userId = "user123";
        var expectedResponse = new List<PetResponse>
        {
            new PetResponse { Id = 1, Name = "Fluffy" },
            new PetResponse { Id = 2, Name = "Rex" }
        };
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetPetsOfUserQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedResponse));

        // Act
        var result = await _controller.GetPetsOfUser(userId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetPetsOfUser_WithInvalidUserId_ReturnsNotFoundResult()
    {
        // Arrange
        var userId = "nonexistent";
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetPetsOfUserQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<List<PetResponse>>("User not found"));

        // Act
        var result = await _controller.GetPetsOfUser(userId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult!.Value.Should().Be("User not found");
    }

    [Fact]
    public async Task GetPetsOfUser_WithEmptyPetList_ReturnsOkResultWithEmptyList()
    {
        // Arrange
        var userId = "user123";
        var expectedResponse = new List<PetResponse>();
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetPetsOfUserQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedResponse));

        // Act
        var result = await _controller.GetPetsOfUser(userId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedResponse);
    }
}
