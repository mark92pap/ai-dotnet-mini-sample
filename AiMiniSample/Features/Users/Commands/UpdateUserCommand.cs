using AiMiniSample.Features.Users.Mappers;
using AiMiniSample.Persistence.Repositories;
using CSharpFunctionalExtensions;
using GeneratedApi.Models;
using MediatR;

namespace AiMiniSample.Features.Users.Commands;

public record UpdateUserCommand(string Id, UpdateUserRequest Request) : IRequest<Result<UserResponse>>;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserResponse>>
{
    private readonly IUserRepository _repository;

    public UpdateUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = UserMapper.ToDomain(request.Id, request.Request);

        var result = await _repository.UpdateAsync(user, cancellationToken);
        
        if (result.IsFailure)
            return Result.Failure<UserResponse>(result.Error);

        return Result.Success(UserMapper.ToResponse(result.Value));
    }
}
