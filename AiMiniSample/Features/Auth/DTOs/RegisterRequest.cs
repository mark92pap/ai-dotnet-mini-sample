namespace AiMiniSample.Features.Auth.DTOs;

public record RegisterRequest(
    string Email,
    string Password,
    string Name
);
