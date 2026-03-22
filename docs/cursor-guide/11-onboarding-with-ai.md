# Practice 11 — Onboarding with AI: Understand Before You Build

## Why It Matters

A new developer joining the team used to spend days (or weeks) reading the codebase, asking questions, and slowly building a mental model of how everything fits together. With AI, that process compresses to hours.

But this only works if you know how to ask the right questions — and if the project has the right context artifacts (rules, memory file) in place.

---

## The Onboarding Workflow

### Day 1: Understand the architecture

Open Ask mode (CMD+L) and start with the big picture:

```
@backend/src @.cursor/rules/backend.md
I just joined this project. Give me a 5-minute architecture overview:
- What does this application do?
- What are the main layers and how do they depend on each other?
- What is the CQRS pattern used here and how does a typical request flow?
- What are the most important conventions I need to know before writing any code?
```

Then zoom into specific domains:

```
@backend/src/WorkoutApp.Domain/Entities/
Explain the domain model. What are the main entities, their relationships,
and any important business rules encoded in the entities?
```

### Day 2: Understand a specific feature end-to-end

```
@backend/src/WorkoutApp.Application/Features/Progress/
@backend/src/WorkoutApp.Api/Controllers/
Walk me through the complete flow for "completing a workout":
from the HTTP request hitting the controller, through the command handler,
to the domain entity method, and back.
```

### Day 3: Understand what's missing or in-progress

This is where `MEMORY.md` becomes critical. A new dev can read it to immediately know:
- What's currently being worked on
- What known issues exist
- What architectural decisions have been made and why

Without `MEMORY.md`, they ask their teammates — who have to interrupt their work to answer.

---

## GymBros 2FA Example

A new dev joining mid-sprint needs to understand the 2FA feature in progress. With a `MEMORY.md` that says:

```markdown
## Current WIP
- 2FA feature in progress on branch feature/2fa
- TotpService lives in Infrastructure/Services/
- Interface ITotpService is in Application/Common/Interfaces/
- TOTP only — no SMS
- secret stored as plain text for now, encryption is a follow-up
```

They get up to speed in 30 seconds. Without it, they spend 20 minutes reading the branch diff and asking questions.

---

## Useful Onboarding Prompts

**Understanding authentication:**
```
@backend/src/WorkoutApp.Infrastructure/Auth/
How does authentication work in this project? Explain the Firebase token verification
flow and how a user identity is extracted from the JWT in request handlers.
```

**Understanding a specific entity:**
```
@backend/src/WorkoutApp.Domain/Entities/UserProgress.cs
Explain this entity. What does it represent, what are its invariants,
and what does CompleteCurrentWorkout() do?
```

**Understanding the mobile API integration:**
```
@mobile/lib/
How does the mobile app communicate with the backend? Walk me through:
- How is the API base URL configured?
- How are queries/mutations structured?
- How is auth handled on the mobile side?
```

**Finding where to add something:**
```
@backend/src
I need to add a new read-only endpoint that returns a user's 2FA status.
Based on the existing patterns, what files do I need to create and where?
Give me the list before writing any code.
```

---

## Creating a Useful MEMORY.md

Add `MEMORY.md` to the repo root and reference it in the backend rule:

```markdown
# Project Memory

## What This Is
A 30-day fitness challenge app. Backend: .NET 8 Clean Architecture.
Mobile: React Native / Expo. Auth: Firebase.

## Current Sprint
- 2FA implementation (branch: feature/2fa) — in progress

## Architectural Decisions
- No EF migrations — using EnsureCreatedAsync (SQLite only project)
- TOTP 2FA only, no SMS
- Single workout plan ("30-Day Challenge") — no multi-plan support yet
- Mobile auth not yet connected to Firebase SDK — transitional state

## Known Issues
- UserProgress has no guard for plan already completed
- Mobile app uses deviceId in URLs — backend uses Firebase JWT (mismatch, fix in progress)

## Do Not Touch
- AppDbContextSeed.cs — seeding data is intentionally hardcoded
```

---

## Common Mistakes

- **Using `@codebase` for every onboarding question** — it's broad and expensive. Use folder-level @-mentions.
- **Asking vague questions** — "explain the project" gets a generic answer. "Explain the domain model entities and their relationships" gets a useful answer.
- **Not maintaining MEMORY.md** — it becomes stale and misleading. Add updating it to your PR checklist.
- **Onboarding devs without a frontend rule** — the AI's backend answers are great; without a frontend rule its mobile answers are generic.
