# 11 — Onboarding with AI

**Principle:** A new developer can build a mental model of the codebase in hours, not days.

---

## How

- Ask before touching anything — use CMD+L to map the territory first
- Scope questions to folders, not `@codebase`
- Use `MEMORY.md` (Practice 10) so new devs get instant context on current state

### Useful Onboarding Prompts

**Architecture overview:**
```
@backend/src @.cursor/rules/backend.md
I just joined this project. Give me a 5-minute overview:
what does it do, how are the layers structured, and what are
the most important conventions before I write any code?
```

**End-to-end feature flow:**
```
@backend/src/WorkoutApp.Application/Features/Progress/
Walk me through the complete flow when a user completes a workout —
from HTTP request to domain entity and back.
```

**Where to add something:**
```
@backend/src
I need to add a read-only endpoint for [X]. What files do I create or modify?
List only — no code.
```

## Demo

> Run the architecture overview prompt live against this repo.
> Show how a new developer gets a useful answer in seconds.

## Watch out for

- `@codebase` on every question — folder-level @-mentions give faster, more focused answers
- Asking the AI to modify code before understanding the existing structure
- Skipping `MEMORY.md` — without it, new devs ask teammates the same questions the AI could answer
