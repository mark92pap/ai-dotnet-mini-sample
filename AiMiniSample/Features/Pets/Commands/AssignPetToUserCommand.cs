using AiMiniSample.Apis;
using AiMiniSample.Apis.Models;
using AiMiniSample.Database_Tables;
using AiMiniSample.Features.Users.Mappers;
using AiMiniSample.Persistence.Repositories;
using CSharpFunctionalExtensions;
using GeneratedApi.Models;
using MediatR;

namespace AiMiniSample.Features.Pets.Commands;

public record AssignPetToUserCommand(string UserId, int PetId) : IRequest<Result<UserResponse>>;

public class AssignPetToUserCommandHandler : IRequestHandler<AssignPetToUserCommand, Result<UserResponse>>
{
    private readonly IUserRepository _repository;
    private readonly IPetStoreApi _petStoreApi;

    public AssignPetToUserCommandHandler(IUserRepository repository, IPetStoreApi petStoreApi)
    {
        _repository = repository;
        _petStoreApi = petStoreApi;
    }

    public async Task<Result<UserResponse>> Handle(AssignPetToUserCommand request, CancellationToken cancellationToken)
    {
        // Get pet from pet store API
        PetStoreDto petFromStore;
        try
        {
            petFromStore = await _petStoreApi.GetPetByIdAsync((int)request.PetId);
        }
        catch (Exception ex)
        {
            return Result.Failure<UserResponse>($"Failed to get pet from store: {ex.Message}");
        }

        if (petFromStore == null || string.IsNullOrEmpty(petFromStore.Name))
            return Result.Failure<UserResponse>("Pet not found in store or has no name");

        // Create new pet with the name from the pet store
        var pet = new Pet
        {
            ApiId = request.PetId,
            Name = petFromStore.Name
        };

        var result = await _repository.AddPetToUserAsync(request.UserId, pet, cancellationToken);
        
        if (result.IsFailure)
            return Result.Failure<UserResponse>(result.Error);

        return Result.Success(UserMapper.ToResponse(result.Value));
    }
}
