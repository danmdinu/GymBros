# Prompt Template: Build Any Feature

> One template for any feature, any layer. Fill in the sections, delete what doesn't apply, paste into Agent (CMD+I).
> **One session = one layer.** Don't combine multiple layers in a single prompt.

---

```
## Context
@[PATH_TO_NEAREST_EXISTING_EXAMPLE]
@.cursor/rules/backend.md        ← keep if backend
@.cursor/rules/frontend.md       ← keep if frontend

## What I'm Building
[One sentence describing what this does and why.]

## Exact Scope — Files to CREATE
- [full/path/to/NewFile.cs]
- [full/path/to/AnotherNewFile.cs]

## Exact Scope — Files to MODIFY
- [full/path/to/ExistingFile.cs] → [one line: what to add or change]

## Do Not Touch
All other files.

## Requirements
1. [Specific behavior, method signature, return type, or edge case]
2. [...]
3. [...]

## Pattern to Follow
Follow the exact pattern in: @[PATH_TO_EXAMPLE]
Specifically: [what to replicate — naming, structure, injection style]

## Constraints
- No business logic outside the domain entity
- No EF Core / DbContext directly — use IAppDbContext
- No DateTime.Now — use IDateTimeProvider if time is needed
- No logic in controllers — thin HTTP layer only
- No inline styles on mobile — use StyleSheet.create and constants/theme.ts
- [Add any feature-specific constraint]
```

---

## Filled-In Example — `GetExerciseById`

```
## Context
@backend/src/WorkoutApp.Application/Features/Exercises/Queries/GetExercises/
@.cursor/rules/backend.md

## What I'm Building
A query that returns a single exercise by its GUID ID.

## Exact Scope — Files to CREATE
- backend/src/WorkoutApp.Application/Features/Exercises/Queries/GetExerciseById/GetExerciseByIdQuery.cs
- backend/src/WorkoutApp.Application/Features/Exercises/Queries/GetExerciseById/GetExerciseByIdQueryHandler.cs

## Exact Scope — Files to MODIFY
- backend/src/WorkoutApp.Api/Controllers/ExercisesController.cs → add GET /api/exercises/{id} endpoint

## Do Not Touch
All other files.

## Requirements
1. Query record: GetExerciseByIdQuery(Guid Id) implementing IRequest<ExerciseDto?>
2. Handler returns null if the exercise is not found (controller returns 404)
3. Uses AsNoTracking()
4. Reuse the existing ExerciseDto — do not create a new one
5. Endpoint: GET /api/exercises/{id} → 200 with ExerciseDto, or 404 if null

## Pattern to Follow
Follow the exact pattern in GetExercisesQueryHandler.cs:
- Same IAppDbContext constructor injection
- Same LINQ .Select() projection into ExerciseDto
- Same AsNoTracking() call

## Constraints
- No business logic in handler or controller
- Controller method only calls _mediator.Send() — nothing else
- No EF Core directly — use IAppDbContext
```

---

## Tips

**If you're not sure what files to list:** use Ask mode first:
```
@backend/src
I want to add [feature]. List the files I need to create or modify. No code yet.
```
Then paste that list into the template above.

**If the output isn't right:** don't ask the AI to fix it in the same session.  
Cancel → identify the specific problem → add it to Requirements or Constraints → start fresh.
