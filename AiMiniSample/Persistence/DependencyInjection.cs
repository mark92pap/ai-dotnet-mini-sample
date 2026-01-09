using AiMiniSample.DatabaseContext;
using AiMiniSample.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AiMiniSample.Persistence;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Data Source=app.db;Foreign Keys=True";
        
        services.AddDbContext<MyDbContext>(options =>
            options.UseSqlite(connectionString));
        // Register your repositories and other persistence-related services here
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStoreItemRepository, StoreItemRepository>();
    }
}