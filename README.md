# AiMiniSample - CRUD API

A minimal .NET 8 Web API with CRUD operations for Users and Pets using Entity Framework Core and SQLite, featuring JWT-based email/password authentication.

## Security Notice

⚠️ **Important**: The JWT secret keys in `appsettings.json` and `appsettings.Development.json` are placeholder values. 

**Before deploying to production:**
- Generate a strong, random secret key (at least 32 characters)
- Store the key in environment variables or a secure secret management system
- Never commit production secrets to source control

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Entity Framework Core CLI Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

## Setup

### 1. Install .NET 8 SDK

Download and install the .NET 8 SDK from [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)

Verify installation:
```bash
dotnet --version
```

### 2. Install EF Core Tools

```bash
dotnet tool install --global dotnet-ef
```

Verify installation:
```bash
dotnet ef
```

### 3. Generate API and Client (Required)

Before restoring NuGet dependencies, generate the OpenAPI server + client code:

```bash
bash ./generate-api.sh
bash ./generate-client.sh
```

On Windows, run the scripts via WSL:

```bash
wsl bash ./generate-api.sh
wsl bash ./generate-client.sh
```

### 4. Restore Dependencies

```bash
dotnet restore
```

### 5. Setup Database

Create and apply database migrations:

```bash
# Navigate to the project folder
cd AiMiniSample

# Update database
dotnet ef database update
```

This will create an `app.db` SQLite database file in the AiMiniSample folder.

### 6. Build the Project

```bash
dotnet build
```

### 7. Run the Application

```bash
dotnet run --project AiMiniSample
```

The API will be available at:
- HTTP: `http://localhost:5170`
- HTTPS: `https://localhost:7285`

## API Endpoints

### Authentication

- **POST** `/api/auth/register` - Register a new user with email and password
  - Request: `{ "email": "user@example.com", "password": "password123", "name": "John Doe" }`
  - Response: `{ "accessToken": "...", "user": { "id": "...", "email": "...", "name": "...", "isActive": true } }`
- **POST** `/api/auth/login` - Login with email and password
  - Request: `{ "email": "user@example.com", "password": "password123" }`
  - Response: `{ "accessToken": "...", "user": { "id": "...", "email": "...", "name": "...", "isActive": true } }`
- **GET** `/api/auth/me` - Get current authenticated user (requires `Authorization: Bearer <token>` header)
  - Response: `{ "id": "...", "email": "...", "name": "...", "isActive": true }`

### Users CRUD Operations

- **GET** `/api/User` - Get all users
- **GET** `/api/User/{id}` - Get user by ID
- **POST** `/api/User` - Create new user
- **PUT** `/api/User/{id}` - Update user
- **DELETE** `/api/User/{id}` - Delete user

### Pets Endpoints

- **GET** `/api/User/{id}/pet` - Get pets of a user
- **POST** `/api/User/{id}/pet/{petId}` - Assign pet to user

### Additional Endpoints

- **POST** `/api/Debug/clear-db` - Clear all users/pets from database

## Testing the API

Use the included `Operations.http` file with the [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) extension for VS Code:

1. Install the REST Client extension
2. Open `Operations.http`
3. Click "Send Request" above any endpoint to test

## Project Structure

```
AiMiniSample/
├── Apis/                 # PetStore client + API wiring
├── Features/             # Use-cases organized by feature
│   ├── Auth/             # Authentication (Register, Login, GetCurrentUser)
│   ├── Users/            # User management (CRUD operations)
│   └── Pets/             # Pet management
├── Controllers/          # API Controllers
├── Database_Tables/      # Entity models
├── DatabaseContext/      # EF Core DbContext
├── Persistence/          # Repository pattern implementation
│   ├── Repositories/     # Repository interfaces and implementations
│   ├── Migrations/       # EF Core database migrations
│   └── DependencyInjection.cs
├── Common/               # Shared extensions and utilities
├── Program.cs            # Application entry point
└── appsettings.json      # Configuration (including JWT settings)
```
