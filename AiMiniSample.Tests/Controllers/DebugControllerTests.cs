using AiMiniSample.Controllers;
using AiMiniSample.Features.Testing.Commands;
using CSharpFunctionalExtensions;
using MediatR;
using Moq;
using FluentAssertions;

namespace AiMiniSample.Tests.Controllers;

public class DebugControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly DebugController _controller;

    public DebugControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new DebugController(_mediatorMock.Object);
    }

    [Fact]
    public async Task ClearDatabase_Successfully_CompletesWithoutError()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<ClearDbCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await _controller.ClearDatabase();

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<ClearDbCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ClearDatabase_CallsMediator_WithCorrectCommand()
    {
        // Arrange
        ClearDbCommand? capturedCommand = null;
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<ClearDbCommand>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Result>, CancellationToken>((cmd, ct) => capturedCommand = cmd as ClearDbCommand)
            .ReturnsAsync(Result.Success());

        // Act
        await _controller.ClearDatabase();

        // Assert
        capturedCommand.Should().NotBeNull();
        capturedCommand.Should().BeOfType<ClearDbCommand>();
    }
}
