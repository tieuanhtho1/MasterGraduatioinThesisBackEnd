# Mind Map API Documentation

> Base URL: `/api/MindMap`  
> All endpoints require **JWT Bearer Authentication** (`Authorization: Bearer <token>`)

---

## Table of Contents

1. [Data Models](#data-models)
2. [Mind Map Endpoints](#mind-map-endpoints)
   - [GET /api/MindMap/{id}](#get-mind-map-by-id)
   - [GET /api/MindMap/{id}/detail](#get-mind-map-detail-with-all-nodes)
   - [GET /api/MindMap/user/{userId}](#get-mind-maps-by-user)
   - [GET /api/MindMap/collection/{collectionId}](#get-mind-maps-by-collection)
   - [POST /api/MindMap](#create-mind-map)
   - [PUT /api/MindMap/{id}](#update-mind-map)
   - [DELETE /api/MindMap/{id}](#delete-mind-map)
3. [Mind Map Node Endpoints](#mind-map-node-endpoints)
   - [GET /api/MindMap/node/{nodeId}](#get-node-by-id)
   - [POST /api/MindMap/node](#create-node)
   - [PUT /api/MindMap/node/{nodeId}](#update-node)
   - [DELETE /api/MindMap/node/{nodeId}](#delete-node)
   - [PUT /api/MindMap/{mindMapId}/nodes](#save-all-nodes-bulk-save)

---

## Data Models

### MindMapResponse (list / metadata)

```json
{
  "id": 1,
  "title": "Biology Mind Map",
  "description": "Chapter 1 concepts",
  "userId": 5,
  "flashCardCollectionId": 3,
  "collectionTitle": "Biology 101",
  "nodeCount": 12,
  "createdAt": "2026-02-24T10:00:00Z",
  "updatedAt": "2026-02-24T12:30:00Z"
}
```

### MindMapDetailResponse (full detail with nodes)

```json
{
  "id": 1,
  "title": "Biology Mind Map",
  "description": "Chapter 1 concepts",
  "userId": 5,
  "flashCardCollectionId": 3,
  "collectionTitle": "Biology 101",
  "createdAt": "2026-02-24T10:00:00Z",
  "updatedAt": "2026-02-24T12:30:00Z",
  "nodes": [
    {
      "id": 10,
      "positionX": 250.0,
      "positionY": 100.0,
      "color": "#4CAF50",
      "hideChildren": false,
      "parentNodeId": null,
      "mindMapId": 1,
      "flashCardId": 42,
      "flashCard": {
        "id": 42,
        "term": "Mitosis",
        "definition": "A type of cell division...",
        "score": 85,
        "timesLearned": 3,
        "flashCardCollectionId": 3
      }
    },
    {
      "id": 11,
      "positionX": 450.0,
      "positionY": 200.0,
      "color": "#2196F3",
      "hideChildren": false,
      "parentNodeId": 10,
      "mindMapId": 1,
      "flashCardId": 43,
      "flashCard": {
        "id": 43,
        "term": "Prophase",
        "definition": "First stage of mitosis...",
        "score": 70,
        "timesLearned": 2,
        "flashCardCollectionId": 3
      }
    }
  ]
}
```

### MindMapNodeResponse

```json
{
  "id": 10,
  "positionX": 250.0,
  "positionY": 100.0,
  "color": "#4CAF50",
  "hideChildren": false,
  "parentNodeId": null,
  "mindMapId": 1,
  "flashCardId": 42,
  "flashCard": {
    "id": 42,
    "term": "Mitosis",
    "definition": "A type of cell division...",
    "score": 85,
    "timesLearned": 3,
    "flashCardCollectionId": 3
  }
}
```

---

## Mind Map Endpoints

### Get Mind Map by ID

Returns mind map metadata (no nodes).

```
GET /api/MindMap/{id}
```

**Response:** `200 OK` → `MindMapResponse`  
**Error:** `404 Not Found` → `{ "message": "Mind map not found" }`

---

### Get Mind Map Detail (with all nodes)

⭐ **Main endpoint for React Flow rendering.** Returns the full mind map with every node and their corresponding flash card data.

```
GET /api/MindMap/{id}/detail
```

**Response:** `200 OK` → `MindMapDetailResponse`  
**Error:** `404 Not Found` → `{ "message": "Mind map not found" }`

**Frontend usage:**
- Use `nodes` array to build React Flow nodes
- Each node's `parentNodeId` defines the edge connections (parent → child)
- `positionX` / `positionY` map directly to React Flow's `position: { x, y }`
- `hideChildren` determines whether child nodes are visible
- `flashCard.term` is the label displayed on each node
- Click a node to show the full `flashCard` object (definition, score, timesLearned)

---

### Get Mind Maps by User

Returns all mind maps belonging to a user (list view).

```
GET /api/MindMap/user/{userId}
```

**Response:** `200 OK` → `MindMapResponse[]`

---

### Get Mind Maps by Collection

Returns all mind maps linked to a specific flash card collection.

```
GET /api/MindMap/collection/{collectionId}
```

**Response:** `200 OK` → `MindMapResponse[]`

---

### Create Mind Map

```
POST /api/MindMap
Content-Type: application/json
```

**Request Body:**

```json
{
  "title": "Biology Mind Map",
  "description": "Chapter 1 concepts",
  "userId": 5,
  "flashCardCollectionId": 3
}
```

| Field                   | Type   | Required | Description                          |
|-------------------------|--------|----------|--------------------------------------|
| `title`                 | string | ✅       | Mind map title                       |
| `description`           | string | ❌       | Optional description                 |
| `userId`                | int    | ✅       | Owner user ID                        |
| `flashCardCollectionId` | int    | ✅       | The collection this map belongs to   |

**Response:** `201 Created` → `MindMapResponse`  
**Errors:**
- `400 Bad Request` → `{ "message": "Title is required" }`
- `400 Bad Request` → `{ "message": "Valid UserId is required" }`
- `400 Bad Request` → `{ "message": "Valid FlashCardCollectionId is required" }`

---

### Update Mind Map

```
PUT /api/MindMap/{id}
Content-Type: application/json
```

**Request Body:**

```json
{
  "title": "Updated Title",
  "description": "Updated description",
  "flashCardCollectionId": 4
}
```

| Field                   | Type   | Required | Description                                |
|-------------------------|--------|----------|--------------------------------------------|
| `title`                 | string | ✅       | Updated title                              |
| `description`           | string | ❌       | Updated description                        |
| `flashCardCollectionId` | int?   | ❌       | Change collection (null = keep current)    |

**Response:** `200 OK` → `MindMapResponse`  
**Error:** `404 Not Found` → `{ "message": "Mind map not found" }`

---

### Delete Mind Map

Deletes the mind map and **all** its nodes.

```
DELETE /api/MindMap/{id}
```

**Response:** `200 OK` → `{ "message": "Mind map deleted successfully" }`  
**Error:** `404 Not Found` → `{ "message": "Mind map not found" }`

---

## Mind Map Node Endpoints

### Get Node by ID

Returns a single node with its flash card info.

```
GET /api/MindMap/node/{nodeId}
```

**Response:** `200 OK` → `MindMapNodeResponse`  
**Error:** `404 Not Found` → `{ "message": "Node not found" }`

---

### Create Node

Add a single node to a mind map.

```
POST /api/MindMap/node
Content-Type: application/json
```

**Request Body:**

```json
{
  "positionX": 300.0,
  "positionY": 150.0,
  "color": "#FF9800",
  "hideChildren": false,
  "parentNodeId": 10,
  "mindMapId": 1,
  "flashCardId": 44
}
```

| Field          | Type    | Required | Description                                      |
|----------------|---------|----------|--------------------------------------------------|
| `positionX`    | double  | ✅       | X coordinate for React Flow                      |
| `positionY`    | double  | ✅       | Y coordinate for React Flow                      |
| `color`        | string  | ❌       | Node color (default: `#ffffff`)                   |
| `hideChildren` | bool    | ❌       | Whether children are hidden (default: `false`)    |
| `parentNodeId` | int?    | ❌       | Parent node ID (`null` for root nodes)            |
| `mindMapId`    | int     | ✅       | Which mind map this node belongs to               |
| `flashCardId`  | int     | ✅       | The flash card this node represents               |

**Response:** `201 Created` → `MindMapNodeResponse`  
**Errors:**
- `400 Bad Request` → `{ "message": "Valid MindMapId is required" }`
- `400 Bad Request` → `{ "message": "Valid FlashCardId is required" }`

---

### Update Node

Update a node's position, color, visibility, or parent.

```
PUT /api/MindMap/node/{nodeId}
Content-Type: application/json
```

**Request Body:**

```json
{
  "positionX": 350.0,
  "positionY": 180.0,
  "color": "#E91E63",
  "hideChildren": true,
  "parentNodeId": 10
}
```

| Field          | Type    | Required | Description                                      |
|----------------|---------|----------|--------------------------------------------------|
| `positionX`    | double  | ✅       | Updated X position                               |
| `positionY`    | double  | ✅       | Updated Y position                               |
| `color`        | string  | ❌       | Updated color                                    |
| `hideChildren` | bool    | ❌       | Updated hide state                               |
| `parentNodeId` | int?    | ❌       | Updated parent (`null` to make root)             |

**Response:** `200 OK` → `MindMapNodeResponse`  
**Error:** `404 Not Found` → `{ "message": "Node not found" }`

---

### Delete Node

Deletes a node **and all its children recursively**.

```
DELETE /api/MindMap/node/{nodeId}
```

**Response:** `200 OK` → `{ "message": "Node deleted successfully" }`  
**Error:** `404 Not Found` → `{ "message": "Node not found" }`

---

### Save All Nodes (Bulk Save)

⭐ **Main save endpoint.** The frontend sends the entire node tree; the backend replaces all existing nodes for this mind map. This preserves positions, colors, hideChildren state, and parent relationships.

```
PUT /api/MindMap/{mindMapId}/nodes
Content-Type: application/json
```

**Request Body:**

```json
{
  "nodes": [
    {
      "id": 10,
      "positionX": 250.0,
      "positionY": 100.0,
      "color": "#4CAF50",
      "hideChildren": false,
      "parentNodeId": null,
      "flashCardId": 42
    },
    {
      "id": 11,
      "positionX": 450.0,
      "positionY": 200.0,
      "color": "#2196F3",
      "hideChildren": false,
      "parentNodeId": 10,
      "flashCardId": 43
    },
    {
      "id": null,
      "positionX": 650.0,
      "positionY": 300.0,
      "color": "#FF9800",
      "hideChildren": false,
      "parentNodeId": 11,
      "flashCardId": 44
    }
  ]
}
```

| Field (per node)  | Type    | Required | Description                                              |
|-------------------|---------|----------|----------------------------------------------------------|
| `id`              | int?    | ❌       | Existing node ID (null for newly created nodes)          |
| `positionX`       | double  | ✅       | X position                                               |
| `positionY`       | double  | ✅       | Y position                                               |
| `color`           | string  | ❌       | Node color                                               |
| `hideChildren`    | bool    | ❌       | Whether children are hidden                              |
| `parentNodeId`    | int?    | ❌       | Parent node's **old ID** (backend remaps automatically)  |
| `flashCardId`     | int     | ✅       | Flash card reference                                     |

**How it works:**
1. Backend deletes all existing nodes for the mind map
2. Creates all new nodes from the request
3. Remaps `parentNodeId` references from old IDs to new IDs
4. Returns the saved nodes with their new IDs and flash card data

**Response:** `200 OK`

```json
{
  "message": "Mind map nodes saved successfully",
  "nodes": [ /* MindMapNodeResponse[] */ ]
}
```

**Error:** `404 Not Found` → `{ "message": "Mind map not found" }`

---

## Frontend Integration Guide (React Flow)

### Loading a Mind Map

```javascript
// 1. Fetch the full mind map detail
const response = await fetch(`/api/MindMap/${mindMapId}/detail`, {
  headers: { Authorization: `Bearer ${token}` }
});
const data = await response.json();

// 2. Convert to React Flow nodes
const rfNodes = data.nodes.map(node => ({
  id: String(node.id),
  position: { x: node.positionX, y: node.positionY },
  data: {
    label: node.flashCard.term,
    flashCard: node.flashCard,
    color: node.color,
    hideChildren: node.hideChildren,
    nodeId: node.id,
    parentNodeId: node.parentNodeId,
    flashCardId: node.flashCardId
  },
  style: { backgroundColor: node.color },
  type: 'mindMapNode' // your custom node type
}));

// 3. Build edges from parent-child relationships
const rfEdges = data.nodes
  .filter(node => node.parentNodeId !== null)
  .map(node => ({
    id: `e${node.parentNodeId}-${node.id}`,
    source: String(node.parentNodeId),
    target: String(node.id),
    type: 'smoothstep'
  }));
```

### Saving a Mind Map

```javascript
// Collect all nodes from React Flow state
const nodesToSave = rfNodes.map(rfNode => ({
  id: rfNode.data.nodeId,           // original DB id (null for new nodes)
  positionX: rfNode.position.x,
  positionY: rfNode.position.y,
  color: rfNode.data.color,
  hideChildren: rfNode.data.hideChildren,
  parentNodeId: rfNode.data.parentNodeId,
  flashCardId: rfNode.data.flashCardId
}));

await fetch(`/api/MindMap/${mindMapId}/nodes`, {
  method: 'PUT',
  headers: {
    'Content-Type': 'application/json',
    Authorization: `Bearer ${token}`
  },
  body: JSON.stringify({ nodes: nodesToSave })
});
```

### Key Behaviors

| Feature | How it works |
|---------|-------------|
| **Tree structure** | `parentNodeId` defines parent-child. `null` = root node. |
| **Node position** | `positionX`/`positionY` map to React Flow `position: {x, y}` |
| **Hide/show children** | `hideChildren` boolean — frontend filters visible nodes |
| **Node colors** | `color` string (hex) — applied to node style |
| **Flash card display** | Each node has full `flashCard` object with term, definition, score, timesLearned |
| **Zoom/pan** | Handled entirely by React Flow — no backend involvement |
| **Save** | Use `PUT /{mindMapId}/nodes` to save entire state at once |

---

## Entity Relationship Diagram

```
User (1) ──── (N) MindMap
FlashCardCollection (1) ──── (N) MindMap
MindMap (1) ──── (N) MindMapNode
MindMapNode (0..1) ──── (N) MindMapNode  [self-referencing: parentNodeId]
FlashCard (1) ──── (N) MindMapNode
```

---

## HTTP Status Codes

| Code | Meaning |
|------|---------|
| `200` | Success |
| `201` | Created |
| `400` | Bad Request (validation error) |
| `401` | Unauthorized (missing/invalid JWT) |
| `404` | Not Found |
| `500` | Internal Server Error |
