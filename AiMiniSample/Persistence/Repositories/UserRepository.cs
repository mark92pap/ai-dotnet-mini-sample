using AiMiniSample.Database_Tables;
using AiMiniSample.DatabaseContext;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace AiMiniSample.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MyDbContext _dbContext;

    public UserRepository(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<User>> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Users
            .Include(u => u.Pets)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        return entity != null 
            ? Result.Success(entity) 
            : Result.Failure<User>("Entity not found");
    }

    public async Task<Result<IEnumerable<User>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _dbContext.Users
            .Include(u => u.Pets)
            .ToListAsync(cancellationToken);
        return Result.Success<IEnumerable<User>>(entities);
    }

    public async Task<Result<User>> CreateAsync(User entity, CancellationToken cancellationToken)
    {
        _dbContext.Users.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success(entity);
    }

    public async Task<Result<User>> UpdateAsync(User entity, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Users
            .Include(u => u.Pets)
            .FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);
        if (existing == null)
        {
            return Result.Failure<User>("Entity not found");
        }

        existing.Name = entity.Name;
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Failure<User>("Entity was modified or deleted. Please retry.");
        }
        return Result.Success(existing);
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Users.FindAsync([id], cancellationToken);
        if (entity == null)
        {
            return Result.Failure("Entity not found");
        }

        _dbContext.Users.Remove(entity);
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Failure("Entity was already deleted. Please refresh and retry.");
        }
        return Result.Success();
    }

    public async Task<Result<User>> AddPetToUserAsync(string userId, Pet pet, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == userId, cancellationToken);

        if (!exists)
            return Result.Failure<User>("User not found");

        pet.UserId = userId;
        _dbContext.Pets.Add(pet);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Failure<User>("Entity was modified or deleted. Please retry.");
        }

        var updated = await _dbContext.Users
            .Include(u => u.Pets)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return updated != null
            ? Result.Success(updated)
            : Result.Failure<User>("User not found");
    }

}