using AiMiniSample.Features.Users.Mappers;
using AiMiniSample.Persistence.Repositories;
using CSharpFunctionalExtensions;
using GeneratedApi.Models;
using MediatR;

namespace AiMiniSample.Features.Users.Queries;

public record GetUserByIdQuery(string Id) : IRequest<Result<UserResponse>>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserResponse>>
{
    private readonly IUserRepository _repository;

    public GetUserByIdQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (result.IsFailure)
            return Result.Failure<UserResponse>(result.Error);

        return Result.Success(UserMapper.ToResponse(result.Value));
    }
}
