using AiMiniSample.Database_Tables;
using GeneratedApi.Models;

namespace AiMiniSample.Features.StoreItems.Mappers;

public static class StoreItemMapper
{
    public static StoreItemResponse ToResponse(StoreItem item)
    {
        return new StoreItemResponse
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Sku = item.Sku,
            Category = item.Category,
            Price = item.Price,
            Currency = item.Currency,
            IsActive = item.IsActive,
            StockQuantity = item.StockQuantity,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        };
    }

    public static StoreItem ToDomain(CreateStoreItemRequest request)
    {
        return new StoreItem
        {
            Name = request.Name,
            Description = request.Description,
            Sku = request.Sku,
            Category = request.Category,
            Price = request.Price,
            Currency = request.Currency ?? "USD",
            IsActive = request.IsActive,
            StockQuantity = request.StockQuantity
        };
    }

    public static StoreItem ToDomain(int id, UpdateStoreItemRequest request)
    {
        return new StoreItem
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            Sku = request.Sku,
            Category = request.Category,
            Price = request.Price,
            Currency = request.Currency,
            IsActive = request.IsActive,
            StockQuantity = request.StockQuantity
        };
    }
}
