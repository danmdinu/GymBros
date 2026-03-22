# 10 — Memory & Skills Files

**Principle:** Rules handle what never changes. Memory tracks what's changing right now. Skills are reusable playbooks for specific tasks.

---

## How

| File | Where | Always active? | Purpose |
|---|---|---|---|
| **Rule** | `.cursor/rules/*.md` | Yes | Stable conventions |
| **Memory** | `MEMORY.md` (repo root) | Yes, if referenced in a rule | Current project state, decisions, WIP |
| **Skill** | `.cursor/skills-cursor/*.md` | On demand | Step-by-step recipe for a specialized task |

### Memory File Pattern

Create `MEMORY.md` at the repo root and add one line to your rule:
```
Always read /MEMORY.md before making suggestions.
```

Keep it updated with:
- Current work in progress and active branches
- Architectural decisions made (and why)
- Known issues and things to avoid

### Skills File Pattern

A skill is read only when you tell the AI to:
```
Read and follow the skill at .cursor/skills-cursor/scaffold-cqrs-feature.md.
Add a GetProgressSummary query.
```

Useful for task recipes that are too detailed for a rule but too specialized to explain from scratch each time.

## Demo

> Show `MEMORY.md` concept by drafting 3 lines live:
```
## WIP
- GetExerciseById added in today's session (not yet merged)
## Decisions
- No EF migrations — using EnsureCreatedAsync only
```

## Watch out for

- Putting WIP state inside a rule — rules are for conventions, memory is for state
- Not updating `MEMORY.md` — a stale memory file actively misleads the AI
