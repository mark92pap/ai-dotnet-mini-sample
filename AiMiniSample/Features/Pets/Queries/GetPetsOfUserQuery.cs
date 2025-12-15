using AiMiniSample.Persistence.Repositories;
using CSharpFunctionalExtensions;
using GeneratedApi.Models;
using MediatR;

namespace AiMiniSample.Features.Pets.Queries;

public record GetPetsOfUserQuery(string UserId) : IRequest<Result<List<PetResponse>>>;

public class GetPetsOfUserQueryHandler : IRequestHandler<GetPetsOfUserQuery, Result<List<PetResponse>>>
{
    private readonly IUserRepository _userRepository;

    public GetPetsOfUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<List<PetResponse>>> Handle(GetPetsOfUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user.IsFailure)
            return Result.Failure<List<PetResponse>>("Test user not found");

        var petResponses = user.Value.Pets
            .Select(p => new PetResponse
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToList();

        return Result.Success(petResponses);
    }
}
