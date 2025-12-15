using AiMiniSample.Persistence.Repositories;
using CSharpFunctionalExtensions;
using MediatR;

namespace AiMiniSample.Features.Users.Commands;

public record DeleteUserCommand(string Id) : IRequest<Result>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUserRepository _repository;

    public DeleteUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.Id, cancellationToken);
    }
}
