# 02 — Ask vs Agent

**Principle:** Ask is your architect. Agent is your developer. Never hire the developer before the architect has a plan.

---

## How

| Mode | Shortcut | Does | Cost |
|---|---|---|---|
| **Ask** | CMD+L | Explores, explains, plans — reads only | Low |
| **Agent** | CMD+I | Creates and modifies files | Higher |

- Use Ask to understand the codebase, map what needs to change, and validate the approach
- Switch to Agent only once you know exactly what to build and where
- A 2-minute Ask session prevents a 20-minute Agent mess

## Demo

> Open CMD+L and type:

```
@backend/src
I want to add a GetExerciseById endpoint. What files would I need to create
or modify? List them only — no code yet.
```

> The AI maps the territory. Now you know exactly what to put in the Agent prompt.

## Watch out for

- Opening Agent for anything exploratory ("how should I...") — use Ask
- Treating Ask answers as final — verify the plan makes sense before executing
- Skipping Ask on "small" features — the map is always worth 2 minutes
