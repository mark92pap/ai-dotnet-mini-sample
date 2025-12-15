using AiMiniSample.Database_Tables;
using GeneratedApi.Models;

namespace AiMiniSample.Features.Users.Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Pets = user.Pets?.Select(p => new PetResponse
            {
                Id = p.Id,
                Name = p.Name
            }).ToList()
        };
    }

    public static IEnumerable<UserResponse> ToResponseList(IEnumerable<User> users)
    {
        return users.Select(ToResponse);
    }

    public static User ToDomain(CreateUserRequest request)
    {
        return new User
        {
            Id = request.Id,
            Name = request.Name
        };
    }

    public static User ToDomain(string id, UpdateUserRequest request)
    {
        return new User
        {
            Id = id,
            Name = request.Name
        };
    }
}
