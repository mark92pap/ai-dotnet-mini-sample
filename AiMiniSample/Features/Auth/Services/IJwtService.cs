using AiMiniSample.Database_Tables;

namespace AiMiniSample.Features.Auth.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}
