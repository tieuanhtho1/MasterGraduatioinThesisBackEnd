# Architecture Overview

This project follows a layered architecture pattern with clear separation of concerns:

## Architecture Flow

```
Controller → BusinessLogic → Services → Database (DbContext)
```

## Layer Descriptions

### 1. Controllers Layer
**Location:** `Controllers/`

**Responsibility:** Handle HTTP requests and responses
- Validate basic request format
- Call business logic layer
- Return appropriate HTTP status codes
- Handle authentication/authorization attributes

**Example:** `UserController` handles user-related HTTP endpoints

### 2. Business Logic Layer
**Location:** `BusinessLogic/`

**Responsibility:** Implement business rules and validation
- Enforce business rules
- Coordinate multiple service calls if needed
- Perform complex validations
- Transform data for business requirements

**Examples:**
- `UserBusinessLogic` - Manages user-related business rules
- `AuthBusinessLogic` - Handles authentication business logic
- `FlashCardBusinessLogic` - Manages flashcard business rules
- `FlashCardCollectionBusinessLogic` - Handles collection business rules

### 3. Services Layer
**Location:** `Services/`

**Responsibility:** Direct database operations (CRUD)
- Execute database queries via DbContext
- Perform basic data operations
- Abstract database implementation details
- No business logic, pure data access

**Examples:**
- `UserService` - Database operations for User entity
- `AuthService` - Authentication operations (JWT generation, password hashing) + uses UserService
- `FlashCardService` - Database operations for FlashCard entity
- `FlashCardCollectionService` - Database operations for FlashCardCollection entity

### 4. Data Layer
**Location:** `Data/`

**Responsibility:** Database context and entity configuration
- Define DbContext
- Configure entity relationships
- Define database constraints

## Benefits of This Architecture

1. **Separation of Concerns:** Each layer has a single, well-defined responsibility
2. **Testability:** Each layer can be tested independently
3. **Maintainability:** Changes in one layer don't affect others
4. **Scalability:** Easy to add new features without affecting existing code
5. **Reusability:** Business logic and services can be reused across controllers

## Dependency Injection

All layers are registered in `Program.cs`:

```csharp
// Services Layer (Data Access)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFlashCardService, FlashCardService>();
builder.Services.AddScoped<IFlashCardCollectionService, FlashCardCollectionService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Business Logic Layer
builder.Services.AddScoped<IUserBusinessLogic, UserBusinessLogic>();
builder.Services.AddScoped<IFlashCardBusinessLogic, FlashCardBusinessLogic>();
builder.Services.AddScoped<IFlashCardCollectionBusinessLogic, FlashCardCollectionBusinessLogic>();
builder.Services.AddScoped<IAuthBusinessLogic, AuthBusinessLogic>();
```

## Example Flow

When a user calls `GET /api/user/1`:

1. **Controller** (`UserController.GetUserById`) receives the request
2. **Business Logic** (`UserBusinessLogic.GetUserByIdAsync`) applies business rules
3. **Service** (`UserService.GetUserByIdAsync`) queries the database
4. **Database** (`ApplicationDbContext`) executes SQL query
5. Response flows back through the layers to the client

## Notes

- Each layer depends only on the layer directly below it
- Controllers never directly access the database
- Business logic never directly touches DbContext
- Services are the only layer that interact with the database
