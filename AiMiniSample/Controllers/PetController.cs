using System.ComponentModel.DataAnnotations;
using AiMiniSample.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GeneratedApi.Controllers;
using GeneratedApi.Models;
using AiMiniSample.Features.Pets.Queries;
using AiMiniSample.Features.Pets.Commands;

namespace AiMiniSample.Controllers;

[AllowAnonymous]
public class PetController : PetApiController
{
    private readonly IMediator _mediator;

    public PetController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<ActionResult<UserResponse>> AssignPetToUser(
        [FromRoute(Name = "id"), Required] string id,
        [FromRoute(Name = "petId"), Required] int petId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new AssignPetToUserCommand(id, petId), cancellationToken);
        return result.ToWebResult();
    }

    public override async Task<ActionResult<List<PetResponse>>> GetPetsOfUser(
        [FromRoute(Name = "id"), Required] string id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new GetPetsOfUserQuery(id), cancellationToken);
        return result.ToWebResult();
    }
}
