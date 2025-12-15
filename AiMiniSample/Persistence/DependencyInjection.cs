using AiMiniSample.DatabaseContext;
using AiMiniSample.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AiMiniSample.Persistence;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<MyDbContext>(options =>
            options.UseSqlite("Data Source=app.db;Foreign Keys=True"));
        // Register your repositories and other persistence-related services here
        services.AddScoped<IUserRepository, UserRepository>();
    }
}