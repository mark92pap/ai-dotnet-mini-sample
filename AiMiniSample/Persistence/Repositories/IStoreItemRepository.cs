using AiMiniSample.Database_Tables;
using CSharpFunctionalExtensions;

namespace AiMiniSample.Persistence.Repositories;

public interface IStoreItemRepository
{
    Task<Result<StoreItem>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<IEnumerable<StoreItem>>> GetAllAsync(string? category, bool? isActive, CancellationToken cancellationToken);
    Task<Result<StoreItem>> CreateAsync(StoreItem item, CancellationToken cancellationToken);
    Task<Result<StoreItem>> UpdateAsync(StoreItem item, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
}
