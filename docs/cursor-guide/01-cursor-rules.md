# 01 — Cursor Rules

**Principle:** Write your conventions once, get them for free in every session forever.

---

## How

- Rules live in `.cursor/rules/*.md` — Cursor injects them automatically into every chat and Agent session
- They prevent the AI from violating your patterns (wrong patterns, missing DI, logic in controllers)
- Each rule file covers one concern: backend conventions, frontend conventions, testing conventions
- This repo already has `.cursor/rules/backend.md` — open it and read the first 10 lines

## Demo

> Show `backend.md` on screen. Point to the "Things to Avoid" section.
> Then ask in CMD+L:

```
Look at the mobile/ folder and write a Cursor rule file for the frontend stack.
Cover: tech stack, folder structure, how TanStack Query hooks are structured,
how Zustand is used, and anti-patterns to avoid.
Match the format of .cursor/rules/backend.md.
Save it as .cursor/rules/frontend.md
```

## Watch out for

- Rules that are too vague ("write clean code") — be specific and actionable
- Putting fast-changing project state in rules — use a `MEMORY.md` for that (see Practice 10)
- Forgetting a frontend rule — without it, the AI gives generic mobile advice
