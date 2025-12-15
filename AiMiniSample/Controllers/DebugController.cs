using AiMiniSample.Features.Testing.Commands;
using GeneratedApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace AiMiniSample.Controllers;

[AllowAnonymous]
public class DebugController : DebugApiController
{
    private readonly IMediator _mediator;

    public DebugController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public override async Task ClearDatabase(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new ClearDbCommand(), cancellationToken);
        
        return;
    }
}