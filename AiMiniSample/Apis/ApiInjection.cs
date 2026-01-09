// using PetStoreClient.Api;

namespace AiMiniSample.Apis;

public static class ApiInjection
{
    public static IServiceCollection AddApis(this IServiceCollection services)
    {
        services.AddTransient<IPetStoreApi, PetStoreApi>();
        // services.AddTransient<IPetApi, PetApi>(sp => new PetApi("https://petstore.swagger.io/v2"));
        return services;
    }
}