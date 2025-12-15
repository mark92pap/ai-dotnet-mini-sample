using PetStoreClient.Api;
using PetStoreClient.Model;

namespace AiMiniSample.Apis;

public class PetStoreApi : IPetStoreApi
{
    private readonly IPetApi _petApi;
    
    public PetStoreApi(IPetApi petApi)
    {
        _petApi = petApi;
    }
    
    public async Task<Pet> GetPetByIdAsync(int petId)
    {
        var pet = await _petApi.GetPetByIdAsync(petId);
        return pet;
    }
}