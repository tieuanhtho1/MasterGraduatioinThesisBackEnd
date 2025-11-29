# API Testing Guide

## Quick Start

1. **Update database** (if not already done):
   ```powershell
   dotnet ef database update
   ```

2. **Run the application**:
   ```powershell
   dotnet run
   ```

3. **Open Swagger UI**: Navigate to `https://localhost:5001/swagger` (or the URL shown in console)

## Testing with Swagger UI

### 1. Register a New User (Regular User)
1. Find the **POST /api/auth/register** endpoint under **Auth** section
2. Click "Try it out"
3. Enter the request body:
   ```json
   {
     "username": "testuser",
     "email": "test@example.com",
     "password": "Test123!"
   }
   ```
   Note: Role will default to 0 (User) if not specified
4. Click "Execute"
5. Copy the `token` from the response

### 1b. Register an Admin User
To register an admin user, include `"role": 1` in the request:
```json
{
  "username": "admin",
  "email": "admin@example.com",
  "password": "Admin123!",
  "role": 1
}
```

### 2. Authorize with Token
1. Click the **"Authorize"** button at the top right
2. In the dialog, enter: `Bearer YOUR_TOKEN_HERE`
3. Click "Authorize"
4. Click "Close"

### 3. Access Protected Endpoint
1. Find the **GET /api/user/profile** endpoint under **User** section
2. Click "Try it out"
3. Click "Execute"
4. You should see your user information including role

### 4. Test Admin-Only Endpoints (Admin token required)
After authorizing with an admin token:

#### Get All Users
1. Find the **GET /api/user** endpoint
2. Click "Try it out"
3. Click "Execute"
4. You should see a list of all users

#### Update User Role
1. Find the **PATCH /api/user/{id}/role** endpoint
2. Click "Try it out"
3. Enter a user ID and role:
   ```json
   {
     "role": 1
   }
   ```
4. Click "Execute"

#### Delete User
1. Find the **DELETE /api/user/{id}** endpoint
2. Enter a user ID
3. Click "Execute"

### 5. Test Login
1. Find the **POST /api/auth/login** endpoint
2. Click "Try it out"
3. Enter credentials:
   ```json
   {
     "username": "testuser",
     "password": "Test123!"
   }
   ```
4. Click "Execute"
5. You'll get a new token with role information

### 6. Logout
1. Find the **POST /api/auth/logout** endpoint
2. Click "Try it out"
3. Click "Execute"
4. You should see a success message

## Testing with cURL

### Register User
```bash
curl -X POST "https://localhost:5001/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "Test123!"
  }'
```

### Register Admin
```bash
curl -X POST "https://localhost:5001/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "email": "admin@example.com",
    "password": "Admin123!",
    "role": 1
  }'
```

### Login
```bash
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "Test123!"
  }'
```

### Access Protected Endpoint
```bash
curl -X GET "https://localhost:5001/api/user/profile" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### Get All Users (Admin only)
```bash
curl -X GET "https://localhost:5001/api/user" \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN_HERE"
```

### Update User Role (Admin only)
```bash
curl -X PATCH "https://localhost:5001/api/user/1/role" \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{"role": 1}'
```

## Testing with PowerShell

### Register User
```powershell
$body = @{
    username = "testuser"
    email = "test@example.com"
    password = "Test123!"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/auth/register" `
    -Method Post `
    -Body $body `
    -ContentType "application/json" `
    -SkipCertificateCheck
```

### Register Admin
```powershell
$body = @{
    username = "admin"
    email = "admin@example.com"
    password = "Admin123!"
    role = 1
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/auth/register" `
    -Method Post `
    -Body $body `
    -ContentType "application/json" `
    -SkipCertificateCheck
```

### Login
```powershell
$body = @{
    username = "testuser"
    password = "Test123!"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "https://localhost:5001/api/auth/login" `
    -Method Post `
    -Body $body `
    -ContentType "application/json" `
    -SkipCertificateCheck

$token = $response.token
```

### Access Protected Endpoint
```powershell
$headers = @{
    Authorization = "Bearer $token"
}

Invoke-RestMethod -Uri "https://localhost:5001/api/user/profile" `
    -Method Get `
    -Headers $headers `
    -SkipCertificateCheck
```

### Get All Users (Admin only)
```powershell
$headers = @{
    Authorization = "Bearer $adminToken"
}

Invoke-RestMethod -Uri "https://localhost:5001/api/user" `
    -Method Get `
    -Headers $headers `
    -SkipCertificateCheck
```

### Update User Role (Admin only)
```powershell
$headers = @{
    Authorization = "Bearer $adminToken"
}

$body = @{
    role = 1
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/user/1/role" `
    -Method Patch `
    -Headers $headers `
    -Body $body `
    -ContentType "application/json" `
    -SkipCertificateCheck
```

## Troubleshooting

### Database Connection Issues
- Make sure SQL Server is running
- Verify the connection string in `appsettings.json`
- Check if the database exists: `dotnet ef database update`

### JWT Token Issues
- Ensure the token is prefixed with "Bearer " in the Authorization header
- Tokens expire after 24 hours by default
- Make sure the SecretKey in appsettings.json is at least 32 characters

### Common Errors
- **401 Unauthorized**: Token is missing, invalid, or expired
- **403 Forbidden**: Valid token but insufficient permissions (e.g., User role trying to access Admin endpoint)
- **400 Bad Request**: Invalid input data (check required fields)
- **404 Not Found**: User or resource not found
- **Database not found**: Run `dotnet ef database update`

## Role Testing Scenarios

### Scenario 1: User Role Access
1. Register as User (role = 0 or omit role)
2. Login and get token
3. Access `/api/user/profile` ✅ Should work
4. Try to access `/api/user` ❌ Should return 403 Forbidden

### Scenario 2: Admin Role Access
1. Register as Admin (role = 1)
2. Login and get token
3. Access `/api/user/profile` ✅ Should work
4. Access `/api/user` ✅ Should work
5. Access `/api/user/{id}` ✅ Should work
6. Update user role ✅ Should work
7. Delete user ✅ Should work
