# 03 — Context & @-Mentions

**Principle:** Give the AI exactly what it needs — nothing more, nothing less.

---

## How

| Mention | Includes | Use when |
|---|---|---|
| `@file.cs` | One file | Working on a specific file |
| `@folder/` | All files in folder | Feature spans a directory |
| `@codebase` | Semantic search across repo | You don't know where something lives |
| `@docs` | External docs / URLs | Referencing a library |

- Every open editor tab is silently added to context — **close unrelated files** before a session
- Narrow context = cheaper tokens + more accurate output
- Use `@codebase` for discovery, then switch to specific file @-mentions for implementation

## Demo

> Show the difference side by side:

**Broad (expensive, drifts):**
```
@backend/src Add a GetExerciseById handler
```

**Surgical (cheap, accurate):**
```
@backend/src/WorkoutApp.Application/Features/Exercises/Queries/GetExercises/GetExercisesQueryHandler.cs
Add a GetExerciseById handler following this exact pattern.
```

## Watch out for

- Leaving 10 unrelated tabs open — they all go into context silently
- Using `@codebase` for implementation prompts — use it for discovery only
- Over-narrowing: if the AI gives a generic answer, you probably need to add the relevant example file
