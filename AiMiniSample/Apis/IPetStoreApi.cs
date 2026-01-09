// using PetStoreClient.Model;

namespace AiMiniSample.Apis;

public interface IPetStoreApi
{
    Task<object> GetPetByIdAsync(int petId);
}