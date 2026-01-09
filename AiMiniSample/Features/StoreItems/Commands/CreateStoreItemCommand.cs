using AiMiniSample.Features.StoreItems.Mappers;
using AiMiniSample.Persistence.Repositories;
using CSharpFunctionalExtensions;
using GeneratedApi.Models;
using MediatR;

namespace AiMiniSample.Features.StoreItems.Commands;

public record CreateStoreItemCommand(CreateStoreItemRequest Request) : IRequest<Result<StoreItemResponse>>;

public class CreateStoreItemCommandHandler : IRequestHandler<CreateStoreItemCommand, Result<StoreItemResponse>>
{
    private readonly IStoreItemRepository _repository;

    public CreateStoreItemCommandHandler(IStoreItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<StoreItemResponse>> Handle(CreateStoreItemCommand request, CancellationToken cancellationToken)
    {
        // Validate price
        if (request.Request.Price <= 0)
            return Result.Failure<StoreItemResponse>("Price must be greater than 0");

        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Request.Name))
            return Result.Failure<StoreItemResponse>("Name is required");

        if (string.IsNullOrWhiteSpace(request.Request.Category))
            return Result.Failure<StoreItemResponse>("Category is required");

        var item = StoreItemMapper.ToDomain(request.Request);

        var result = await _repository.CreateAsync(item, cancellationToken);
        
        if (result.IsFailure)
            return Result.Failure<StoreItemResponse>(result.Error);

        return Result.Success(StoreItemMapper.ToResponse(result.Value));
    }
}
