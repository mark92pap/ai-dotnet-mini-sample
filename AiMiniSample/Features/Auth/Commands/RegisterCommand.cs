using AiMiniSample.Database_Tables;
using AiMiniSample.DatabaseContext;
using AiMiniSample.Features.Auth.DTOs;
using AiMiniSample.Features.Auth.Services;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AiMiniSample.Features.Auth.Commands;

public record RegisterCommand(RegisterRequest Request) : IRequest<Result<LoginResponse>>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<LoginResponse>>
{
    private readonly MyDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly PasswordHasher<User> _passwordHasher;

    public RegisterCommandHandler(MyDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<Result<LoginResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Validate email
        if (string.IsNullOrWhiteSpace(request.Request.Email))
            return Result.Failure<LoginResponse>("Email is required");

        // Normalize email
        var normalizedEmail = request.Request.Email.ToLowerInvariant().Trim();

        // Check if email already exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);

        if (existingUser != null)
            return Result.Failure<LoginResponse>("Email already registered");

        // Validate password
        if (string.IsNullOrWhiteSpace(request.Request.Password) || request.Request.Password.Length < 6)
            return Result.Failure<LoginResponse>("Password must be at least 6 characters");

        // Validate name
        if (string.IsNullOrWhiteSpace(request.Request.Name))
            return Result.Failure<LoginResponse>("Name is required");

        // Create user
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = normalizedEmail,
            Name = request.Request.Name,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Hash password
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        // Generate token
        var token = _jwtService.GenerateToken(user);

        var response = new LoginResponse(
            AccessToken: token,
            User: new AuthUserResponse(
                Id: user.Id,
                Email: user.Email,
                Name: user.Name,
                IsActive: user.IsActive
            )
        );

        return Result.Success(response);
    }
}
