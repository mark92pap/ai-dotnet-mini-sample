namespace AiMiniSample.Features.Auth.DTOs;

public record LoginResponse(
    string AccessToken,
    AuthUserResponse User
);
