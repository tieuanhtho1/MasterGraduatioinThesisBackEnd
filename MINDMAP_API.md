# MindMap API Documentation

## Overview
The MindMap API provides endpoints for managing mind maps and their nodes. Each user can have multiple mind maps, and each mind map can contain multiple nodes arranged in a tree structure. Each node references a flashcard and can have visual properties like position, color, and visibility.

## Authentication
All endpoints require JWT Bearer token authentication.

---

## MindMap Endpoints

### 1. Get All User's MindMaps
**GET** `/api/mindmap`

Returns all mind maps belonging to the authenticated user.

**Response:**
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

### 2. Get MindMap by ID
**GET** `/api/mindmap/{id}`

Returns basic information about a specific mind map.

**Parameters:**
- `id` (path): The mind map ID

**Response:**
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

### 3. Get Full MindMap with Nodes and FlashCards
**GET** `/api/mindmap/{id}/full`

Returns complete mind map data including all nodes and their associated flashcard information. **This is the primary endpoint for displaying the mind map in the frontend.**

**Parameters:**
- `id` (path): The mind map ID

**Response:**
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
      "positionX": 500,
      "positionY": 300,
      "color": "#3B82F6",
      "hideChildren": false,
      "createdAt": "2024-01-15T10:35:00Z",
      "updatedAt": null,
      "flashCard": {
        "id": 42,
        "term": "Hola",
        "definition": "Hello",
        "score": 3,
        "learnCount": 5,
        "collectionId": 10,
        "collectionName": "Spanish Basics"
      }
    },
    {
      "id": 2,
      "mindMapId": 1,
      "parentNodeId": 1,
      "positionX": 700,
      "positionY": 400,
      "color": "#10B981",
      "hideChildren": false,
      "createdAt": "2024-01-15T10:40:00Z",
      "updatedAt": "2024-01-16T14:20:00Z",
      "flashCard": {
        "id": 43,
        "term": "Adiós",
        "definition": "Goodbye",
        "score": 2,
        "learnCount": 3,
        "collectionId": 10,
        "collectionName": "Spanish Basics"
      }
    }
  ]
}
```

---

### 4. Create MindMap
**POST** `/api/mindmap`

Creates a new mind map for the authenticated user.

**Request Body:**
```json
{
  "name": "Spanish Vocabulary",
  "description": "Mind map for learning Spanish words"
}
```

**Response:**
```json
{
  "id": 1,
  "name": "Spanish Vocabulary",
  "description": "Mind map for learning Spanish words",
  "userId": 5,
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": null,
  "nodeCount": 0
}
```

---

### 5. Update MindMap
**PUT** `/api/mindmap/{id}`

Updates the basic information (name, description) of a mind map.

**Parameters:**
- `id` (path): The mind map ID

**Request Body:**
```json
{
  "name": "Advanced Spanish Vocabulary",
  "description": "Updated description"
}
```

**Response:**
```json
{
  "id": 1,
  "name": "Advanced Spanish Vocabulary",
  "description": "Updated description",
  "userId": 5,
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": "2024-01-16T15:00:00Z",
  "nodeCount": 15
}
```

---

### 6. Delete MindMap
**DELETE** `/api/mindmap/{id}`

Deletes a mind map and all its nodes.

**Parameters:**
- `id` (path): The mind map ID

**Response:**
```json
{
  "message": "MindMap deleted successfully"
}
```

---

## MindMapNode Endpoints

### 7. Get Node by ID
**GET** `/api/mindmap/nodes/{nodeId}`

Returns information about a specific node.

**Parameters:**
- `nodeId` (path): The node ID

**Response:**
```json
{
  "id": 1,
  "mindMapId": 1,
  "flashCardId": 42,
  "parentNodeId": null,
  "positionX": 500,
  "positionY": 300,
  "color": "#3B82F6",
  "hideChildren": false,
  "createdAt": "2024-01-15T10:35:00Z",
  "updatedAt": null
}
```

---

### 8. Create Node
**POST** `/api/mindmap/{mindMapId}/nodes`

Creates a new node in a mind map.

**Parameters:**
- `mindMapId` (path): The mind map ID

**Request Body:**
```json
{
  "flashCardId": 42,
  "parentNodeId": 1,
  "positionX": 700,
  "positionY": 400,
  "color": "#10B981",
  "hideChildren": false
}
```

**Notes:**
- `parentNodeId` can be null for root nodes
- `color` defaults to "#3B82F6" if not provided
- `hideChildren` defaults to false if not provided

**Response:**
```json
{
  "id": 2,
  "mindMapId": 1,
  "flashCardId": 42,
  "parentNodeId": 1,
  "positionX": 700,
  "positionY": 400,
  "color": "#10B981",
  "hideChildren": false,
  "createdAt": "2024-01-15T10:40:00Z",
  "updatedAt": null
}
```

---

### 9. Update Node
**PUT** `/api/mindmap/nodes/{nodeId}`

Updates a node's properties (position, color, parent, hideChildren).

**Parameters:**
- `nodeId` (path): The node ID

**Request Body (all fields optional):**
```json
{
  "parentNodeId": 3,
  "positionX": 800,
  "positionY": 500,
  "color": "#EF4444",
  "hideChildren": true
}
```

**Notes:**
- All fields are optional - only provided fields will be updated
- To make a node a root node, set `parentNodeId` to 0
- Automatically updates the mind map's `updatedAt` timestamp

**Response:**
```json
{
  "id": 2,
  "mindMapId": 1,
  "flashCardId": 42,
  "parentNodeId": 3,
  "positionX": 800,
  "positionY": 500,
  "color": "#EF4444",
  "hideChildren": true,
  "createdAt": "2024-01-15T10:40:00Z",
  "updatedAt": "2024-01-16T14:20:00Z"
}
```

---

### 10. Delete Node
**DELETE** `/api/mindmap/nodes/{nodeId}`

Deletes a node. Child nodes will become root nodes (their `parentNodeId` will be set to null).

**Parameters:**
- `nodeId` (path): The node ID

**Response:**
```json
{
  "message": "Node deleted successfully"
}
```

---

## Data Models

### MindMap
```typescript
{
  id: number;
  name: string;           // Max 200 characters
  description?: string;   // Max 500 characters
  userId: number;
  createdAt: Date;
  updatedAt?: Date;
  nodeCount: number;      // Number of nodes in the map
}
```

### MindMapNode
```typescript
{
  id: number;
  mindMapId: number;
  flashCardId: number;
  parentNodeId?: number;  // null for root nodes
  positionX: number;      // X coordinate on canvas
  positionY: number;      // Y coordinate on canvas
  color: string;          // Hex color code (max 50 chars)
  hideChildren: boolean;  // Whether child nodes are hidden
  createdAt: Date;
  updatedAt?: Date;
}
```

### FlashCardInfo (in full mindmap response)
```typescript
{
  id: number;
  term: string;
  definition: string;
  score: number;
  learnCount: number;
  collectionId: number;
  collectionName: string;
}
```

---

## Error Responses

### 401 Unauthorized
```json
{
  "message": "MindMap not found or access denied"
}
```

### 404 Not Found
```json
{
  "message": "MindMap not found"
}
```

### 400 Bad Request
```json
{
  "message": "Parent node not found or doesn't belong to the same mindmap"
}
```

### 500 Internal Server Error
```json
{
  "message": "An error occurred while...",
  "error": "Error details"
}
```

---

## Frontend Implementation Notes

### Displaying the MindMap
1. Use `GET /api/mindmap/{id}/full` to fetch complete mindmap data
2. Each node contains full flashcard information (term, definition, score, etc.)
3. Build the tree structure using `parentNodeId` relationships
4. Render nodes at their `positionX` and `positionY` coordinates
5. Apply the `color` property to node styling
6. Hide child nodes if `hideChildren` is true

### Saving Node Positions
When a user moves nodes around:
```javascript
// Save position for each moved node
await fetch(`/api/mindmap/nodes/${nodeId}`, {
  method: 'PUT',
  body: JSON.stringify({
    positionX: newX,
    positionY: newY
  })
});
```

### Batch Updates
For multiple node updates (like saving entire mindmap state), make individual PUT requests for each changed node.

### Adding a New Node
```javascript
// User selects a flashcard and position
await fetch(`/api/mindmap/${mindMapId}/nodes`, {
  method: 'POST',
  body: JSON.stringify({
    flashCardId: selectedFlashCardId,
    parentNodeId: selectedParentNodeId || null,
    positionX: clickX,
    positionY: clickY,
    color: '#3B82F6'
  })
});
```

---

## Tree Structure Example

```
Root Node (parentNodeId: null)
├── Child 1 (parentNodeId: root)
│   ├── Grandchild 1 (parentNodeId: child1)
│   └── Grandchild 2 (parentNodeId: child1)
└── Child 2 (parentNodeId: root)
    └── Grandchild 3 (parentNodeId: child2)
```

Each node can have multiple children but only one parent.
