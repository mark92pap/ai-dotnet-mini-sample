using AiMiniSample.Persistence.Repositories;
using CSharpFunctionalExtensions;
using MediatR;

namespace AiMiniSample.Features.StoreItems.Commands;

public record DeleteStoreItemCommand(int Id) : IRequest<Result>;

public class DeleteStoreItemCommandHandler : IRequestHandler<DeleteStoreItemCommand, Result>
{
    private readonly IStoreItemRepository _repository;

    public DeleteStoreItemCommandHandler(IStoreItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteStoreItemCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.Id, cancellationToken);
    }
}
