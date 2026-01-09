using System.Security.Claims;
using AiMiniSample.Common;
using AiMiniSample.Features.Auth.Commands;
using AiMiniSample.Features.Auth.DTOs;
using AiMiniSample.Features.Auth.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiMiniSample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new RegisterCommand(request), cancellationToken);
        return result.ToWebResult();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new LoginCommand(request), cancellationToken);
        return result.ToWebResult();
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<AuthUserResponse>> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _mediator.Send(new GetCurrentUserQuery(userId), cancellationToken);
        return result.ToWebResult();
    }
}
