# MindMap API Test Examples

This file contains example HTTP requests you can use to test the MindMap API.

## Prerequisites
1. You need a valid JWT token (login first)
2. You need existing FlashCards in your account
3. Replace `{token}` with your actual JWT token
4. Replace IDs with actual values from your database

---

## Test Sequence

### 1. Create a MindMap
```http
POST http://localhost:5000/api/mindmap
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Test MindMap",
  "description": "Testing the mindmap API"
}
```

Expected Response (200):
```json
{
  "id": 1,
  "name": "Test MindMap",
  "description": "Testing the mindmap API",
  "userId": 1,
  "createdAt": "2024-12-30T15:40:35Z",
  "updatedAt": null,
  "nodeCount": 0
}
```

---

### 2. Get All MindMaps
```http
GET http://localhost:5000/api/mindmap
Authorization: Bearer {token}
```

Expected Response (200):
```json
[
  {
    "id": 1,
    "name": "Test MindMap",
    "description": "Testing the mindmap API",
    "userId": 1,
    "createdAt": "2024-12-30T15:40:35Z",
    "updatedAt": null,
    "nodeCount": 0
  }
]
```

---

### 3. Add First Node (Root Node)
```http
POST http://localhost:5000/api/mindmap/1/nodes
Authorization: Bearer {token}
Content-Type: application/json

{
  "flashCardId": 1,
  "parentNodeId": null,
  "positionX": 500,
  "positionY": 300,
  "color": "#3B82F6",
  "hideChildren": false
}
```

Expected Response (201):
```json
{
  "id": 1,
  "mindMapId": 1,
  "flashCardId": 1,
  "parentNodeId": null,
  "positionX": 500,
  "positionY": 300,
  "color": "#3B82F6",
  "hideChildren": false,
  "createdAt": "2024-12-30T15:45:00Z",
  "updatedAt": null
}
```

---

### 4. Add Child Node
```http
POST http://localhost:5000/api/mindmap/1/nodes
Authorization: Bearer {token}
Content-Type: application/json

{
  "flashCardId": 2,
  "parentNodeId": 1,
  "positionX": 700,
  "positionY": 400,
  "color": "#10B981",
  "hideChildren": false
}
```

---

### 5. Add Another Child Node
```http
POST http://localhost:5000/api/mindmap/1/nodes
Authorization: Bearer {token}
Content-Type: application/json

{
  "flashCardId": 3,
  "parentNodeId": 1,
  "positionX": 700,
  "positionY": 200,
  "color": "#EF4444",
  "hideChildren": false
}
```

---

### 6. Get Full MindMap (THE MAIN ENDPOINT)
```http
GET http://localhost:5000/api/mindmap/1/full
Authorization: Bearer {token}
```

Expected Response (200):
```json
{
  "id": 1,
  "name": "Test MindMap",
  "description": "Testing the mindmap API",
  "userId": 1,
  "createdAt": "2024-12-30T15:40:35Z",
  "updatedAt": "2024-12-30T15:50:00Z",
  "nodes": [
    {
      "id": 1,
      "mindMapId": 1,
      "parentNodeId": null,
      "positionX": 500,
      "positionY": 300,
      "color": "#3B82F6",
      "hideChildren": false,
      "createdAt": "2024-12-30T15:45:00Z",
      "updatedAt": null,
      "flashCard": {
        "id": 1,
        "term": "Hello",
        "definition": "A greeting",
        "score": 3,
        "learnCount": 5,
        "collectionId": 1,
        "collectionName": "Basic Words"
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
      "createdAt": "2024-12-30T15:47:00Z",
      "updatedAt": null,
      "flashCard": {
        "id": 2,
        "term": "World",
        "definition": "The earth",
        "score": 2,
        "learnCount": 3,
        "collectionId": 1,
        "collectionName": "Basic Words"
      }
    },
    {
      "id": 3,
      "mindMapId": 1,
      "parentNodeId": 1,
      "positionX": 700,
      "positionY": 200,
      "color": "#EF4444",
      "hideChildren": false,
      "createdAt": "2024-12-30T15:49:00Z",
      "updatedAt": null,
      "flashCard": {
        "id": 3,
        "term": "Goodbye",
        "definition": "A farewell",
        "score": 1,
        "learnCount": 2,
        "collectionId": 1,
        "collectionName": "Basic Words"
      }
    }
  ]
}
```

