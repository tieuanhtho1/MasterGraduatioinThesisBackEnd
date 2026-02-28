# Comprehensive Application Documentation
## Flashcard Learning Management System with Mind Mapping

**Version:** 1.0  
**Framework:** ASP.NET Core 8.0 Web API  
**Database:** Microsoft SQL Server with Entity Framework Core  
**Authentication:** JWT Bearer Token

---

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [System Architecture](#system-architecture)
3. [Technology Stack](#technology-stack)
4. [Database Schema](#database-schema)
5. [Core Features](#core-features)
6. [API Documentation](#api-documentation)
7. [Security & Authentication](#security--authentication)
8. [Business Logic Layer](#business-logic-layer)
9. [Data Models](#data-models)
10. [Testing & Development](#testing--development)
11. [Deployment Configuration](#deployment-configuration)

---

## 1. Executive Summary

This application is a comprehensive **Flashcard Learning Management System** designed to enhance the learning experience through:

- **Digital Flashcards**: Create, organize, and manage flashcards in hierarchical collections
- **Adaptive Learning Sessions**: Smart algorithm that prioritizes cards based on performance scores
- **Visual Mind Mapping**: Transform flashcard collections into interactive mind maps for visual learning
- **Analytics Dashboard**: Track learning progress with detailed statistics and insights
- **Role-Based Access**: User and Admin roles with appropriate authorization levels

The system implements a RESTful Web API built with modern software architecture principles, ensuring scalability, maintainability, and security.

---

## 2. System Architecture

### 2.1 Layered Architecture Pattern

The application follows a **three-tier layered architecture** with clear separation of concerns:

```
┌─────────────────────────────────────────┐
│         Controllers Layer               │
│  (HTTP Request/Response Handling)       │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│      Business Logic Layer               │
│  (Business Rules & Validation)          │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│         Services Layer                  │
│  (Database Operations via EF Core)      │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│         Data Layer                      │
│  (DbContext & Database)                 │
└─────────────────────────────────────────┘
```

#### **Controllers Layer** (`Controllers/`)
- Handles HTTP requests and responses
- Performs input validation
- Manages authentication/authorization
- Returns appropriate HTTP status codes
- No business logic

#### **Business Logic Layer** (`BusinessLogic/`)
- Implements business rules and complex validations
- Coordinates multiple service operations
- Transforms data for business requirements
- Enforces application-specific logic
- Examples: Score calculations, collection hierarchy management

#### **Services Layer** (`Services/`)
- Direct database operations (CRUD)
- Uses Entity Framework Core DbContext
- Pure data access, no business logic
- Abstracts database implementation details

#### **Data Layer** (`Data/`)
- ApplicationDbContext configuration
- Entity relationships and constraints
- Database schema definition

### 2.2 Benefits of This Architecture

1. **Separation of Concerns**: Each layer has a distinct responsibility
2. **Testability**: Layers can be unit tested independently
3. **Maintainability**: Changes in one layer don't affect others
4. **Scalability**: Easy to add new features without affecting existing code
5. **Reusability**: Business logic and services can be reused across controllers
6. **Dependency Injection**: All layers use dependency injection for loose coupling

### 2.3 Request Flow Example

**Example: User creates a flashcard**

1. **HTTP POST** request → `/api/flashcard`
2. **Controller** (`FlashCardController`) validates request format
3. **Business Logic** (`FlashCardBusinessLogic`) validates business rules (e.g., collection exists)
4. **Service** (`FlashCardService`) inserts record into database via DbContext
5. **Database** persists the flashcard
6. Response flows back through all layers to client

---

## 3. Technology Stack

### 3.1 Core Technologies

| Component | Technology | Version |
|-----------|------------|---------|
| Framework | ASP.NET Core | 8.0 |
| Language | C# | Latest |
| Runtime | .NET | 8.0 |
| Database | Microsoft SQL Server | Latest |
| ORM | Entity Framework Core | 8.0.11 |
| Authentication | JWT Bearer | 8.0.11 |
| Password Hashing | BCrypt.Net-Next | 4.0.3 |
| API Documentation | Swagger/OpenAPI | 6.6.2 |

### 3.2 NuGet Packages

```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.11" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.11" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.11" />
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
```

### 3.3 Development Tools

- **IDE**: Visual Studio / Visual Studio Code
- **Package Manager**: NuGet
- **Database Management**: SQL Server Management Studio (SSMS) / Azure Data Studio
- **API Testing**: Swagger UI, Postman
- **Version Control**: Git

---

## 4. Database Schema

### 4.1 Entity Relationship Diagram

```
┌──────────────┐
│    User      │
│──────────────│
│ Id (PK)      │
│ Username     │◄───────┐
│ Email        │        │
│ PasswordHash │        │
│ Role         │        │
│ CreatedAt    │        │
│ LastLoginAt  │        │
└──────────────┘        │
      ▲                 │
      │                 │
      │1                │
      │                 │
      │N                │
┌─────┴──────────────┐  │
│ FlashCardCollection│  │
│────────────────────│  │
│ Id (PK)            │  │
│ UserId (FK)        │──┘
│ ParentId (FK)      │───┐ Self-referencing
│ Title              │   │ (Hierarchical)
│ Description        │◄──┘
└─────┬──────────────┘
      │1
      │
      │N
┌─────▼──────────┐
│   FlashCard    │
│────────────────│
│ Id (PK)        │
│ Term           │
│ Definition     │
│ Score          │
│ TimesLearned   │
│ CollectionId(FK)│
└─────┬──────────┘
      ▲
      │N
      │
┌─────┴──────────┐     ┌──────────────┐
│ MindMapNode    │N   1│   MindMap    │
│────────────────│◄────│──────────────│
│ Id (PK)        │     │ Id (PK)      │
│ MindMapId (FK) │     │ Name         │
│ FlashCardId(FK)│     │ Description  │
│ ParentNodeId(FK)│     │ UserId (FK)  │───┐
│ PositionX      │     │ CreatedAt    │   │
│ PositionY      │     │ UpdatedAt    │   │
│ Color          │     └──────────────┘   │
│ HideChildren   │                         │
│ CreatedAt      │                         │
│ UpdatedAt      │                         │
└────────────────┘          └──────────────┘
      ▲                    (Links to User)
      │
      └─────┐ Self-referencing
            │ (Tree Structure)
            │
```

### 4.2 Table Descriptions

#### **Users Table**
Stores user authentication and profile information.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Unique identifier |
| Username | nvarchar(50) | Unique, Required | Login username |
| Email | nvarchar(100) | Unique, Required | User email |
| PasswordHash | nvarchar(max) | Required | BCrypt hashed password |
| Role | int | Required, Default: 0 | 0=User, 1=Admin |
| CreatedAt | datetime2 | Required | Registration date |
| LastLoginAt | datetime2 | Nullable | Last login timestamp |

#### **FlashCardCollections Table**
Organizes flashcards into hierarchical collections.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Unique identifier |
| UserId | int | FK → Users | Owner of collection |
| ParentId | int | FK → Self, Nullable | Parent collection for hierarchy |
| Title | nvarchar(max) | Required | Collection name |
| Description | nvarchar(max) | Optional | Collection description |

**Relationships:**
- One User → Many Collections
- Self-referencing for hierarchical structure (Collections can have sub-collections)
- Delete behavior: Restrict (prevents orphaned collections)

#### **FlashCards Table**
Individual flashcard items within collections.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Unique identifier |
| Term | nvarchar(max) | Required | Front of card (question/term) |
| Definition | nvarchar(max) | Required | Back of card (answer/definition) |
| Score | int | Required | Raw score (0-100) |
| TimesLearned | int | Required | Study session count |
| FlashCardCollectionId | int | FK → Collections | Parent collection |

**Relationships:**
- One Collection → Many FlashCards
- Delete behavior: Cascade (deleting collection removes all flashcards)

#### **MindMaps Table**
Container for mind map visualizations.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Unique identifier |
| Name | nvarchar(200) | Required | Mind map title |
| Description | nvarchar(500) | Optional | Mind map description |
| UserId | int | FK → Users | Owner of mind map |
| CreatedAt | datetime2 | Required | Creation timestamp |
| UpdatedAt | datetime2 | Nullable | Last modification time |

**Relationships:**
- One User → Many MindMaps
- Delete behavior: Cascade (deleting user removes mind maps)

#### **MindMapNodes Table**
Individual nodes in a mind map, each linked to a flashcard.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Unique identifier |
| MindMapId | int | FK → MindMaps | Parent mind map |
| FlashCardId | int | FK → FlashCards | Associated flashcard |
| ParentNodeId | int | FK → Self, Nullable | Parent node (null = root) |
| PositionX | double | Required | X coordinate on canvas |
| PositionY | double | Required | Y coordinate on canvas |
| Color | nvarchar(50) | Default: "#3B82F6" | Hex color code |
| HideChildren | bool | Default: false | Visibility toggle |
| CreatedAt | datetime2 | Required | Creation timestamp |
| UpdatedAt | datetime2 | Nullable | Last modification time |

**Relationships:**
- One MindMap → Many Nodes
- One FlashCard → Many Nodes (same card can appear in multiple maps)
- Self-referencing for tree structure (nodes can have parent/children)
- Delete behaviors:
  - MindMap delete: Cascade (removes all nodes)
  - FlashCard delete: Restrict (prevents deletion if used in mind map)
  - Parent node: Restrict (prevents cascading deletes)

---

## 5. Core Features

### 5.1 Authentication & Authorization

#### **JWT-Based Authentication**
- Stateless token-based authentication
- Tokens contain user claims (ID, username, email, role)
- Configurable expiration time
- Secure secret key signing

#### **Role-Based Authorization**
- **User Role (0)**: Standard user access
  - Manage own flashcards and collections
  - Create learning sessions
  - View own analytics
  - Create mind maps
- **Admin Role (1)**: Full system access
  - All User permissions
  - View all users
  - Delete users
  - Modify user roles

#### **Password Security**
- BCrypt hashing algorithm (one-way encryption)
- Salt automatically generated per password
- Industry-standard security practices

### 5.2 Flashcard Management

#### **Hierarchical Collections**
- Collections can contain sub-collections (tree structure)
- Unlimited nesting depth
- Organize by topic, subject, difficulty, etc.
- Cascading operations (deleting parent removes children)

#### **Flashcard Operations**
- Create individual flashcards
- Bulk create/update operations
- Search within collections
- Pagination support (10, 25, 50 items per page)
- Edit term and definition
- Delete flashcards

#### **Score System**
- Raw score: 0-100 scale
- Automatically updated based on learning session performance
- TimesLearned: Tracks study frequency
- Average score calculation: Score ÷ TimesLearned

### 5.3 Adaptive Learning Sessions

#### **Smart Flashcard Selection**
- **Weighted Random Algorithm**: Cards with lower scores appear more frequently
- Weight formula: `weight = (100 - score) + 20`
- Configurable session size (1-50 cards)
- Includes cards from entire collection tree (parent + all descendants)

#### **Score Updates**
- Batch score modification after session
- Increment/decrement based on user performance
- TimesLearned counter increments automatically
- Supports multiple cards in single request

#### **Learning Metrics**
- Cards to Review (never studied)
- Cards Mastered (score ≥ 80)
- Cards in Progress (score 40-79)
- Cards Need Work (score < 40, studied at least once)

### 5.4 Mind Mapping

#### **Visual Learning Tool**
- Transform flashcards into interactive mind maps
- Tree-based node structure
- Drag-and-drop positioning (X, Y coordinates)
- Customizable node colors
- Show/hide children functionality

#### **Node Management**
- Link flashcards to nodes
- Parent-child relationships
- Multiple root nodes supported
- Delete nodes (children become roots)
- Move nodes (change parent)

#### **Canvas Properties**
- Position tracking (PositionX, PositionY)
- Color coding for organization
- HideChildren toggle for focus
- Real-time updates via API

### 5.5 Analytics & Reporting

#### **User Analytics**
- **Overview Statistics**:
  - Total collections
  - Total flashcards
  - Cards learned (studied at least once)
  - Average score across all cards

- **Learning Progress**:
  - Cards to review
  - Cards mastered
  - Cards in progress
  - Cards needing work
  - Completion rate percentage

- **Score Distribution**:
  - 0-20: Needs significant work
  - 21-40: Needs work
  - 41-60: In progress
  - 61-80: Good progress
  - 81-100: Mastered

#### **Collection Analytics**
- Collection-specific statistics
- Top performing cards
- Cards needing review
- Completion rate
- Average study time per card

#### **Top Collections**
- Ranked by average score
- Study frequency metrics
- Completion percentage
- Total learning time

---

## 6. API Documentation

### 6.1 Authentication Endpoints

#### **POST /api/auth/register**
Register a new user account.

**Request Body:**
```json
{
  "username": "john_doe",
  "email": "john@example.com",
  "password": "SecurePass123!",
  "role": 0  // Optional: 0=User (default), 1=Admin
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "john_doe",
  "email": "john@example.com",
  "role": "User",
  "userId": 1
}
```

**Validation Rules:**
- Username: Required, unique, max 50 characters
- Email: Required, unique, valid email format, max 100 characters
- Password: Required, minimum 8 characters (recommended)
- Role: Optional, defaults to 0 (User)

---

#### **POST /api/auth/login**
Authenticate user and receive JWT token.

**Request Body:**
```json
{
  "username": "john_doe",
  "password": "SecurePass123!"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "john_doe",
  "email": "john@example.com",
  "role": "User",
  "userId": 1
}
```

**Error Responses:**
- `400 Bad Request`: Invalid credentials
- `400 Bad Request`: Missing username or password

---

#### **POST /api/auth/logout**
Logout current user (client-side token removal).

**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "message": "Logged out successfully"
}
```

---

### 6.2 User Management Endpoints

#### **GET /api/user/profile**
Get current user profile information.

**Authorization:** User or Admin  
**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "userId": "1",
  "username": "john_doe",
  "email": "john@example.com",
  "role": "User",
  "message": "This is a protected endpoint"
}
```

---

#### **GET /api/user**
Get all registered users (Admin only).

**Authorization:** Admin  
**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "username": "john_doe",
    "email": "john@example.com",
    "role": "User",
    "createdAt": "2024-01-15T10:30:00Z",
    "lastLoginAt": "2024-01-20T14:25:00Z"
  },
  {
    "id": 2,
    "username": "admin",
    "email": "admin@example.com",
    "role": "Admin",
    "createdAt": "2024-01-10T08:00:00Z",
    "lastLoginAt": "2024-01-21T09:15:00Z"
  }
]
```

---

#### **GET /api/user/{id}**
Get specific user by ID (Admin only).

**Authorization:** Admin  
**Parameters:**
- `id` (path, required): User ID

**Response (200 OK):**
```json
{
  "id": 1,
  "username": "john_doe",
  "email": "john@example.com",
  "role": "User",
  "createdAt": "2024-01-15T10:30:00Z",
  "lastLoginAt": "2024-01-20T14:25:00Z"
}
```

**Error Responses:**
- `404 Not Found`: User does not exist
- `403 Forbidden`: Non-admin user attempting access

---

#### **DELETE /api/user/{id}**
Delete user account (Admin only).

**Authorization:** Admin  
**Parameters:**
- `id` (path, required): User ID

**Response (200 OK):**
```json
{
  "message": "User deleted successfully"
}
```

---

#### **PATCH /api/user/{id}/role**
Update user role (Admin only).

**Authorization:** Admin  
**Parameters:**
- `id` (path, required): User ID

**Request Body:**
```json
{
  "role": 1  // 0=User, 1=Admin
}
```

**Response (200 OK):**
```json
{
  "message": "User role updated successfully",
  "userId": 1,
  "username": "john_doe",
  "role": "Admin"
}
```

---

### 6.3 FlashCard Collection Endpoints

#### **GET /api/flashcardcollection/{id}**
Get collection by ID with flashcard count.

**Authorization:** User/Admin  
**Parameters:**
- `id` (path, required): Collection ID

**Response (200 OK):**
```json
{
  "id": 1,
  "userId": 5,
  "parentId": null,
  "title": "Spanish Vocabulary",
  "description": "Basic Spanish words",
  "flashCardCount": 50,
  "childrenCount": 3
}
```

---

#### **GET /api/flashcardcollection/user/{userId}**
Get all collections for a user.

**Authorization:** User/Admin  
**Parameters:**
- `userId` (path, required): User ID

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "userId": 5,
    "parentId": null,
    "title": "Spanish Vocabulary",
    "description": "Basic Spanish words",
    "flashCardCount": 50,
    "childrenCount": 3
  },
  {
    "id": 2,
    "userId": 5,
    "parentId": 1,
    "title": "Spanish Verbs",
    "description": "Common Spanish verbs",
    "flashCardCount": 25,
    "childrenCount": 0
  }
]
```

---

#### **GET /api/flashcardcollection/{id}/children**
Get child collections (sub-collections).

**Authorization:** User/Admin  
**Parameters:**
- `id` (path, required): Parent collection ID

**Response (200 OK):**
```json
[
  {
    "id": 2,
    "userId": 5,
    "parentId": 1,
    "title": "Spanish Verbs",
    "description": "Common Spanish verbs",
    "flashCardCount": 25,
    "childrenCount": 0
  }
]
```

---

#### **POST /api/flashcardcollection**
Create new collection.

**Authorization:** User/Admin  
**Request Body:**
```json
{
  "userId": 5,
  "parentId": null,  // Optional: null for root collection
  "title": "French Vocabulary",
  "description": "Basic French words"
}
```

**Response (201 Created):**
```json
{
  "id": 10,
  "userId": 5,
  "parentId": null,
  "title": "French Vocabulary",
  "description": "Basic French words",
  "flashCardCount": 0,
  "childrenCount": 0
}
```

---

#### **PUT /api/flashcardcollection/{id}**
Update collection information.

**Authorization:** User/Admin  
**Parameters:**
- `id` (path, required): Collection ID

**Request Body:**
```json
{
  "title": "Advanced French Vocabulary",
  "description": "Updated description",
  "parentId": null
}
```

**Response (200 OK):**
```json
{
  "id": 10,
  "userId": 5,
  "parentId": null,
  "title": "Advanced French Vocabulary",
  "description": "Updated description",
  "flashCardCount": 0,
  "childrenCount": 0
}
```

---

#### **DELETE /api/flashcardcollection/{id}**
Delete collection and all flashcards.

**Authorization:** User/Admin  
**Parameters:**
- `id` (path, required): Collection ID

**Response (200 OK):**
```json
{
  "message": "Collection deleted successfully"
}
```

**Note:** Cascading delete removes all flashcards in the collection.

---

### 6.4 FlashCard Endpoints

#### **GET /api/flashcard/{id}**
Get flashcard by ID.

**Authorization:** User/Admin  
**Parameters:**
- `id` (path, required): Flashcard ID

**Response (200 OK):**
```json
{
  "id": 42,
  "term": "Hola",
  "definition": "Hello",
  "score": 85,
  "flashCardCollectionId": 1
}
```

---

#### **GET /api/flashcard/collection/{collectionId}**
Get flashcards in a collection with pagination and search.

**Authorization:** User/Admin  
**Parameters:**
- `collectionId` (path, required): Collection ID
- `pageNumber` (query, optional): Page number (default: 1)
- `pageSize` (query, optional): Items per page (default: 10)
- `searchText` (query, optional): Search term/definition

**Example Request:**
```
GET /api/flashcard/collection/1?pageNumber=1&pageSize=20&searchText=hola
```

**Response (200 OK):**
```json
{
  "flashCards": [
    {
      "id": 42,
      "term": "Hola",
      "definition": "Hello",
      "score": 85,
      "flashCardCollectionId": 1
    }
  ],
  "pageNumber": 1,
  "pageSize": 20,
  "totalCount": 50,
  "totalPages": 3,
  "searchText": "hola"
}
```

---

#### **POST /api/flashcard**
Create single flashcard.

**Authorization:** User/Admin  
**Request Body:**
```json
{
  "term": "Adiós",
  "definition": "Goodbye",
  "score": 0,  // Optional: defaults to 0
  "flashCardCollectionId": 1
}
```

**Response (201 Created):**
```json
{
  "id": 43,
  "term": "Adiós",
  "definition": "Goodbye",
  "score": 0,
  "flashCardCollectionId": 1
}
```

---

#### **POST /api/flashcard/bulk**
Create or update multiple flashcards at once.

**Authorization:** User/Admin  
**Request Body:**
```json
{
  "flashCardCollectionId": 1,
  "flashCards": [
    {
      "id": 0,  // 0 = create new, >0 = update existing
      "term": "Buenos días",
      "definition": "Good morning",
      "score": 0
    },
    {
      "id": 42,  // Update existing card
      "term": "Hola",
      "definition": "Hello (updated)",
      "score": 90
    }
  ]
}
```

**Response (200 OK):**
```json
{
  "message": "Successfully created 1 and updated 1 flashcard(s)",
  "flashCards": [
    {
      "id": 44,
      "term": "Buenos días",
      "definition": "Good morning",
      "score": 0,
      "flashCardCollectionId": 1
    },
    {
      "id": 42,
      "term": "Hola",
      "definition": "Hello (updated)",
      "score": 90,
      "flashCardCollectionId": 1
    }
  ]
}
```

---

#### **PUT /api/flashcard/{id}**
Update flashcard.

**Authorization:** User/Admin  
**Parameters:**
- `id` (path, required): Flashcard ID

**Request Body:**
```json
{
  "term": "Hola",
  "definition": "Hello (formal and informal)",
  "score": 90
}
```

**Response (200 OK):**
```json
{
  "id": 42,
  "term": "Hola",
  "definition": "Hello (formal and informal)",
  "score": 90,
  "flashCardCollectionId": 1
}
```

---

#### **DELETE /api/flashcard/{id}**
Delete flashcard.

**Authorization:** User/Admin  
**Parameters:**
- `id` (path, required): Flashcard ID

**Response (200 OK):**
```json
{
  "message": "FlashCard deleted successfully"
}
```

---

### 6.5 Learn Session Endpoints

#### **GET /api/learnsession**
Get weighted random flashcards for study session.

**Authorization:** User/Admin  
**Query Parameters:**
- `collectionId` (required): Root collection ID
- `count` (optional): Number of cards (default: 10, max: 50)

**Example Request:**
```
GET /api/learnsession?collectionId=1&count=15
```

**Response (200 OK):**
```json
{
  "collectionId": 1,
  "count": 15,
  "flashCards": [
    {
      "id": 42,
      "term": "Difícil",
      "definition": "Difficult",
      "score": 20,  // Low score = appears more frequently
      "flashCardCollectionId": 1
    },
    {
      "id": 43,
      "term": "Hola",
      "definition": "Hello",
      "score": 85,
      "flashCardCollectionId": 1
    }
  ]
}
```

**Weighting Algorithm:**
```csharp
weight = (100 - score) + 20
// Cards with lower scores have higher weights (appear more often)
// Example: Score 20 → Weight 100
//          Score 80 → Weight 40
```

---

#### **PUT /api/learnsession/scores**
Update scores after learning session.

**Authorization:** User/Admin  
**Request Body:**
```json
{
  "scoreUpdates": [
    {
      "flashCardId": 42,
      "scoreModification": 10,  // Add 10 to current score
      "timesLearned": 1  // Increment times learned
    },
    {
      "flashCardId": 43,
      "scoreModification": -5,  // Subtract 5 from current score
      "timesLearned": 1
    }
  ]
}
```

**Response (200 OK):**
```json
{
  "message": "Scores updated successfully",
  "updatedCount": 2
}
```

**Score Constraints:**
- Minimum score: 0
- Maximum score: 100
- TimesLearned always increments

---

### 6.6 Analytics Endpoints

#### **GET /api/analytics/{userId}**
Get comprehensive user analytics.

**Authorization:** User/Admin  
**Parameters:**
- `userId` (path, required): User ID

**Response (200 OK):**
```json
{
  "overview": {
    "totalCollections": 5,
    "totalFlashCards": 150,
    "totalFlashCardsLearned": 120,
    "averageScore": 75.5
  },
  "learningProgress": {
    "cardsToReview": 30,
    "cardsMastered": 80,
    "cardsInProgress": 40,
    "cardsNeedWork": 0,
    "completionRate": 80.0
  },
  "topCollections": [
    {
      "collectionId": 1,
      "collectionTitle": "Spanish Vocabulary",
      "flashCardCount": 50,
      "cardsLearned": 45,
      "totalTimesLearned": 320,
      "averageScore": 85.2,
      "completionRate": 90.0
    }
  ],
  "scoreDistribution": {
    "score0To20": 5,
    "score21To40": 10,
    "score41To60": 15,
    "score61To80": 50,
    "score81To100": 70
  }
}
```

---

#### **GET /api/analytics/{userId}/collection/{collectionId}**
Get detailed collection analytics.

**Authorization:** User/Admin  
**Parameters:**
- `userId` (path, required): User ID
- `collectionId` (path, required): Collection ID

**Response (200 OK):**
```json
{
  "collectionId": 1,
  "collectionTitle": "Spanish Vocabulary",
  "description": "Basic Spanish words and phrases",
  "totalFlashCards": 50,
  "flashCardsLearned": 45,
  "averageScore": 85.2,
  "completionRate": 90.0,
  "scoreDistribution": {
    "score0To20": 1,
    "score21To40": 2,
    "score41To60": 5,
    "score61To80": 15,
    "score81To100": 27
  },
  "topPerformingCards": [
    {
      "id": 101,
      "term": "Hola",
      "score": 100,
      "timesLearned": 15,
      "averageScore": 6.67
    }
  ],
  "cardsNeedingReview": [
    {
      "id": 105,
      "term": "Difícil",
      "score": 25,
      "timesLearned": 3,
      "averageScore": 8.33
    }
  ]
}
```

---

#### **GET /api/analytics/{userId}/overview**
Get overview statistics only.

**Authorization:** User/Admin  
**Parameters:**
- `userId` (path, required): User ID

**Response (200 OK):**
```json
{
  "totalCollections": 5,
  "totalFlashCards": 150,
  "totalFlashCardsLearned": 120,
  "averageScore": 75.5
}
```

---

#### **GET /api/analytics/{userId}/progress**
Get learning progress metrics.

**Authorization:** User/Admin  
**Parameters:**
- `userId` (path, required): User ID

**Response (200 OK):**
```json
{
  "cardsToReview": 30,
  "cardsMastered": 80,
  "cardsInProgress": 40,
  "cardsNeedWork": 0,
  "completionRate": 80.0
}
```

**Progress Categories:**
- **Cards to Review**: TimesLearned = 0
- **Cards Mastered**: Score ≥ 80
- **Cards in Progress**: Score 40-79
- **Cards Need Work**: Score < 40 and TimesLearned > 0
- **Completion Rate**: (Cards Learned / Total Cards) × 100

---

### 6.7 MindMap Endpoints

#### **GET /api/mindmap**
Get all mind maps for current user.

**Authorization:** User/Admin  
**Query Parameters:**
- `userId` (required): User ID

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Spanish Vocabulary",
    "description": "Mind map for learning Spanish words",
    "userId": 5,
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": "2024-01-16T14:20:00Z",
    "nodeCount": 15
  }
]
```

---

#### **GET /api/mindmap/{id}**
Get mind map by ID (basic info).

**Authorization:** User/Admin  
**Parameters:**
- `id` (path, required): MindMap ID

**Response (200 OK):**
```json
{
  "id": 1,
  "name": "Spanish Vocabulary",
  "description": "Mind map for learning Spanish words",
  "userId": 5,
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": "2024-01-16T14:20:00Z",
  "nodeCount": 15
}
```

---

#### **GET /api/mindmap/{id}/full**
Get full mind map with all nodes and flashcard data.

**Authorization:** User/Admin  
**Parameters:**
- `id` (path, required): MindMap ID

**Response (200 OK):**
```json
{
  "id": 1,
  "name": "Spanish Vocabulary",
  "description": "Mind map for learning Spanish words",
  "userId": 5,
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": "2024-01-16T14:20:00Z",
  "nodes": [
    {
      "id": 1,
      "mindMapId": 1,
      "parentNodeId": null,
      "positionX": 500.0,
      "positionY": 300.0,
      "color": "#3B82F6",
      "hideChildren": false,
      "createdAt": "2024-01-15T10:35:00Z",
      "updatedAt": null,
      "flashCard": {
        "id": 42,
        "term": "Hola",
        "definition": "Hello",
        "score": 85,
        "learnCount": 10,
        "collectionId": 10,
        "collectionName": "Spanish Basics"
      }
    },
    {
      "id": 2,
      "mindMapId": 1,
      "parentNodeId": 1,
      "positionX": 700.0,
      "positionY": 400.0,
      "color": "#10B981",
      "hideChildren": false,
      "createdAt": "2024-01-15T10:40:00Z",
      "updatedAt": "2024-01-16T14:20:00Z",
      "flashCard": {
        "id": 43,
        "term": "Adiós",
        "definition": "Goodbye",
        "score": 70,
        "learnCount": 5,
        "collectionId": 10,
        "collectionName": "Spanish Basics"
      }
    }
  ]
}
```

**Note:** This is the primary endpoint for frontend rendering.

---

#### **POST /api/mindmap**
Create new mind map.

**Authorization:** User/Admin  
**Request Body:**
```json
{
  "name": "French Verbs",
  "description": "Common French verbs mind map"
}
```

**Response (201 Created):**
```json
{
  "id": 2,
  "name": "French Verbs",
  "description": "Common French verbs mind map",
  "userId": 5,
  "createdAt": "2024-01-21T10:30:00Z",
  "updatedAt": null,
  "nodeCount": 0
}
```

---

#### **PUT /api/mindmap/{id}**
Update mind map basic information.

**Authorization:** User/Admin  
**Parameters:**
- `id` (path, required): MindMap ID

**Request Body:**
```json
{
  "name": "Advanced French Verbs",
  "description": "Updated description"
}
```

**Response (200 OK):**
```json
{
  "id": 2,
  "name": "Advanced French Verbs",
  "description": "Updated description",
  "userId": 5,
  "createdAt": "2024-01-21T10:30:00Z",
  "updatedAt": "2024-01-21T15:45:00Z",
  "nodeCount": 0
}
```

---

#### **DELETE /api/mindmap/{id}**
Delete mind map and all nodes.

**Authorization:** User/Admin  
**Parameters:**
- `id` (path, required): MindMap ID

**Response (200 OK):**
```json
{
  "message": "MindMap deleted successfully"
}
```

**Note:** Cascading delete removes all associated nodes.

---

### 6.8 MindMap Node Endpoints

#### **GET /api/mindmap/nodes/{nodeId}**
Get node by ID.

**Authorization:** User/Admin  
**Parameters:**
- `nodeId` (path, required): Node ID

**Response (200 OK):**
```json
{
  "id": 1,
  "mindMapId": 1,
  "flashCardId": 42,
  "parentNodeId": null,
  "positionX": 500.0,
  "positionY": 300.0,
  "color": "#3B82F6",
  "hideChildren": false,
  "createdAt": "2024-01-15T10:35:00Z",
  "updatedAt": null
}
```

---

#### **POST /api/mindmap/{mindMapId}/nodes**
Create new node in mind map.

**Authorization:** User/Admin  
**Parameters:**
- `mindMapId` (path, required): MindMap ID

**Request Body:**
```json
{
  "flashCardId": 42,
  "parentNodeId": 1,  // null for root node
  "positionX": 700.0,
  "positionY": 400.0,
  "color": "#10B981",  // Optional: defaults to "#3B82F6"
  "hideChildren": false  // Optional: defaults to false
}
```

**Response (201 Created):**
```json
{
  "id": 2,
  "mindMapId": 1,
  "flashCardId": 42,
  "parentNodeId": 1,
  "positionX": 700.0,
  "positionY": 400.0,
  "color": "#10B981",
  "hideChildren": false,
  "createdAt": "2024-01-15T10:40:00Z",
  "updatedAt": null
}
```

**Validation:**
- FlashCard must exist
- Parent node (if specified) must belong to same mind map
- Parent node cannot be the node itself (circular reference prevention)

---

#### **PUT /api/mindmap/nodes/{nodeId}**
Update node properties.

**Authorization:** User/Admin  
**Parameters:**
- `nodeId` (path, required): Node ID

**Request Body (all fields optional):**
```json
{
  "parentNodeId": 3,  // 0 = make root node, null = no change
  "positionX": 800.0,
  "positionY": 500.0,
  "color": "#EF4444",
  "hideChildren": true
}
```

**Response (200 OK):**
```json
{
  "id": 2,
  "mindMapId": 1,
  "flashCardId": 42,
  "parentNodeId": 3,
  "positionX": 800.0,
  "positionY": 500.0,
  "color": "#EF4444",
  "hideChildren": true,
  "createdAt": "2024-01-15T10:40:00Z",
  "updatedAt": "2024-01-21T16:30:00Z"
}
```

**Note:** Automatically updates mind map's `updatedAt` timestamp.

---

#### **DELETE /api/mindmap/nodes/{nodeId}**
Delete node (children become root nodes).

**Authorization:** User/Admin  
**Parameters:**
- `nodeId` (path, required): Node ID

**Response (200 OK):**
```json
{
  "message": "Node deleted successfully"
}
```

**Behavior:**
- Child nodes are NOT deleted
- Child nodes' `parentNodeId` is set to `null` (become root nodes)
- Updates mind map's `updatedAt` timestamp

---

## 7. Security & Authentication

### 7.1 JWT Token Structure

#### **Token Claims**
```json
{
  "sub": "john_doe",
  "email": "john@example.com",
  "jti": "unique-token-id",
  "nameid": "1",
  "name": "john_doe",
  "role": "User",
  "exp": 1706270400,
  "iss": "https://yourdomain.com",
  "aud": "https://yourdomain.com"
}
```

#### **Claim Types**
- `sub`: Username (subject)
- `email`: User email address
- `jti`: JWT ID (unique token identifier)
- `nameid`: User ID (ClaimTypes.NameIdentifier)
- `name`: Username (ClaimTypes.Name)
- `role`: User role (ClaimTypes.Role)
- `exp`: Expiration timestamp
- `iss`: Issuer
- `aud`: Audience

### 7.2 Authentication Flow

```
┌─────────┐                     ┌─────────┐
│ Client  │                     │  Server │
└────┬────┘                     └────┬────┘
     │                               │
     │ POST /api/auth/register       │
     │ (username, email, password)   │
     │──────────────────────────────>│
     │                               │
     │                          Hash Password
     │                          (BCrypt)
     │                               │
     │                          Save to DB
     │                               │
     │                          Generate JWT
     │                               │
     │ <──────────────────────────── │
     │ { token, user info }          │
     │                               │
     │ POST /api/flashcard           │
     │ Authorization: Bearer {token} │
     │──────────────────────────────>│
     │                               │
     │                          Validate Token
     │                          (Signature, Exp)
     │                               │
     │                          Extract Claims
     │                               │
     │                          Check Authorization
     │                               │
     │ <──────────────────────────── │
     │ { flashcard data }            │
     │                               │
```

### 7.3 Password Hashing

#### **BCrypt Algorithm**
- Work factor: 10-12 (configurable)
- Automatic salt generation
- One-way hashing (cannot be reversed)
- Industry-standard security

**Example:**
```csharp
// Registration
string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

// Login verification
bool isValid = BCrypt.Net.BCrypt.Verify(password, passwordHash);
```

### 7.4 Authorization Levels

#### **Public Endpoints** (No authentication required)
- POST `/api/auth/register`
- POST `/api/auth/login`

#### **Authenticated Endpoints** (Any logged-in user)
- POST `/api/auth/logout`
- GET `/api/user/profile`
- All FlashCard endpoints
- All FlashCardCollection endpoints
- All LearnSession endpoints
- All Analytics endpoints
- All MindMap endpoints

#### **Admin-Only Endpoints**
- GET `/api/user` (list all users)
- GET `/api/user/{id}` (get user by ID)
- DELETE `/api/user/{id}` (delete user)
- PATCH `/api/user/{id}/role` (update user role)

### 7.5 Security Best Practices

#### **Implemented**
✅ JWT token authentication  
✅ BCrypt password hashing  
✅ Role-based authorization  
✅ HTTPS enforcement (production)  
✅ CORS configuration  
✅ Password complexity requirements  
✅ Unique username and email constraints  
✅ Token expiration  

#### **Recommended for Production**
⚠️ Token refresh mechanism  
⚠️ Token blacklisting for logout  
⚠️ Rate limiting  
⚠️ Account lockout after failed attempts  
⚠️ Two-factor authentication (2FA)  
⚠️ Password reset functionality  
⚠️ Email verification  
⚠️ Audit logging  

### 7.6 CORS Configuration

**Development Configuration:**
```csharp
app.UseCors(x => x.AllowAnyHeader()
                  .AllowAnyMethod()
                  .WithOrigins("http://localhost:3000", 
                               "https://localhost:3000"));
```

**Production Recommendations:**
- Specify exact allowed origins
- Limit allowed methods (GET, POST, PUT, DELETE)
- Specify allowed headers
- Enable credentials if needed

---

## 8. Business Logic Layer

### 8.1 AuthBusinessLogic

#### **Registration Logic**
1. Validate username and email uniqueness
2. Hash password with BCrypt
3. Create user record with default role (User)
4. Generate JWT token
5. Update LastLoginAt timestamp
6. Return auth response with token

#### **Login Logic**
1. Find user by username
2. Verify password with BCrypt
3. Generate new JWT token
4. Update LastLoginAt timestamp
5. Return auth response with token

### 8.2 FlashCardBusinessLogic

#### **Score Calculation**
```csharp
// Average score calculation
double averageScore = timesLearned > 0 
    ? (double)score / timesLearned 
    : 0;
```

#### **Bulk Create/Update Logic**
1. Validate collection exists
2. Iterate through flashcards
3. If ID = 0: Create new flashcard
4. If ID > 0: Update existing flashcard
5. Validate term and definition not empty
6. Track created and updated counts
7. Return results with counts

### 8.3 LearnSessionBusinessLogic

#### **Weighted Random Selection**
```csharp
// Weight formula
int weight = (100 - flashCard.Score) + 20;

// Cards with lower scores get higher weights
// Example weights:
// Score 0   → Weight 120
// Score 50  → Weight 70
// Score 100 → Weight 20
```

**Algorithm Steps:**
1. Get all flashcards from collection tree (recursive)
2. Calculate weight for each flashcard
3. Select random cards based on weights
4. Return requested count (or all if less available)

#### **Score Update Logic**
```csharp
// Update score with constraints
newScore = currentScore + scoreModification;
newScore = Math.Max(0, Math.Min(100, newScore)); // Clamp 0-100

// Increment times learned
timesLearned += scoreUpdate.TimesLearned;
```

### 8.4 AnalyticsBusinessLogic

#### **Completion Rate Calculation**
```csharp
double completionRate = totalFlashCards > 0
    ? (double)flashCardsLearned / totalFlashCards * 100
    : 0;
```

#### **Learning Progress Categorization**
- **Cards to Review**: `timesLearned == 0`
- **Cards Mastered**: `score >= 80`
- **Cards in Progress**: `score >= 40 && score < 80`
- **Cards Need Work**: `score < 40 && timesLearned > 0`

#### **Top Collections Ranking**
1. Get all user collections
2. Calculate average score for each
3. Order by average score descending
4. Return top N collections

### 8.5 MindMapBusinessLogic

#### **Node Validation**
```csharp
// Validate parent node belongs to same mind map
if (parentNode.MindMapId != mindMapId)
    throw new InvalidOperationException(
        "Parent node doesn't belong to the same mindmap");

// Prevent circular references
if (parentNodeId == nodeId)
    throw new InvalidOperationException(
        "Node cannot be its own parent");
```

#### **Node Deletion Logic**
1. Find all child nodes
2. Set their `parentNodeId` to `null` (make them root nodes)
3. Delete the target node
4. Update mind map's `updatedAt` timestamp

### 8.6 FlashCardCollectionBusinessLogic

#### **Recursive Flashcard Count**
```csharp
// Count all flashcards in collection tree
int GetTotalFlashCardCount(int collectionId)
{
    int count = collection.FlashCards.Count;
    
    foreach (var child in collection.Children)
    {
        count += GetTotalFlashCardCount(child.Id);
    }
    
    return count;
}
```

---

## 9. Data Models

### 9.1 User Model
```csharp
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    
    // Navigation properties
    public List<FlashCardCollection> FlashCardCollections { get; set; } = new();
}

public enum UserRole
{
    User = 0,
    Admin = 1
}
```

### 9.2 FlashCardCollection Model
```csharp
public class FlashCardCollection
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? ParentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Navigation properties
    public User User { get; set; } = null!;
    public FlashCardCollection? Parent { get; set; }
    public List<FlashCardCollection> Children { get; set; } = new();
    public List<FlashCard> FlashCards { get; set; } = new();
}
```

### 9.3 FlashCard Model
```csharp
public class FlashCard
{
    public int Id { get; set; }
    public string Term { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public int Score { get; set; }
    public int TimesLearned { get; set; }
    public int FlashCardCollectionId { get; set; }
    
    // Navigation property
    public FlashCardCollection FlashCardCollection { get; set; } = null!;
}
```

### 9.4 MindMap Model
```csharp
public class MindMap
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int UserId { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<MindMapNode> Nodes { get; set; } = new List<MindMapNode>();
}
```

### 9.5 MindMapNode Model
```csharp
public class MindMapNode
{
    public int Id { get; set; }
    public int MindMapId { get; set; }
    public int FlashCardId { get; set; }
    public int? ParentNodeId { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public string Color { get; set; } = "#3B82F6";
    public bool HideChildren { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public MindMap MindMap { get; set; } = null!;
    public FlashCard FlashCard { get; set; } = null!;
    public MindMapNode? ParentNode { get; set; }
    public ICollection<MindMapNode> ChildNodes { get; set; } = new List<MindMapNode>();
}
```

### 9.6 DTO Models

#### **Authentication DTOs**
```csharp
// Register Request
public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
}

// Login Request
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

// Auth Response
public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int UserId { get; set; }
}
```

#### **FlashCard DTOs**
```csharp
// Create Request
public class CreateFlashCardRequest
{
    public string Term { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public int Score { get; set; } = 0;
    public int FlashCardCollectionId { get; set; }
}

// Update Request
public class UpdateFlashCardRequest
{
    public string Term { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public int Score { get; set; }
}

// Response
public class FlashCardResponse
{
    public int Id { get; set; }
    public string Term { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public int Score { get; set; }
    public int FlashCardCollectionId { get; set; }
}

// Bulk Create/Update Request
public class BulkCreateFlashCardsRequest
{
    public int FlashCardCollectionId { get; set; }
    public List<FlashCardDto> FlashCards { get; set; } = new();
}

public class FlashCardDto
{
    public int Id { get; set; } // 0 = create, >0 = update
    public string Term { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public int Score { get; set; }
}
```

---

## 10. Testing & Development

### 10.1 Database Setup

#### **Initial Setup**
```powershell
# Create migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

#### **Update After Schema Changes**
```powershell
# Create new migration
dotnet ef migrations add [MigrationName]

# Apply to database
dotnet ef database update
```

#### **Reset Database**
```powershell
# Drop database
dotnet ef database drop

# Recreate from migrations
dotnet ef database update
```

### 10.2 Running the Application

#### **Development Mode**
```powershell
# Run with hot reload
dotnet run

# Run on specific port
dotnet run --urls "https://localhost:5001"
```

#### **Production Build**
```powershell
# Publish for deployment
dotnet publish -c Release -o ./publish
```

### 10.3 Swagger UI Testing

**Access:** `https://localhost:5001/swagger`

#### **Testing Workflow**
1. **Register User**: POST `/api/auth/register`
2. **Copy JWT Token** from response
3. **Click "Authorize"** button (top right)
4. **Enter**: `Bearer {your_token}`
5. **Test Protected Endpoints**

#### **Example Test Sequence**
```
1. POST /api/auth/register → Get token
2. Authorize with token
3. POST /api/flashcardcollection → Create collection (note ID)
4. POST /api/flashcard → Create flashcard (use collection ID)
5. GET /api/flashcard/collection/{id} → View flashcards
6. GET /api/learnsession?collectionId={id}&count=5 → Get session
7. PUT /api/learnsession/scores → Update scores
8. GET /api/analytics/{userId} → View analytics
```

### 10.4 API Testing with HTTP Client

**Example: `WebAPI.http` file**
```http
### Register User
POST https://localhost:5001/api/auth/register
Content-Type: application/json

{
  "username": "testuser",
  "email": "test@example.com",
  "password": "Test123!"
}

### Login
POST https://localhost:5001/api/auth/login
Content-Type: application/json

{
  "username": "testuser",
  "password": "Test123!"
}

### Get Profile
GET https://localhost:5001/api/user/profile
Authorization: Bearer YOUR_TOKEN_HERE
```

### 10.5 Database Testing Scripts

**SQL Server Test Queries:**
```sql
-- View all users
SELECT * FROM Users;

-- View collections with counts
SELECT 
    c.Id,
    c.Title,
    u.Username,
    COUNT(f.Id) as FlashCardCount
FROM FlashCardCollections c
LEFT JOIN Users u ON c.UserId = u.Id
LEFT JOIN FlashCards f ON c.Id = f.FlashCardCollectionId
GROUP BY c.Id, c.Title, u.Username;

-- View flashcard statistics
SELECT 
    fc.Term,
    fc.Score,
    fc.TimesLearned,
    CASE 
        WHEN fc.TimesLearned > 0 THEN CAST(fc.Score AS FLOAT) / fc.TimesLearned
        ELSE 0
    END as AverageScore
FROM FlashCards fc;

-- View mind map structure
SELECT 
    mm.Name,
    mn.Id as NodeId,
    mn.ParentNodeId,
    f.Term,
    mn.PositionX,
    mn.PositionY
FROM MindMaps mm
JOIN MindMapNodes mn ON mm.Id = mn.MindMapId
JOIN FlashCards f ON mn.FlashCardId = f.Id
ORDER BY mm.Id, mn.ParentNodeId NULLS FIRST;
```

---

## 11. Deployment Configuration

### 11.1 appsettings.json

**Development Configuration:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=WebAPIDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyWithAtLeast32Characters123!",
    "Issuer": "https://yourdomain.com",
    "Audience": "https://yourdomain.com",
    "ExpirationMinutes": 1440
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Production Configuration:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=production-server;Database=WebAPIDb;User Id=sa;Password=YourStrongPassword;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "USE_ENVIRONMENT_VARIABLE_IN_PRODUCTION",
    "Issuer": "https://production.yourdomain.com",
    "Audience": "https://production.yourdomain.com",
    "ExpirationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Error"
    }
  },
  "AllowedHosts": "yourdomain.com"
}
```

### 11.2 Environment Variables

**Recommended Production Environment Variables:**
```bash
# Database
export ConnectionStrings__DefaultConnection="Server=...;Database=...;..."

# JWT
export JwtSettings__SecretKey="YourProductionSecretKey"
export JwtSettings__Issuer="https://production.yourdomain.com"
export JwtSettings__Audience="https://production.yourdomain.com"

# ASP.NET Core
export ASPNETCORE_ENVIRONMENT="Production"
export ASPNETCORE_URLS="https://+:443;http://+:80"
```

### 11.3 Dependency Injection Configuration

**Program.cs Registration:**
```csharp
// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services (Data Access)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFlashCardService, FlashCardService>();
builder.Services.AddScoped<IFlashCardCollectionService, FlashCardCollectionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILearnSessionService, LearnSessionService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IMindMapService, MindMapService>();

// Business Logic
builder.Services.AddScoped<IUserBusinessLogic, UserBusinessLogic>();
builder.Services.AddScoped<IFlashCardBusinessLogic, FlashCardBusinessLogic>();
builder.Services.AddScoped<IFlashCardCollectionBusinessLogic, FlashCardCollectionBusinessLogic>();
builder.Services.AddScoped<IAuthBusinessLogic, AuthBusinessLogic>();
builder.Services.AddScoped<ILearnSessionBusinessLogic, LearnSessionBusinessLogic>();
builder.Services.AddScoped<IAnalyticsBusinessLogic, AnalyticsBusinessLogic>();
builder.Services.AddScoped<IMindMapBusinessLogic, MindMapBusinessLogic>();

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { /* JWT configuration */ });

// CORS
builder.Services.AddCors();
```

### 11.4 Middleware Pipeline

**Order is important:**
```csharp
var app = builder.Build();

// 1. CORS (before authentication)
app.UseCors(x => x.AllowAnyHeader()
                  .AllowAnyMethod()
                  .WithOrigins("http://localhost:3000"));

// 2. HTTPS Redirection
app.UseHttpsRedirection();

// 3. Authentication (before authorization)
app.UseAuthentication();

// 4. Authorization
app.UseAuthorization();

// 5. Map Controllers
app.MapControllers();

app.Run();
```

### 11.5 Production Deployment Checklist

#### **Security**
- [ ] Change JWT secret key (use strong random key)
- [ ] Use environment variables for secrets
- [ ] Enable HTTPS only
- [ ] Configure specific CORS origins
- [ ] Implement rate limiting
- [ ] Add API key authentication (optional)
- [ ] Enable request logging
- [ ] Implement audit logging

#### **Database**
- [ ] Use production database connection string
- [ ] Configure database backups
- [ ] Optimize database indexes
- [ ] Set up connection pooling
- [ ] Configure retry policies

#### **Performance**
- [ ] Enable response compression
- [ ] Configure caching (Redis/MemoryCache)
- [ ] Optimize Entity Framework queries
- [ ] Add health check endpoints
- [ ] Configure load balancing

#### **Monitoring**
- [ ] Set up application logging (Serilog/NLog)
- [ ] Configure error tracking (Application Insights)
- [ ] Add performance monitoring
- [ ] Set up alerts for errors
- [ ] Implement health checks

#### **Documentation**
- [ ] Update API documentation
- [ ] Document deployment process
- [ ] Create API usage guide
- [ ] Document database schema
- [ ] Write troubleshooting guide

---

## 12. Future Enhancements

### 12.1 Planned Features
- **Spaced Repetition Algorithm**: SM-2 or Leitner system
- **Image Support**: Add images to flashcards
- **Audio Pronunciation**: Text-to-speech for language learning
- **Collaborative Collections**: Share collections with other users
- **Public Collection Library**: Browse and import public collections
- **Mobile Application**: iOS and Android apps
- **Offline Mode**: PWA with offline support
- **Export/Import**: CSV, Anki, Quizlet formats

### 12.2 Technical Improvements
- **Token Refresh**: Implement refresh token mechanism
- **Real-time Updates**: SignalR for live collaboration
- **Full-Text Search**: Elasticsearch integration
- **Microservices**: Split into smaller services
- **GraphQL API**: Alternative to REST
- **Containerization**: Docker deployment
- **CI/CD Pipeline**: Automated testing and deployment
- **API Versioning**: Support multiple API versions

---

## 13. Conclusion

This Flashcard Learning Management System provides a comprehensive solution for digital learning through:

1. **Robust API Architecture**: Three-tier layered architecture ensures maintainability and scalability
2. **Advanced Learning Features**: Adaptive learning sessions with weighted random selection
3. **Visual Learning Tools**: Interactive mind mapping for visual learners
4. **Comprehensive Analytics**: Detailed progress tracking and insights
5. **Enterprise Security**: JWT authentication, role-based authorization, and BCrypt password hashing
6. **Modern Technology Stack**: ASP.NET Core 8.0, Entity Framework Core, SQL Server

The system is designed with best practices in mind, making it suitable for both educational purposes and production deployment. The clear separation of concerns, extensive API documentation, and comprehensive testing capabilities make it an ideal platform for flashcard-based learning applications.

For questions, issues, or contributions, please refer to the individual API documentation files:
- [README.md](README.md) - Quick start guide
- [ARCHITECTURE.md](ARCHITECTURE.md) - Architecture details
- [MINDMAP_API.md](MINDMAP_API.md) - Mind map API reference
- [ANALYTICS_API.md](ANALYTICS_API.md) - Analytics API reference
- [ROLES.md](ROLES.md) - Role-based authorization guide
- [TESTING.md](TESTING.md) - Testing guide

---

**Document Version:** 1.0  
**Last Updated:** January 24, 2026  
**Author:** WebAPI Development Team
