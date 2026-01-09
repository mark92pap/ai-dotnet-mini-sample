// using PetStoreClient.Model;
using AiMiniSample.Apis.Models;

namespace AiMiniSample.Apis;

public interface IPetStoreApi
{
    Task<PetStoreDto> GetPetByIdAsync(int petId);
}