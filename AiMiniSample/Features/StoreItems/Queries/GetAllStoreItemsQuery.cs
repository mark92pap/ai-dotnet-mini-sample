using AiMiniSample.Features.StoreItems.Mappers;
using AiMiniSample.Persistence.Repositories;
using CSharpFunctionalExtensions;
using GeneratedApi.Models;
using MediatR;

namespace AiMiniSample.Features.StoreItems.Queries;

public record GetAllStoreItemsQuery(string? Category, bool? IsActive) : IRequest<Result<List<StoreItemResponse>>>;

public class GetAllStoreItemsQueryHandler : IRequestHandler<GetAllStoreItemsQuery, Result<List<StoreItemResponse>>>
{
    private readonly IStoreItemRepository _repository;

    public GetAllStoreItemsQueryHandler(IStoreItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<StoreItemResponse>>> Handle(GetAllStoreItemsQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync(request.Category, request.IsActive, cancellationToken);

        if (result.IsFailure)
            return Result.Failure<List<StoreItemResponse>>(result.Error);

        var response = result.Value.Select(StoreItemMapper.ToResponse).ToList();
        return Result.Success(response);
    }
}
