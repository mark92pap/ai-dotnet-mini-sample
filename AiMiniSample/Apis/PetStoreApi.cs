// using PetStoreClient.Api;
// using PetStoreClient.Model;
using AiMiniSample.Apis.Models;

namespace AiMiniSample.Apis;

public class PetStoreApi : IPetStoreApi
{
    // private readonly IPetApi _petApi;
    
    // public PetStoreApi(IPetApi petApi)
    // {
    //     _petApi = petApi;
    // }
    
    public async Task<PetStoreDto> GetPetByIdAsync(int petId)
    {
        // Temporary stub - returns a mock PetStoreDto
        // In production, this would call the external PetStore API
        return await Task.FromResult(new PetStoreDto 
        { 
            Id = petId, 
            Name = $"Pet {petId}",
            Status = "available"
        });
    }
}