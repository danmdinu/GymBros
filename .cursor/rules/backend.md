# Backend Rules — .NET Core

## Overview

This is the backend API for a workout/fitness tracking application. Built with ASP.NET Core following Clean Architecture principles.

---

## Tech Stack

- **Framework**: .NET 8 (LTS)
- **Database**: SQLite with Entity Framework Core
- **Authentication**: JWT Bearer tokens with refresh token rotation
- **Validation**: FluentValidation
- **Mapping**: AutoMapper (or manual mapping for simple cases)
- **CQRS**: MediatR for commands and queries
- **API Documentation**: Swagger/OpenAPI
- **Testing**: xUnit, Moq, FluentAssertions

---

### Key Principles
1. **No business logic in controllers** — Controllers only handle HTTP concerns
2. **No EF Core in Application layer** — Use repository interfaces
3. **Rich domain models** — Entities contain behavior, not just data
4. **Always use DTOs** — Never expose domain entities in API responses
5. **Use known design patterns** Use well-known design patterns when applicable. Things like Strategy Pattern, Decorator Pattern, etc. But don't overdesign, if a pattern makes the code too complex to understand then keep it simple. 
6. **In general use KISS principle** Keep it Simple, Stupid to make it easier to extend, maintain and read. 

---

## API Conventions

### Endpoints
- `GET /api/workouts` — List workouts
- `GET /api/workouts/{id}` — Get single workout
- `POST /api/workouts` — Create workout
- `PUT /api/workouts/{id}` — Full update
- `PATCH /api/workouts/{id}` — Partial update
- `DELETE /api/workouts/{id}` — Delete workout

### Response Codes
- `200 OK` — Successful GET, PUT, PATCH
- `201 Created` — Successful POST
- `204 No Content` — Successful DELETE
- `400 Bad Request` — Validation errors
- `401 Unauthorized` — Missing/invalid token
- `403 Forbidden` — Valid token but no permission
- `404 Not Found` — Resource doesn't exist

### Error Response Format
```json
{
  "type": "ValidationError",
  "title": "One or more validation errors occurred",
  "status": 400,
  "errors": {
    "Name": ["Name is required", "Name must be less than 100 characters"]
  }
}
```

---

## Database Conventions

### EF Core Configuration
- Use Fluent API in separate configuration classes
- Never use data annotations on entities
- Always configure string max lengths
- Use `ValueGeneratedOnAdd()` for IDs

## Logging

Use `ILogger<T>` with structured logging. Always include relevant context IDs.

```csharp
// ✅ GOOD
_logger.LogInformation("Workout {WorkoutId} completed by user {UserId}", workoutId, userId);
_logger.LogError(ex, "Failed to complete workout {WorkoutId}", workoutId);

// ❌ BAD
_logger.LogError("Something went wrong");
```

Log levels:
- `LogDebug` — internal trace info (disabled in production)
- `LogInformation` — business events (workout created, user registered)
- `LogWarning` — recoverable issues (retry, fallback used)
- `LogError` — failures that need attention

---

## Error Handling

Throw domain-specific exceptions. Handle them globally — never inside controllers.

```csharp
// ✅ GOOD — throw specific exceptions
throw new NotFoundException(nameof(Workout), id);
throw new ValidationException("Workout name is required");

// ❌ BAD — generic catch-and-rethrow
catch (Exception ex) { _logger.LogError(ex, "..."); throw; }
```

- Define domain exceptions in the `Domain` layer (`NotFoundException`, `ForbiddenException`, etc.)
- Handle them in a single global middleware (e.g. `ExceptionHandlingMiddleware`)
- Never swallow exceptions silently

---

## Things to Avoid

❌ **Don't** put business logic in controllers  
❌ **Don't** use `async void` — always return `Task`  
❌ **Don't** catch exceptions just to log and rethrow  
❌ **Don't** expose entity IDs as integers (use GUIDs)  
❌ **Don't** use `DateTime.Now` — inject `IDateTimeProvider`  
❌ **Don't** hardcode connection strings  
❌ **Don't** return `IQueryable` from repositories  
❌ **Don't** use `Include()` eagerly — be explicit about what you load  

---

## Location
Backend code lives in `/backend` folder.