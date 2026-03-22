# 05 — Iterative Prompting

**Principle:** One layer at a time. Review and accept each step before starting the next.

---

## How

- Never ask for a full feature in one prompt — the AI makes assumptions at each layer that compound into bugs
- Follow the natural dependency order of your architecture: Domain → Application → Infrastructure → API → Mobile
- Each accepted diff becomes trusted context for the next prompt
- Rule of thumb: if a prompt touches more than 4 files, split it

**Bad:**
```
Add GetExerciseById with the query, handler, DTO, and controller endpoint.
```

**Good:**
```
Prompt 1: Create GetExerciseByIdQuery + Handler  → review → accept
Prompt 2: Add the endpoint to ExercisesController → review → accept
```

## Demo

> Show the "bad" prompt above, paste it into Agent, and let it run.
> Then point out how the diff mixes layers and is hard to review as a whole.
> Contrast with starting fresh and doing it in 2 scoped sessions.

## Watch out for

- "Just one more thing" — adding a second concern to a prompt mid-session
- Not reviewing between steps — the entire point of iterating is to catch mistakes before they propagate
- Reopening the same Agent session to extend it — start fresh; long sessions accumulate noise from rejected changes
