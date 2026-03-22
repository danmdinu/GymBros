# Prompt Template: Build Any Feature

> The master template. Use this as your starting point for any feature, any layer.
> Fill in every section — the more specific you are, the better the output.
> Delete sections that don't apply to your feature.

---

## How to Use

1. Copy everything below the `---`
2. Fill in all `[PLACEHOLDERS]`
3. Delete any sections marked `(delete if not applicable)`
4. Open a **new** Agent session (CMD+I)
5. Paste and send

One template = one layer or one concern. Do not combine multiple layers in a single session.

---

## The Template

```
## Context
@[PATH_TO_NEAREST_EXISTING_EXAMPLE]
@.cursor/rules/backend.md        ← (keep if backend)
@.cursor/rules/frontend.md       ← (keep if frontend)

## What I'm Building
[One sentence describing the feature and its purpose.]

## Exact Scope — Files to CREATE
[List every new file with its full path. If you're not sure, use Ask mode first.]
- [full/path/to/NewFile1.cs]
- [full/path/to/NewFile2.cs]

## Exact Scope — Files to MODIFY
[List every existing file to change and what to add/change in each.]
- [full/path/to/ExistingFile.cs] → [what to add or change, one line]

## Do Not Touch
All other files. Specifically do not modify:
- [file you're worried the AI might touch]
- [file you're worried the AI might touch]

## Requirements
[Number each requirement. Be specific — if you know the method signature, write it.
If you know the return type, write it. If you know an edge case, include it.]

1. [Requirement 1]
2. [Requirement 2]
3. [Requirement 3]

## Pattern to Follow
[Point to the specific example in the codebase the AI should replicate.
This is the most important part of the template — the AI pattern-matches from real code.]

Follow the exact pattern in: @[PATH_TO_EXAMPLE]
Specifically:
- [What to replicate from the example — naming, structure, method style]
- [What to replicate from the example]

## Constraints (Do Not Violate)
- No business logic outside the domain entity
- No EF Core / DbContext directly — use IAppDbContext
- No DateTime.Now — use IDateTimeProvider if time is needed
- No logic in controllers — thin HTTP layer only, calls _mediator.Send() only
- No inline styles on mobile — use StyleSheet.create and constants/theme.ts
- No data annotations on entities — Fluent API only
- [Add any feature-specific constraint]

## Expected Output
[Describe what you expect to receive. This anchors the AI and reduces drift.]

When done, I should have:
- [File 1 created with X and Y]
- [File 2 modified to include Z]
- No other files changed
```

---

## Filled-In Examples

### Example A — Backend: Add a new read query

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
2. Handler: returns null if exercise not found (controller maps null → 404)
3. Uses AsNoTracking()
4. Reuses the existing ExerciseDto — do not create a new DTO
5. Controller endpoint: GET /api/exercises/{id} → 200 with ExerciseDto, 404 if null

## Pattern to Follow
Follow the exact pattern in: GetExercisesQueryHandler.cs
Specifically:
- Same constructor injection of IAppDbContext
- Same LINQ Select() projection into ExerciseDto

## Constraints
- No business logic in handler or controller
- No EF Core directly — use IAppDbContext
- Controller method is thin — only calls _mediator.Send()

## Expected Output
2 new files created + 1 endpoint added to ExercisesController. No other files changed.
```

---

### Example B — Frontend: Add a new screen

```
## Context
@mobile/app/(tabs)/settings.tsx
@mobile/lib/queries.ts
@mobile/constants/theme.ts
@.cursor/rules/frontend.md

## What I'm Building
A screen where users can view and update their display name.

## Exact Scope — Files to CREATE
- mobile/app/(tabs)/profile.tsx

## Exact Scope — Files to MODIFY
- mobile/lib/queries.ts → add useUpdateDisplayName mutation (PATCH /api/user/display-name)
- mobile/app/(tabs)/_layout.tsx → add the Profile tab

## Do Not Touch
All other files.

## Requirements
1. Screen shows current display name from the existing useProgress() hook data
2. Text input for new display name (max 50 chars)
3. "Save" button triggers useUpdateDisplayName mutation
4. Loading state on Save button while mutation is pending
5. Success: show a brief confirmation message

## Pattern to Follow
Follow the exact structure in: settings.tsx
Specifically:
- Same StyleSheet.create pattern at the bottom of the file
- Same color and spacing imports from constants/theme

## Constraints
- No fetch() calls in the component
- No Zustand for server state — use TanStack Query only
- No inline styles
- No hardcoded colors

## Expected Output
1 new screen, 1 mutation added to queries.ts, 1 tab added to _layout.tsx. Nothing else changed.
```

---

### Example C — Domain: Add behavior to an entity

```
## Context
@backend/src/WorkoutApp.Domain/Entities/User.cs
@backend/src/WorkoutApp.Domain/Entities/UserProgress.cs

## What I'm Building
A method on the User entity to update the user's display name with validation.

## Exact Scope — Files to MODIFY
- backend/src/WorkoutApp.Domain/Entities/User.cs → add UpdateDisplayName(string name) method

## Do Not Touch
All other files.

## Requirements
1. Method signature: void UpdateDisplayName(string displayName)
2. Throws ArgumentException if displayName is null or whitespace
3. Throws ArgumentException if displayName.Length > 50
4. Sets DisplayName = displayName.Trim()
5. No external dependencies — domain entity is self-contained

## Pattern to Follow
Follow the pattern of existing behavior methods in UserProgress.cs (CompleteCurrentWorkout)
Specifically:
- Guard clauses at the top
- Private setter for the backing field

## Constraints
- No EF Core, no services, no DI — pure domain logic only
- No data annotations

## Expected Output
One method added to User.cs. No other files changed.
```

---

## Tips for Getting the Best Output

**The most important line in any prompt:**
```
Follow the exact pattern in: @[path/to/nearest/existing/example]
```
The AI learns from real code in your repo faster than from any description.

**If you're unsure what files to list:** use Ask mode first:
```
@backend/src
I want to add [feature]. What files would I need to create or modify?
Give me the list only — no code yet.
```
Then paste that list into this template.

**If the output isn't right:** don't ask the AI to "fix it" in the same session.
Cancel, identify the specific problem, add it to the Constraints section, and start fresh.
