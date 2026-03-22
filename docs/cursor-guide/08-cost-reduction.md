# 08 — Cost Reduction

**Principle:** Most cost comes from a few bad habits — fixing them also makes the AI more accurate.

---

## How

| Habit | Impact |
|---|---|
| **Tab completion** for edits under 5 lines | Free, instant — use it constantly |
| **Ask before Agent** | Planning is 5–10x cheaper than a misdirected execution |
| **Surgical prompts** ("only change X, don't rewrite the file") | Avoids regenerating hundreds of unchanged lines |
| **Close unrelated tabs** | Every open file is silently added to context |
| **Rules eliminate repetition** | Without rules, you re-explain conventions every session |
| **Reuse templates** | Prevents clarification back-and-forths |

### Reasoning / Thinking Budget
Claude has an "extended thinking" mode — expensive, and often unnecessary.  
For well-defined tasks where the pattern already exists in the codebase, add this to your prompt:

```
The conventions are already defined in @.cursor/rules/backend.md and the example
at @[path/to/example]. Follow the pattern directly — no need for extended analysis.
```

Reserve extended thinking for genuinely open-ended design questions.

## Demo

> Show a before/after prompt:

**Before** (expensive): `Rewrite ExercisesController.cs to add a GetExerciseById endpoint.`  
**After** (surgical): `Add one endpoint to ExercisesController.cs. Keep all existing code unchanged. Only add: GET /api/exercises/{id} → calls _mediator.Send(new GetExerciseByIdQuery(id))`

## Watch out for

- Using Agent for curiosity questions — Ask is for exploration
- Long Agent sessions — accumulated context from rejected changes adds noise and cost; start fresh between tasks
- `@codebase` on every prompt — expensive; use specific @-mentions
