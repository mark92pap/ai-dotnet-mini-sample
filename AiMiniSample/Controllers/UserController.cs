using System.ComponentModel.DataAnnotations;
using AiMiniSample.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GeneratedApi.Controllers;
using GeneratedApi.Models;
using AiMiniSample.Features.Users.Commands;
using AiMiniSample.Features.Users.Queries;

namespace AiMiniSample.Controllers;

[AllowAnonymous]
public class TestController : UserApiController
{
    private readonly IMediator _mediator;

    public TestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<ActionResult<UserResponse>> CreateUser(
        [FromBody] CreateUserRequest createUserRequest, 
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new CreateUserCommand(createUserRequest), cancellationToken);
        return result.ToWebResult();
    }

    public override async Task DeleteUser(
        [FromRoute(Name = "id"), Required] string id, 
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new DeleteUserCommand(id), cancellationToken);
        return;
    }

    public override async Task<ActionResult<List<UserResponse>>> GetAllUsers(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllUsersQuery(), cancellationToken);
        return result.ToWebResult();
    }

    public override async Task<ActionResult<UserResponse>> GetUserById(
        [FromRoute(Name = "id"), Required] string id, 
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);
        return result.ToWebResult();
    }

    public override async Task<ActionResult<UserResponse>> UpdateUser(
        [FromRoute(Name = "id"), Required] string id, 
        [FromBody] GeneratedApi.Models.UpdateUserRequest updateUserRequest, 
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new UpdateUserCommand(id, updateUserRequest), cancellationToken);
        return result.ToWebResult();
    }

}