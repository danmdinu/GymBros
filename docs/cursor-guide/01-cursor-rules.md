# Practice 1 — Cursor Rules: Set Context Once, Benefit Forever

## Why It Matters

Every time you open a new Cursor session, the AI starts with zero knowledge of your project. Without guidance it will:
- Put `DateTime.Now` in domain entities (we use `IDateTimeProvider`)
- Add business logic to controllers (we use MediatR)
- Use data annotations instead of Fluent API
- Return entities directly instead of DTOs

You could correct it every single time — or you could write the rules once and never think about it again.

---

## The Three Types of Persistent AI Context in Cursor

### 1. Rules (`.cursor/rules/*.md`) — Always On

Rules are markdown files in `.cursor/rules/`. Cursor automatically injects them into every chat and Agent session in the workspace. They are the highest-leverage thing you can invest in as a team.

**Already exists in this repo:** [`.cursor/rules/backend.md`](../../.cursor/rules/backend.md)

Open it and look at what it contains — the tech stack, key principles, anti-patterns. Every session the AI opens in this repo already knows all of that, for free.

**Rule of thumb for rules:**
- Conventions that never change → rules
- Things every dev should know → rules
- Anti-patterns specific to your codebase → rules
- Architecture decisions → rules

### 2. Memory Files (`MEMORY.md`) — Team-Maintained Living State

A `MEMORY.md` is a markdown file you commit to the repo (typically at the root or in `docs/`) that tracks the *current state* of the project: in-progress work, recent architectural decisions, known issues, things the AI should be aware of right now.

You reference it from a rule so the AI always reads it:

```markdown
# In .cursor/rules/backend.md — add this line:
Always read /MEMORY.md before making suggestions. It contains current project state and decisions.
```

**Example entries:**

```markdown
# MEMORY.md

## Current WIP
- 2FA feature is in progress. Branch: feature/2fa
- TotpService lives in Infrastructure. Interface is ITotpService in Application.

## Architectural Decisions
- SMS-based 2FA is out of scope. TOTP only.
- TOTP secret is stored as plain text for now; encryption is a follow-up task.
- We do NOT use EF migrations — schema is managed via EnsureCreatedAsync.

## Known Issues
- UserProgress.CompleteCurrentWorkout() has no guard for plan already completed — fix pending.
```

Unlike rules (which are static), memory is updated as the project evolves. Think of it as the AI's project briefing.

### 3. Skills Files (`.cursor/skills-cursor/*.md`) — On-Demand Playbooks

Skills are opt-in — the AI only reads one when you (or a rule) explicitly tells it to. They contain step-by-step instructions for a specialized task.

**This repo already uses skills** (see `.cursor/skills-cursor/`). Examples of useful custom skills:
- `scaffold-cqrs-feature.md` — step-by-step recipe for adding a new query or command
- `add-expo-screen.md` — recipe for adding a new Expo Router screen with TanStack Query

**Difference summary:**

| | Rules | Memory | Skills |
|---|---|---|---|
| When active | Always | Always (if referenced in a rule) | On demand |
| Who updates | Team, rarely | Team, frequently | Team, occasionally |
| Purpose | Conventions | Current project state | Specialized task recipes |

---

## Live Demo: Create the Frontend Rule

We have `backend.md` but no `frontend.md`. The mobile app uses React Native, Expo, TanStack Query, and Zustand — and the AI doesn't know any of that by default.

**Demo prompt (use Ask mode, CMD+L):**

```
Look at the mobile/ folder in this project and write a Cursor rule file for the frontend.
It should cover: tech stack, folder structure, conventions for adding new screens (Expo Router),
how TanStack Query hooks are structured in lib/queries.ts, how Zustand is used,
and any anti-patterns to avoid. Follow the same format as .cursor/rules/backend.md.
```

The AI will read the mobile source and generate a rule. Review it, then save it as `.cursor/rules/frontend.md`.

---

## Common Mistakes

- **Writing rules that are too vague** — "write good code" is useless. Be specific: "all string columns must have `.HasMaxLength()` in their EF configuration."
- **Putting fast-changing state in rules** — use memory files for that. Rules should be stable.
- **Not reviewing the AI output** — rules shape every future session, so a bad rule compounds.
- **Not creating a frontend rule** — the backend rule does nothing for mobile sessions.
