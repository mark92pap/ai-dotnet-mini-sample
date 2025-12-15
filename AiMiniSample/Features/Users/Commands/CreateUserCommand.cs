using AiMiniSample.Features.Users.Mappers;
using AiMiniSample.Persistence.Repositories;
using CSharpFunctionalExtensions;
using GeneratedApi.Models;
using MediatR;

namespace AiMiniSample.Features.Users.Commands;

public record CreateUserCommand(CreateUserRequest Request) : IRequest<Result<UserResponse>>;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserResponse>>
{
    private readonly IUserRepository _repository;

    public CreateUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = UserMapper.ToDomain(request.Request);

        var result = await _repository.CreateAsync(user, cancellationToken);
        
        if (result.IsFailure)
            return Result.Failure<UserResponse>(result.Error);

        return Result.Success(UserMapper.ToResponse(result.Value));
    }
}
