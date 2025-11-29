# Role-Based Authorization Quick Reference

## Overview
The API now supports two roles: **User** and **Admin** with proper role-based authorization implemented using controllers.

## User Roles

### UserRole Enum
```csharp
public enum UserRole
{
    User = 0,   // Default role
    Admin = 1   // Administrator role
}
```

## API Structure

### AuthController (`/api/auth`)
- **POST** `/api/auth/register` - Register new user (public)
- **POST** `/api/auth/login` - User login (public)
- **POST** `/api/auth/logout` - Logout (authenticated)

### UserController (`/api/user`)
- **GET** `/api/user/profile` - Get current user profile (User/Admin)
- **GET** `/api/user` - Get all users (**Admin only**)
- **GET** `/api/user/{id}` - Get user by ID (**Admin only**)
- **DELETE** `/api/user/{id}` - Delete user (**Admin only**)
- **PATCH** `/api/user/{id}/role` - Update user role (**Admin only**)

## Authorization Attributes

### Public Endpoints (No Authorization)
```csharp
[HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterRequest request)
```

### Authenticated Endpoints (Any logged-in user)
```csharp
[HttpGet("profile")]
[Authorize]
public IActionResult GetProfile()
```

### Admin-Only Endpoints
```csharp
[HttpGet]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> GetAllUsers()
```

## Registration with Roles

### Register as User (Default)
```json
{
  "username": "johndoe",
  "email": "john@example.com",
  "password": "SecurePass123!"
}
```

### Register as Admin
```json
{
  "username": "admin",
  "email": "admin@example.com",
  "password": "AdminPass123!",
  "role": 1
}
```

## JWT Token Claims

The JWT token includes the following claims:
- `sub` - Username
- `email` - User email
- `jti` - Token ID
- `nameid` - User ID (ClaimTypes.NameIdentifier)
- `name` - Username (ClaimTypes.Name)
- **`role`** - User role (ClaimTypes.Role)

## Access Control Flow

### User Role Access
```
✅ POST /api/auth/register
✅ POST /api/auth/login
✅ POST /api/auth/logout (with token)
✅ GET  /api/user/profile (with token)
❌ GET  /api/user (403 Forbidden)
❌ GET  /api/user/{id} (403 Forbidden)
❌ DELETE /api/user/{id} (403 Forbidden)
❌ PATCH /api/user/{id}/role (403 Forbidden)
```

### Admin Role Access
```
✅ POST /api/auth/register
✅ POST /api/auth/login
✅ POST /api/auth/logout (with token)
✅ GET  /api/user/profile (with token)
✅ GET  /api/user (with admin token)
✅ GET  /api/user/{id} (with admin token)
✅ DELETE /api/user/{id} (with admin token)
✅ PATCH /api/user/{id}/role (with admin token)
```

## Database Schema

### Users Table
```sql
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Role INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL,
    LastLoginAt DATETIME2 NULL
)
```

## Testing Admin Access

1. **Create first admin user** during registration:
   ```bash
   POST /api/auth/register
   {
     "username": "admin",
     "email": "admin@example.com", 
     "password": "Admin123!",
     "role": 1
   }
   ```

2. **Login as admin** to get admin token:
   ```bash
   POST /api/auth/login
   {
     "username": "admin",
     "password": "Admin123!"
   }
   ```

3. **Use admin token** to manage users:
   ```bash
   Authorization: Bearer <admin_token>
   ```

4. **Promote user to admin** using PATCH endpoint:
   ```bash
   PATCH /api/user/2/role
   {
     "role": 1
   }
   ```

## Common HTTP Status Codes

- **200 OK** - Request successful
- **201 Created** - Resource created successfully
- **400 Bad Request** - Invalid request data
- **401 Unauthorized** - Missing or invalid token
- **403 Forbidden** - Valid token but insufficient permissions
- **404 Not Found** - Resource not found

## Security Notes

1. **JWT tokens include role claims** - The role is embedded in the token
2. **Token expiration** - Tokens expire after 24 hours
3. **Password hashing** - All passwords are hashed using BCrypt
4. **Role validation** - Role-based authorization is enforced at the controller level
5. **Admin creation** - First admin must be created via registration with role=1
