# Analytics API Documentation

## Overview
The Analytics API provides comprehensive statistics and insights for your flashcard learning application. It tracks learning progress, collection performance, and score distributions.

## Endpoints

### 1. Get Complete User Analytics
**GET** `/api/analytics/{userId}`

Returns comprehensive analytics including overview stats, learning progress, top collections, and score distribution.

**Parameters:**
- `userId` (int, required): The ID of the user to get analytics for

**Response:**
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

### 2. Get Collection Analytics
**GET** `/api/analytics/{userId}/collection/{collectionId}`

Returns detailed analytics for a specific collection including score distribution, top performing cards, and cards needing review.

**Parameters:**
- `userId` (int, required): The ID of the user
- `collectionId` (int, required): The ID of the collection to analyze

**Response:**
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

### 3. Get Overview Statistics
**GET** `/api/analytics/{userId}/overview`

Returns high-level statistics about the user's learning activity.

**Parameters:**
- `userId` (int, required): The ID of the user to get overview stats for

**Response:**
```json
{
  "totalCollections": 5,
  "totalFlashCards": 150,
  "totalFlashCardsLearned": 120,
  "averageScore": 75.5
}
```

### 4. Get Learning Progress
**GET** `/api/analytics/{userId}/progress`

Returns detailed learning progress metrics categorized by mastery level.

**Parameters:**
- `userId` (int, required): The ID of the user to get learning progress for

**Response:**
```json
{
  "cardsToReview": 30,
  "cardsMastered": 80,
  "cardsInProgress": 40,
  "cardsNeedWork": 0,
  "completionRate": 80.0
}
```

## Metrics Explanation

### Overview Stats
- **Total Collections**: Number of flashcard collections created
- **Total Flash Cards**: Total number of flashcards across all collections
- **Total Flash Cards Learned**: Cards that have been studied at least once
- **Average Score**: Average raw score across all flashcards

### Learning Progress
- **Cards to Review**: Cards that haven't been studied yet (TimesLearned = 0)
- **Cards Mastered**: Cards with raw score ≥ 80 (well learned)
- **Cards in Progress**: Cards with raw score 40-79 (partially learned)
- **Cards Need Work**: Cards with raw score < 40 and studied at least once
- **Completion Rate**: Percentage of cards studied at least once

### Collection Stats
- **Flash Card Count**: Total cards in the collection
- **Cards Learned**: Cards studied at least once in this collection
- **Total Times Learned**: Sum of all TimesLearned for cards in the collection
- **Average Score**: Average score calculated as Score/TimesLearned for learned cards
- **Completion Rate**: Percentage of cards in the collection that have been studied

### Score Distribution
Cards are grouped into 5 score ranges based on raw score:
- 0-20: Needs significant work
- 21-40: Needs work
- 41-60: In progress
- 61-80: Good progress
- 81-100: Mastered

## Use Cases

1. **Dashboard Display**: Use GET `/api/analytics/{userId}` to populate a comprehensive dashboard
2. **Collection Insights**: Use GET `/api/analytics/{userId}/collection/{collectionId}` to show detailed collection performance
3. **Progress Tracking**: Use GET `/api/analytics/{userId}/progress` to display learning progress widgets
4. **Quick Stats**: Use GET `/api/analytics/{userId}/overview` for header/sidebar statistics

## Authentication
All endpoints require JWT Bearer token authentication. Include the token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
## Notes
- All endpoints are scoped to the authenticated user
- Raw scores are used for Learning Progress categorization
- Collection average scores are calculated as Score/TimesLearned for learned cards
- Each flashcard performance includes: Score (raw), TimesLearned, and AverageScore (Score/TimesLearned)
- TimesLearned field tracks how many times each card has been studied
- Raw scores are used for Learning Progress categorization
- Collection average scores are calculated as Score/TimesLearned for learned cards
- Each flashcard performance includes: Score (raw), TimesLearned, and AverageScore (Score/TimesLearned)
- Learning time is estimated at 2 minutes per card study session
- TimesLearned field tracks how many times each card has been studied
