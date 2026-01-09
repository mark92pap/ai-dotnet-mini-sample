using System.ComponentModel.DataAnnotations;
using AiMiniSample.Common;
using AiMiniSample.Features.StoreItems.Commands;
using AiMiniSample.Features.StoreItems.Queries;
using GeneratedApi.Controllers;
using GeneratedApi.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiMiniSample.Controllers;

[AllowAnonymous]
public class StoreItemController : StoreItemApiController
{
    private readonly IMediator _mediator;

    public StoreItemController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<ActionResult<StoreItemResponse>> CreateStoreItem(
        [FromBody] CreateStoreItemRequest createStoreItemRequest,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new CreateStoreItemCommand(createStoreItemRequest), cancellationToken);
        return result.ToWebResult();
    }

    public override async Task DeleteStoreItem(
        [FromRoute(Name = "id"), Required] int id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new DeleteStoreItemCommand(id), cancellationToken);
        
        if (result.IsFailure)
        {
            throw new InvalidOperationException(result.Error);
        }
        
        return;
    }

    public override async Task<ActionResult<List<StoreItemResponse>>> GetAllStoreItems(
        [FromQuery(Name = "category")] string? category,
        [FromQuery(Name = "isActive")] bool? isActive,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new GetAllStoreItemsQuery(category, isActive), cancellationToken);
        return result.ToWebResult();
    }

    public override async Task<ActionResult<StoreItemResponse>> GetStoreItemById(
        [FromRoute(Name = "id"), Required] int id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new GetStoreItemByIdQuery(id), cancellationToken);
        return result.ToWebResult();
    }

    public override async Task<ActionResult<StoreItemResponse>> UpdateStoreItem(
        [FromRoute(Name = "id"), Required] int id,
        [FromBody] UpdateStoreItemRequest updateStoreItemRequest,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new UpdateStoreItemCommand(id, updateStoreItemRequest), cancellationToken);
        return result.ToWebResult();
    }
}
