using AiMiniSample.DatabaseContext;
using CSharpFunctionalExtensions;
using MediatR;

namespace AiMiniSample.Features.Testing.Commands;

public record ClearDbCommand : IRequest<Result>;

public class ClearDbCommandHandler : IRequestHandler<ClearDbCommand, Result>
{
    private readonly MyDbContext _dbContext;

    public ClearDbCommandHandler(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(ClearDbCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Database.EnsureDeletedAsync(cancellationToken);
            await _dbContext.Database.EnsureCreatedAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to clear database: {ex.Message}");
        }
    }
}
