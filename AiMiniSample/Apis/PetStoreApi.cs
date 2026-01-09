// using PetStoreClient.Api;
// using PetStoreClient.Model;

namespace AiMiniSample.Apis;

public class PetStoreApi : IPetStoreApi
{
    // private readonly IPetApi _petApi;
    
    // public PetStoreApi(IPetApi petApi)
    // {
    //     _petApi = petApi;
    // }
    
    public async Task<object> GetPetByIdAsync(int petId)
    {
        // Temporary stub
        return await Task.FromResult(new { Id = petId, Name = "Stub" });
    }
}