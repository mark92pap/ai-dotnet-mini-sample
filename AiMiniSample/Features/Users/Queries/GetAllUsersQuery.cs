using AiMiniSample.Features.Users.Mappers;
using AiMiniSample.Persistence.Repositories;
using CSharpFunctionalExtensions;
using GeneratedApi.Models;
using MediatR;

namespace AiMiniSample.Features.Users.Queries;

public record GetAllUsersQuery : IRequest<Result<IEnumerable<UserResponse>>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserResponse>>>
{
    private readonly IUserRepository _repository;

public GetAllUsersQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync(cancellationToken);
        
        if (result.IsFailure)
            return Result.Failure<IEnumerable<UserResponse>>(result.Error);

        var responses = UserMapper.ToResponseList(result.Value);
        return Result.Success(responses);
    }
}
