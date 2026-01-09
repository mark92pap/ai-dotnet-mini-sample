using AiMiniSample.Database_Tables;
using AiMiniSample.DatabaseContext;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace AiMiniSample.Persistence.Repositories;

public class StoreItemRepository : IStoreItemRepository
{
    private readonly MyDbContext _context;

    public StoreItemRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<Result<StoreItem>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var item = await _context.StoreItems
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        return item != null
            ? Result.Success(item)
            : Result.Failure<StoreItem>($"Store item with id {id} not found");
    }

    public async Task<Result<IEnumerable<StoreItem>>> GetAllAsync(string? category, bool? isActive, CancellationToken cancellationToken)
    {
        var query = _context.StoreItems.AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(i => i.Category == category);
        }

        if (isActive.HasValue)
        {
            query = query.Where(i => i.IsActive == isActive.Value);
        }

        var items = await query.ToListAsync(cancellationToken);
        return Result.Success<IEnumerable<StoreItem>>(items);
    }

    public async Task<Result<StoreItem>> CreateAsync(StoreItem item, CancellationToken cancellationToken)
    {
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;

        _context.StoreItems.Add(item);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(item);
    }

    public async Task<Result<StoreItem>> UpdateAsync(StoreItem item, CancellationToken cancellationToken)
    {
        var existingItem = await _context.StoreItems
            .FirstOrDefaultAsync(i => i.Id == item.Id, cancellationToken);

        if (existingItem == null)
            return Result.Failure<StoreItem>($"Store item with id {item.Id} not found");

        existingItem.Name = item.Name;
        existingItem.Description = item.Description;
        existingItem.Sku = item.Sku;
        existingItem.Category = item.Category;
        existingItem.Price = item.Price;
        existingItem.Currency = item.Currency;
        existingItem.IsActive = item.IsActive;
        existingItem.StockQuantity = item.StockQuantity;
        existingItem.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(existingItem);
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var item = await _context.StoreItems
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        if (item == null)
            return Result.Failure($"Store item with id {id} not found");

        // Soft delete by setting IsActive to false
        item.IsActive = false;
        item.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
