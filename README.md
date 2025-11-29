# WebAPI - JWT Authentication with Entity Framework Core

## Features
- JWT Bearer Token Authentication
- **Role-Based Authorization (User and Admin roles)**
- User Registration and Login
- Protected API Endpoints
- **Controllers-based API**
- Entity Framework Core with SQL Server
- Password Hashing with BCrypt

## Configuration

### Database Connection
Update the connection string in `appsettings.json` or `appsettings.Development.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=WebAPIDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

For SQL Server with username/password:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=WebAPIDb;User Id=your_user;Password=your_password;TrustServerCertificate=True;"
}
```

### JWT Settings
The JWT secret key is configured in `appsettings.json`. **Important**: Change this to a secure random key in production!

## Database Setup

1. Create the initial migration (if starting fresh):
```powershell
dotnet ef migrations add InitialCreate
```

2. Or if you already have the initial migration, add the role migration:
```powershell
dotnet ef migrations add AddUserRole
```

3. Update the database:
```powershell
dotnet ef database update
```

## Running the Application

```powershell
dotnet run
```

The API will be available at `https://localhost:5001` (or the port shown in the console).

## API Endpoints

### Authentication Endpoints (AuthController)

#### Register
- **POST** `/api/auth/register`
- Body:
```json
{
  "username": "john_doe",
  "email": "john@example.com",
  "password": "SecurePassword123!",
  "role": 0
}
```
- Role: 0 = User (default), 1 = Admin
- Response: Returns JWT token and user info with role

#### Login
- **POST** `/api/auth/login`
- Body:
```json
{
  "username": "john_doe",
  "password": "SecurePassword123!"
}
```
- Response: Returns JWT token and user info

#### Logout
- **POST** `/api/auth/logout`
- Requires: Bearer Token in Authorization header
- Response: Success message

### User Endpoints (UserController)

#### Get User Profile
- **GET** `/api/user/profile`
- Requires: Bearer Token in Authorization header
- Authorization: User or Admin
- Response: Returns current user information with role

#### Get All Users
- **GET** `/api/user`
- Requires: Bearer Token in Authorization header
- Authorization: **Admin only**
- Response: Returns list of all users

#### Get User By ID
- **GET** `/api/user/{id}`
- Requires: Bearer Token in Authorization header
- Authorization: **Admin only**
- Response: Returns specific user information

#### Delete User
- **DELETE** `/api/user/{id}`
- Requires: Bearer Token in Authorization header
- Authorization: **Admin only**
- Response: Success message

#### Update User Role
- **PATCH** `/api/user/{id}/role`
- Requires: Bearer Token in Authorization header
- Authorization: **Admin only**
- Body:
```json
{
  "role": 1
}
```
- Response: Updated user information

### Weather Forecast (Example)
- **GET** `/weatherforecast`
- No authentication required
- Response: Sample weather data

## Using Authentication

After logging in or registering, use the returned token in the Authorization header:

```
Authorization: Bearer your_jwt_token_here
```

In Swagger UI:
1. Click the "Authorize" button at the top
2. Enter: `Bearer your_jwt_token_here`
3. Click "Authorize"

## Project Structure

```
WebAPI/
├── Controllers/
│   ├── AuthController.cs           # Authentication endpoints
│   └── UserController.cs            # User management endpoints (role-based)
├── Data/
│   └── ApplicationDbContext.cs      # EF Core DbContext
├── Models/
│   ├── User.cs                      # User entity with Role
│   ├── UserRole.cs                  # Role enum (User, Admin)
│   └── DTOs/
│       ├── RegisterRequest.cs       # Registration DTO with Role
│       ├── LoginRequest.cs          # Login DTO
│       └── AuthResponse.cs          # Auth response DTO with Role
├── Services/
│   ├── IAuthService.cs              # Auth service interface
│   └── AuthService.cs               # Auth service implementation
├── Migrations/                       # EF Core migrations
├── Program.cs                        # Application entry point
└── appsettings.json                  # Configuration
```

## Dependencies
- Microsoft.AspNetCore.Authentication.JwtBearer (8.0.11)
- Microsoft.EntityFrameworkCore.SqlServer (8.0.11)
- Microsoft.EntityFrameworkCore.Tools (10.0.0)
- Microsoft.EntityFrameworkCore.Design (8.0.11)
- BCrypt.Net-Next (4.0.3)

## Role-Based Authorization

The API supports two roles:
- **User (0)**: Default role for registered users, can access their own profile
- **Admin (1)**: Full access to all endpoints including user management

Admin-only endpoints:
- GET `/api/user` - List all users
- GET `/api/user/{id}` - Get user by ID
- DELETE `/api/user/{id}` - Delete user
- PATCH `/api/user/{id}/role` - Update user role

To create an admin user, register with `"role": 1` in the request body, or use the Update User Role endpoint as an existing admin.
