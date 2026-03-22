# 04 — Prompt Structure

**Principle:** A prompt that front-loads context, scope, and constraints gets the right answer on the first try.

---

## How

Every good prompt has these 7 parts:

```
1. CONTEXT       @-mention the nearest existing example
2. WHAT          One sentence — what to build
3. CREATE        List every new file (full path)
4. MODIFY        List every file to change + what to change
5. DO NOT TOUCH  Files the AI must not modify
6. REQUIREMENTS  Numbered list — specific behaviors, signatures, edge cases
7. CONSTRAINTS   Anti-patterns to avoid (mirrors your rule file)
```

The full template is in [`prompts/build-feature.md`](prompts/build-feature.md).

- The most important part is **#1 — the nearest existing example**. The AI learns from real code faster than from any description.
- **#5 — Do Not Touch** is the most commonly skipped part and the most commonly regretted.

## Demo

> Open `prompts/build-feature.md` on screen.
> Walk through each section and explain what goes in it.
> Then fill it in together for `GetExerciseById` — this becomes Practice 06.

## Watch out for

- Vague prompts ("add auth to this") — no scope, no example, no constraints = bad output
- Skipping the example @-mention — "follow standard .NET patterns" is not the same as "follow this file"
- Combining multiple features in one prompt — one feature per prompt (see Practice 05)
