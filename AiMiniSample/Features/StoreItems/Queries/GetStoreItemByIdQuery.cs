using AiMiniSample.Features.StoreItems.Mappers;
using AiMiniSample.Persistence.Repositories;
using CSharpFunctionalExtensions;
using GeneratedApi.Models;
using MediatR;

namespace AiMiniSample.Features.StoreItems.Queries;

public record GetStoreItemByIdQuery(int Id) : IRequest<Result<StoreItemResponse>>;

public class GetStoreItemByIdQueryHandler : IRequestHandler<GetStoreItemByIdQuery, Result<StoreItemResponse>>
{
    private readonly IStoreItemRepository _repository;

    public GetStoreItemByIdQueryHandler(IStoreItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<StoreItemResponse>> Handle(GetStoreItemByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (result.IsFailure)
            return Result.Failure<StoreItemResponse>(result.Error);

        return Result.Success(StoreItemMapper.ToResponse(result.Value));
    }
}
