namespace AiMiniSample.Features.Auth.DTOs;

public record AuthUserResponse(
    string Id,
    string Email,
    string Name,
    bool IsActive
);
