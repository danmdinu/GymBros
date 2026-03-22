# 06 — Live Demo: Build a Feature

**What we build:** `GetExerciseById` — a single endpoint that returns one exercise by ID.  
**Why:** 3 files, ~5 minutes, touches the full CQRS stack. Perfect live canvas.

---

## Presenter Script

### Step 1 — Open the template (1 min)
Open `prompts/build-feature.md`. Walk the team through the 7 sections briefly.

### Step 2 — Fill in the template together (3 min)
Ask the team to call out what goes in each section. Fill it in live:

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
- backend/src/WorkoutApp.Api/Controllers/ExercisesController.cs → add GET /api/exercises/{id}

## Do Not Touch
All other files.

## Requirements
1. Query: GetExerciseByIdQuery(Guid Id) implementing IRequest<ExerciseDto?>
2. Handler: returns null if not found (controller maps to 404)
3. Uses AsNoTracking()
4. Reuse the existing ExerciseDto — no new DTO needed
5. Endpoint: GET /api/exercises/{id} → 200 or 404

## Pattern to Follow
Exact pattern from GetExercisesQueryHandler.cs — same IAppDbContext injection, same LINQ Select projection.

## Constraints
- No business logic in handler or controller
- Controller only calls _mediator.Send()
- No EF Core directly — use IAppDbContext
```

### Step 3 — Run in Agent (2 min)
Open CMD+I, paste the filled template, send. Watch the 3 files generate.

### Step 4 — Review the diff together (3 min)
Open each changed file. Ask the team:
- "Does the handler follow the same pattern as GetExercisesQueryHandler?"
- "Is there anything in the controller besides `_mediator.Send()`?"
- "Is `AsNoTracking()` there?"

This is the Trust but Verify moment (Practice 09).

### Step 5 — Run `dotnet build` (1 min)
Confirm it compiles. Point out: compiling is the minimum bar — reading the diff is the real review.

---

## Teaching Points to Call Out During the Demo

- The rules file guided the AI without you repeating the conventions
- The `@GetExercisesQueryHandler` example is why the output pattern-matched correctly
- The "Do Not Touch" line is why only 3 files changed
- The diff is small and reviewable because the prompt was scoped
