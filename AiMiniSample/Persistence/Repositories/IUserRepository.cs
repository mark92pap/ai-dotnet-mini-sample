using AiMiniSample.Database_Tables;
using CSharpFunctionalExtensions;

namespace AiMiniSample.Persistence.Repositories;

public interface IUserRepository
{
    Task<Result<User>> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Result<IEnumerable<User>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<User>> CreateAsync(User user, CancellationToken cancellationToken);
    Task<Result<User>> UpdateAsync(User user, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken);
    Task<Result<User>> AddPetToUserAsync(string userId, Pet pet, CancellationToken cancellationToken);
}