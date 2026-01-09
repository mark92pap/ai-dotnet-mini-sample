using AiMiniSample.DatabaseContext;
using AiMiniSample.Features.Auth.DTOs;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiMiniSample.Features.Auth.Queries;

public record GetCurrentUserQuery(string UserId) : IRequest<Result<AuthUserResponse>>;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<AuthUserResponse>>
{
    private readonly MyDbContext _context;

    public GetCurrentUserQueryHandler(MyDbContext context)
    {
        _context = context;
    }

    public async Task<Result<AuthUserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
            return Result.Failure<AuthUserResponse>("User not found");

        var response = new AuthUserResponse(
            Id: user.Id,
            Email: user.Email,
            Name: user.Name,
            IsActive: user.IsActive
        );

        return Result.Success(response);
    }
}
