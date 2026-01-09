using AiMiniSample.Database_Tables;
using Microsoft.AspNetCore.Identity;

namespace AiMiniSample.DatabaseContext;

public static class DbSeeder
{
    public static async Task SeedAsync(MyDbContext context, IWebHostEnvironment environment)
    {
        // Only seed in Development or Testing environments to prevent seeding production with known credentials
        var isAllowedEnvironment = environment.IsDevelopment() || environment.IsEnvironment("Testing");
        if (!isAllowedEnvironment)
        {
            return;
        }

        if (context.Users.Any())
        {
            return; // Database already seeded
        }

        var passwordHasher = new PasswordHasher<User>();

        var users = new List<User>
        {
            new User
            {
                Id = "user-1",
                Name = "John Doe",
                Email = "john.doe@example.com",
                PasswordHash = passwordHasher.HashPassword(null!, "Password123!"),
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-30)
            },
            new User
            {
                Id = "user-2",
                Name = "Jane Smith",
                Email = "jane.smith@example.com",
                PasswordHash = passwordHasher.HashPassword(null!, "Password123!"),
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-25)
            },
            new User
            {
                Id = "user-3",
                Name = "Bob Wilson",
                Email = "bob.wilson@example.com",
                PasswordHash = passwordHasher.HashPassword(null!, "Password123!"),
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-20)
            },
            new User
            {
                Id = "user-4",
                Name = "Alice Brown",
                Email = "alice.brown@example.com",
                PasswordHash = passwordHasher.HashPassword(null!, "Password123!"),
                IsActive = false,
                CreatedAt = DateTime.UtcNow.AddDays(-15)
            },
            new User
            {
                Id = "user-5",
                Name = "Charlie Davis",
                Email = "charlie.davis@example.com",
                PasswordHash = passwordHasher.HashPassword(null!, "Password123!"),
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }
}
