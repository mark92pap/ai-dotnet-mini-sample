# Collaboration & Onboarding Guide

Welcome to the AiMiniSample project! This guide will help you get started with development.

## Table of Contents

1. [Getting Started](#getting-started)
2. [Development Environment](#development-environment)
3. [Project Structure](#project-structure)
4. [Development Workflow](#development-workflow)
5. [Code Conventions](#code-conventions)
6. [Testing](#testing)
7. [Common Tasks](#common-tasks)
8. [Troubleshooting](#troubleshooting)

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Entity Framework Core CLI](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
- Node.js (for OpenAPI generator)
- Git

### Initial Setup

```bash
# 1. Clone the repository
git clone <repository-url>
cd ai-dotnet-mini-sample

# 2. Generate API code (REQUIRED before restore)
bash ./generate-api.sh
bash ./generate-client.sh

# Windows users:
wsl bash ./generate-api.sh
wsl bash ./generate-client.sh

# 3. Restore dependencies
dotnet restore

# 4. Setup database
cd AiMiniSample
dotnet ef database update
cd ..

# 5. Build and run
dotnet build
dotnet run --project AiMiniSample
```

### Verify Setup

The API should be available at:
- HTTP: `http://localhost:5170`
- HTTPS: `https://localhost:7285`

Test with the included [Operations.http](Operations.http) file using VS Code REST Client extension.

## Development Environment

### Recommended IDE

- **Visual Studio Code** with extensions:
  - C# Dev Kit
  - REST Client
  - GitLens

### Recommended VS Code Settings

```json
{
  "editor.formatOnSave": true,
  "omnisharp.enableRoslynAnalyzers": true
}
```

## Project Structure

```
AiMiniSample/
├── Apis/                    # External API integrations
│   ├── IPetStoreApi.cs      # Interface for PetStore
│   ├── PetStoreApi.cs       # Implementation
│   └── ApiInjection.cs      # DI registration
├── Common/                  # Shared utilities
│   └── Extensions.cs        # Result → ActionResult extensions
├── Controllers/             # API endpoints
│   ├── UserController.cs    # User CRUD
│   ├── PetController.cs     # Pet operations
│   └── DebugController.cs   # Debug endpoints
├── Database_Tables/         # Entity models
│   ├── User.cs
│   └── Pet.cs
├── DatabaseContext/         # EF Core context
│   └── MyDbContext.cs
├── Features/                # CQRS handlers
│   ├── Users/
│   │   ├── Commands/        # CreateUser, UpdateUser, DeleteUser
│   │   ├── Queries/         # GetUserById, GetAllUsers
│   │   └── Mappers/         # UserMapper
│   ├── Pets/
│   │   ├── Commands/        # AssignPetToUser
│   │   └── Queries/         # GetPetsOfUser
│   └── Testing/
│       └── Commands/        # ClearDbCommand
├── Persistence/             # Data access
│   ├── Repositories/
│   │   ├── IUserRepository.cs
│   │   └── UserRepository.cs
│   ├── Migrations/
│   └── DependencyInjection.cs
└── Program.cs               # Application entry point

Apis/                        # Generated code (DO NOT EDIT)
├── Clients/                 # PetStore client
└── Server/                  # API controller base classes

Template/                    # OpenAPI generator templates
└── controller.mustache
```

## Development Workflow

### Adding a New Feature

1. **Define the API** in [openapispec.json](openapispec.json)
2. **Regenerate server code**: `bash ./generate-api.sh`
3. **Create Command/Query** in `Features/<Feature>/`
4. **Create Handler** implementing `IRequestHandler<TRequest, TResult>`
5. **Implement Controller** inheriting from generated base class
6. **Add tests** (if applicable)

### Example: Adding a New Command

```csharp
// 1. Create Command record
public record MyNewCommand(string Data) : IRequest<Result<MyResponse>>;

// 2. Create Handler
public class MyNewCommandHandler : IRequestHandler<MyNewCommand, Result<MyResponse>>
{
    private readonly IMyRepository _repository;

    public MyNewCommandHandler(IMyRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<MyResponse>> Handle(
        MyNewCommand request, 
        CancellationToken cancellationToken)
    {
        // Implementation
    }
}

// 3. Use in Controller
public override async Task<ActionResult<MyResponse>> MyEndpoint(...)
{
    var result = await _mediator.Send(new MyNewCommand(data), cancellationToken);
    return result.ToWebResult();
}
```

### Database Changes

```bash
# Navigate to project
cd AiMiniSample

# Create migration
dotnet ef migrations add <MigrationName>

# Apply migration
dotnet ef database update

# Rollback (if needed)
dotnet ef database update <PreviousMigrationName>
```

## Code Conventions

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Commands | `<Action><Entity>Command` | `CreateUserCommand` |
| Queries | `<Action><Entity>Query` | `GetUserByIdQuery` |
| Handlers | `<Command/Query>Handler` | `CreateUserCommandHandler` |
| Repositories | `I<Entity>Repository` | `IUserRepository` |

### Result Pattern

Always use `Result<T>` for operations that can fail:

```csharp
// Good
public async Task<Result<User>> GetByIdAsync(string id, CancellationToken ct)
{
    var entity = await _dbContext.Users.FindAsync(id, ct);
    return entity != null 
        ? Result.Success(entity) 
        : Result.Failure<User>("Entity not found");
}

// In handlers, convert to response
if (result.IsFailure)
    return Result.Failure<UserResponse>(result.Error);

return Result.Success(UserMapper.ToResponse(result.Value));
```

### Controller Pattern

```csharp
public override async Task<ActionResult<TResponse>> MyEndpoint(
    [FromRoute] string id,
    CancellationToken cancellationToken = default)
{
    var result = await _mediator.Send(new MyCommand(id), cancellationToken);
    return result.ToWebResult();
}
```

## Testing

### Manual Testing

Use [Operations.http](Operations.http) with VS Code REST Client:

```http
### Create User
POST http://localhost:5170/api/User
Content-Type: application/json

{
  "id": "user1",
  "name": "John Doe"
}

### Get User
GET http://localhost:5170/api/User/user1

### Clear Database
POST http://localhost:5170/api/Debug/clear-db
```

### Testing External API Integration

The PetStore API integration uses `https://petstore.swagger.io/v2`. Valid pet IDs can be found at the [Swagger Petstore](https://petstore.swagger.io/).

## Common Tasks

### Regenerate API Code

```bash
# After changing openapispec.json
bash ./generate-api.sh

# After PetStore API changes
bash ./generate-client.sh
```

### Reset Database

```bash
# Option 1: Via API
POST http://localhost:5170/api/Debug/clear-db

# Option 2: Delete and recreate
cd AiMiniSample
rm app.db
dotnet ef database update
```

### Add New Entity

1. Create entity in `Database_Tables/`
2. Add `DbSet<T>` to [`MyDbContext`](AiMiniSample/DatabaseContext/MyDbContext.cs)
3. Configure relationships in `OnModelCreating`
4. Create migration: `dotnet ef migrations add AddNewEntity`
5. Apply: `dotnet ef database update`

### Add New Repository

1. Create interface in `Persistence/Repositories/`
2. Create implementation
3. Register in [`DependencyInjection.cs`](AiMiniSample/Persistence/DependencyInjection.cs)

## Troubleshooting

### Build Errors After Clone

```bash
# Ensure generated code exists
bash ./generate-api.sh
bash ./generate-client.sh
dotnet restore
```

### Database Errors

```bash
# Reset database
cd AiMiniSample
rm app.db app.db-shm app.db-wal
dotnet ef database update
```

### Port Already in Use

Edit [`Properties/launchSettings.json`](AiMiniSample/Properties/launchSettings.json) to change ports.

### PetStore API Errors

- Verify internet connectivity
- Check if `https://petstore.swagger.io/v2` is accessible
- Use valid pet IDs (check Swagger Petstore for available pets)

## Key Files Reference

| File | Purpose |
|------|---------|
| [Program.cs](AiMiniSample/Program.cs) | Application entry point, DI configuration |
| [openapispec.json](openapispec.json) | API specification |
| [MyDbContext.cs](AiMiniSample/DatabaseContext/MyDbContext.cs) | Database context |
| [DependencyInjection.cs](AiMiniSample/Persistence/DependencyInjection.cs) | Persistence DI |
| [ApiInjection.cs](AiMiniSample/Apis/ApiInjection.cs) | External API DI |
| [Extensions.cs](AiMiniSample/Common/Extensions.cs) | Result extensions |

## Getting Help

1. Check this documentation
2. Review [ARCHITECTURE.md](ARCHITECTURE.md) for technical details
3. Examine existing implementations in `Features/` for patterns
4. Check [README.md](README.md) for quick start guide