# Prompt Template: New Backend Feature (CQRS)

> Copy this template, fill in the [PLACEHOLDERS], and paste into a Cursor Agent session (CMD+I).

---

## Usage

Use this template when adding a new query or command to the GymBros backend.

Fill in:
- `[FEATURE_NAME]` — e.g. "GetExerciseById", "UpdateUserProfile"
- `[DOMAIN]` — e.g. "Exercises", "Users", "Progress"
- `[TYPE]` — "Query" or "Command"
- `[DESCRIPTION]` — what this feature does in one sentence
- `[INPUT_FIELDS]` — the request parameters
- `[RESPONSE_FIELDS]` — the response DTO fields
- `[ENDPOINT]` — the HTTP method and path
- `[RETURN_CODE]` — 200, 201, or 204

---

## Template

```
@backend/src/WorkoutApp.Application/Features/[NEAREST_EXAMPLE_FOLDER]/
@backend/src/WorkoutApp.Application/Common/Interfaces/IAppDbContext.cs
@.cursor/rules/backend.md

Add a new [TYPE] called [FEATURE_NAME] to the [DOMAIN] domain.

Description: [DESCRIPTION]

Follow the exact pattern in the example folder above. Create:

1. [FEATURE_NAME][TYPE].cs
   - Record implementing IRequest<[RESPONSE_TYPE]>
   - Input: [INPUT_FIELDS]

2. [FEATURE_NAME][TYPE]Handler.cs
   - Implements IRequestHandler<[FEATURE_NAME][TYPE], [RESPONSE_TYPE]>
   - Injects IAppDbContext (not DbContext directly)
   - Uses AsNoTracking() for reads
   - No business logic in the handler — delegate to domain methods

3. [DOMAIN]Dto.cs (create or reuse existing)
   - Positional record with fields: [RESPONSE_FIELDS]

4. Add the endpoint to [CONTROLLER_NAME]Controller.cs:
   - [ENDPOINT]
   - Returns [RETURN_CODE] with [RESPONSE_TYPE]
   - Thin — only calls _mediator.Send()

Place everything in: backend/src/WorkoutApp.Application/Features/[DOMAIN]/[Queries|Commands]/[FEATURE_NAME]/

Do not:
- Put business logic in the handler or controller
- Use DbContext directly — use IAppDbContext
- Return entity objects — use DTOs only
- Register the handler in DI — MediatR discovers it automatically
- Touch any other files

Do not use DateTime.Now — inject IDateTimeProvider if time is needed.
```

---

## Example (filled in)

```
@backend/src/WorkoutApp.Application/Features/Exercises/Queries/GetExercises/
@backend/src/WorkoutApp.Application/Common/Interfaces/IAppDbContext.cs
@.cursor/rules/backend.md

Add a new Query called GetExerciseById to the Exercises domain.

Description: Returns a single exercise by its ID.

Follow the exact pattern in the GetExercises example above. Create:

1. GetExerciseByIdQuery.cs
   - Record implementing IRequest<ExerciseDto?>
   - Input: Guid Id

2. GetExerciseByIdQueryHandler.cs
   - Injects IAppDbContext
   - Uses AsNoTracking()
   - Returns null if not found (controller returns 404)

3. Reuse the existing ExerciseDto.cs (no changes needed)

4. Add to ExercisesController.cs:
   - GET /api/exercises/{id}
   - Returns 200 with ExerciseDto or 404 if not found

Place in: Features/Exercises/Queries/GetExerciseById/
Do not touch any other files.
```
