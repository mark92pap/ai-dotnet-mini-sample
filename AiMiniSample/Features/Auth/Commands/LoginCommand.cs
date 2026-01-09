using AiMiniSample.Database_Tables;
using AiMiniSample.DatabaseContext;
using AiMiniSample.Features.Auth.DTOs;
using AiMiniSample.Features.Auth.Services;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AiMiniSample.Features.Auth.Commands;

public record LoginCommand(LoginRequest Request) : IRequest<Result<LoginResponse>>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly MyDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly PasswordHasher<User> _passwordHasher;

    public LoginCommandHandler(MyDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(request.Request.Email) || string.IsNullOrWhiteSpace(request.Request.Password))
            return Result.Failure<LoginResponse>("Email and password are required");

        // Normalize email
        var normalizedEmail = request.Request.Email.ToLowerInvariant().Trim();

        // Find user
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);

        if (user == null)
            return Result.Failure<LoginResponse>("Invalid email or password");

        // Verify password
        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
            return Result.Failure<LoginResponse>("Invalid email or password");

        // Check if user is active
        if (!user.IsActive)
            return Result.Failure<LoginResponse>("Account is inactive");

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