---

### 7. Update Node Position (User drags node)
```http
PUT http://localhost:5000/api/mindmap/nodes/2
Authorization: Bearer {token}
Content-Type: application/json

{
  "positionX": 800,
  "positionY": 450
}
```

---

### 8. Update Node Color
```http
PUT http://localhost:5000/api/mindmap/nodes/2
Authorization: Bearer {token}
Content-Type: application/json

{
  "color": "#8B5CF6"
}
```

---

### 9. Hide Children of Root Node
```http
PUT http://localhost:5000/api/mindmap/nodes/1
Authorization: Bearer {token}
Content-Type: application/json

{
  "hideChildren": true
}
```

---

### 10. Move Node to Different Parent
```http
PUT http://localhost:5000/api/mindmap/nodes/3
Authorization: Bearer {token}
Content-Type: application/json

{
  "parentNodeId": 2
}
```

---

### 11. Make Node a Root Node
```http
PUT http://localhost:5000/api/mindmap/nodes/3
Authorization: Bearer {token}
Content-Type: application/json

{
  "parentNodeId": 0
}
```

---

### 12. Get Specific Node
```http
GET http://localhost:5000/api/mindmap/nodes/1
Authorization: Bearer {token}
```

---

### 13. Delete Node
```http
DELETE http://localhost:5000/api/mindmap/nodes/3
Authorization: Bearer {token}
```

Expected Response (200):
```json
{
  "message": "Node deleted successfully"
}
```

---

### 14. Update MindMap Details
```http
PUT http://localhost:5000/api/mindmap/1
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Updated MindMap Name",
  "description": "Updated description"
}
```

---

### 15. Delete MindMap
```http
DELETE http://localhost:5000/api/mindmap/1
Authorization: Bearer {token}
```

Expected Response (200):
```json
{
  "message": "MindMap deleted successfully"
}
```

---

## Testing with Swagger

You can also test all endpoints using Swagger UI:

1. Navigate to: http://localhost:5000/swagger
2. Click "Authorize" button
3. Enter: `Bearer {your-token}`
4. Try out the endpoints under "MindMap" section

---

## Testing with VS Code REST Client

If you have the REST Client extension installed:

1. Create a file named `mindmap.http`
2. Add the requests above
3. Click "Send Request" above each request

---

## Common Error Scenarios

### 401 Unauthorized
```json
{
  "message": "MindMap not found or access denied"
}
```
**Cause**: Invalid token or trying to access someone else's mindmap

### 404 Not Found
```json
{
  "message": "MindMap not found"
}
```
**Cause**: MindMap ID doesn't exist

### 400 Bad Request
```json
{
  "message": "Parent node not found or doesn't belong to the same mindmap"
}
```
**Cause**: Invalid parent node ID

### 400 Bad Request
```json
{
  "message": "FlashCard not found or access denied"
}
```
**Cause**: FlashCard doesn't exist or doesn't belong to user

---

## Performance Testing

### Create Multiple Nodes at Once
```bash
# Create 10 nodes in sequence
for i in {1..10}
do
  curl -X POST http://localhost:5000/api/mindmap/1/nodes \
    -H "Authorization: Bearer {token}" \
    -H "Content-Type: application/json" \
    -d "{\"flashCardId\":$i,\"positionX\":$((500+i*100)),\"positionY\":300}"
done
```

### Batch Update Positions
Update multiple nodes after dragging them around:

```javascript
// Pseudo-code
const updates = changedNodes.map(node => 
  fetch(`/api/mindmap/nodes/${node.id}`, {
    method: 'PUT',
    body: JSON.stringify({
      positionX: node.x,
      positionY: node.y
    })
  })
);

await Promise.all(updates);
```

---

## Database Verification

After creating mindmap and nodes, check the database:

```sql
-- Check MindMaps table
SELECT * FROM MindMaps WHERE UserId = 1;

-- Check MindMapNodes table
SELECT * FROM MindMapNodes WHERE MindMapId = 1;

-- Check tree structure
SELECT 
  n.Id,
  n.ParentNodeId,
  f.Term,
  n.PositionX,
  n.PositionY,
  n.Color
FROM MindMapNodes n
JOIN FlashCards f ON n.FlashCardId = f.Id
WHERE n.MindMapId = 1
ORDER BY n.ParentNodeId, n.Id;
```
