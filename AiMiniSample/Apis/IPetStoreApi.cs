using PetStoreClient.Model;

namespace AiMiniSample.Apis;

public interface IPetStoreApi
{
    Task<Pet> GetPetByIdAsync(int petId);
}