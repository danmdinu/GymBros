# Practice 14 — Memory Files and Skills Files: Persistent AI Intelligence

## Why It Matters

Rules (`.cursor/rules/*.md`) handle static conventions. But two other context mechanisms handle different needs: **memory files** track the ever-changing state of your project, and **skills files** are reusable playbooks for specialized tasks.

Together, these three form a system that means the AI gets smarter about your project over time — not just per session, but permanently.

---

## Memory Files

### What They Are

A `MEMORY.md` is a markdown file committed to the repo that acts as the AI's project briefing. Unlike rules (which document stable conventions), memory documents the *current state* of the project: what's in progress, what decisions were made and why, what's broken, what's off-limits.

### Why They Work

When you reference `MEMORY.md` from a rule file, the AI reads it at the start of every session. It never asks "what branch is the 2FA work on?" or "are we using TOTP or SMS?" — it already knows.

### How to Set It Up

**1. Create `MEMORY.md` at the repo root:**

```markdown
# GymBros — Project Memory

## What This Is
30-day fitness challenge app. Single workout plan. 30 daily workouts.
Backend: .NET 8, Clean Architecture, MediatR, SQLite, Firebase Auth.
Mobile: React Native + Expo + TanStack Query + Zustand.

## Architecture Decisions
- No EF migrations — EnsureCreatedAsync only (SQLite dev project)
- Firebase Auth for authentication — backend verifies JWT, creates/finds User on first login
- Mobile does NOT have Firebase SDK yet — auth integration is a follow-up
- No AutoMapper — manual projection in LINQ Select() or handler constructors
- No SMS 2FA — TOTP only

## Current Work In Progress
- Feature: 2FA (branch: feature/2fa)
  - TotpService implemented in Infrastructure/Services/
  - Interface ITotpService in Application/Common/Interfaces/
  - Commands: Enable2FA, Verify2FA, Disable2FA — in progress

## Known Issues
- UserProgress has no guard for "plan already completed" — fix pending
- Mobile uses deviceId in API URLs; backend expects Firebase JWT — mismatch, fix pending

## Do Not Change Without Discussion
- AppDbContextSeed.cs — workout data is intentionally hardcoded
- The single-plan structure — multi-plan support is out of scope for now
```

**2. Reference it in `.cursor/rules/backend.md`** (add one line):

```markdown
Always read /MEMORY.md at the start of every session. It contains current project state.
```

### Maintaining MEMORY.md

Add updating `MEMORY.md` to your team's PR checklist:
- New architectural decision? Add it.
- Feature branch created? Document it.
- Known issue discovered? Log it.
- Feature merged? Remove the WIP entry.

---

## Skills Files

### What They Are

Skills files are opt-in instruction playbooks. Unlike rules (always active), a skill is only read when you (or a rule) explicitly tells the AI to read it. They contain detailed, step-by-step instructions for a specialized task.

**This repo already uses skills** — see `.cursor/skills-cursor/`.

### When to Create a Skill

Create a skill for tasks that:
- Are complex enough to need a multi-step recipe
- Happen infrequently enough that you don't want them in the always-on rules
- Have a very specific output format you want to enforce

### Example Skills for GymBros

**`scaffold-cqrs-feature.md`** — a detailed recipe for adding a new CQRS command or query:
```markdown
# Skill: Scaffold a CQRS Feature

When asked to scaffold a new CQRS feature for this project:

1. Create the command/query record in Features/[Domain]/[Queries|Commands]/[Name]/
2. Create the handler in the same folder
3. Create or reuse a DTO in Features/[Domain]/
4. Add the endpoint to the relevant controller (thin — only _mediator.Send)
5. Register nothing in DI — MediatR auto-discovers handlers
6. Follow the pattern in Features/Exercises/Queries/GetExercises/ exactly

Do not: add business logic to the handler, use EF Core directly, return the entity.
```

**`add-expo-screen.md`** — recipe for adding a new Expo Router screen:
```markdown
# Skill: Add a New Expo Router Screen

When adding a new screen:
1. Create the file in app/(tabs)/ for tab screens or app/ for other screens
2. Export a default React component
3. Add the TanStack Query hook to lib/queries.ts first
4. Use useQuery for reads, useMutation for writes
5. Follow the theme constants in constants/Colors.ts and constants/theme.ts
6. Do not inline styles — use StyleSheet.create at the bottom of the file
```

### How to Use a Skill

In your prompt:
```
Read and follow the skill at .cursor/skills-cursor/scaffold-cqrs-feature.md.
Add a GetExerciseById query for the Exercises domain.
```

Or reference it in a rule so the AI uses it automatically when the context is relevant.

---

## The Three-Layer Context System

| Layer | File | When active | Who updates |
|---|---|---|---|
| Rules | `.cursor/rules/*.md` | Always | Team, rarely |
| Memory | `MEMORY.md` | Always (if in rule) | Team, every sprint |
| Skills | `.cursor/skills-cursor/*.md` | On demand | Team, occasionally |

Think of it as:
- **Rules** = the employee handbook (never changes, always applies)
- **Memory** = the daily standup notes (changes constantly, always relevant)
- **Skills** = the specialized training manual (read when doing that specific task)

---

## Common Mistakes

- **Putting fast-changing state in rules** — "we're currently building 2FA" shouldn't be in a rule. It belongs in `MEMORY.md`.
- **Not maintaining MEMORY.md** — a stale memory file is worse than no memory file. It actively misleads the AI.
- **Creating skills for things that belong in rules** — if a convention applies to every session, put it in a rule. Skills are for task-specific recipes.
- **Not referencing MEMORY.md from a rule** — the file exists but the AI won't read it unless instructed to.
