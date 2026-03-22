# Practice 4 — Prompt Templates: Standardize the Team's Vocabulary

## Why It Matters

If every developer on the team prompts the AI differently, you get inconsistent output. One dev gets a handler that follows your CQRS pattern perfectly; another gets one that puts EF Core logic directly in the controller. The difference is usually how they wrote the prompt.

Prompt templates are the solution: shared, versioned, committed-to-the-repo instructions that front-load everything the AI needs to produce consistent, correct output.

---

## What Makes a Good Template

A good prompt template includes:

1. **What to build** — the specific thing you want
2. **Where to look** — @-mentions pointing to the nearest relevant example
3. **Conventions to follow** — the key rules (or a reference to the rule file)
4. **What NOT to do** — explicit anti-patterns for this task
5. **Expected output** — what files should be created/modified

The template lives in `docs/cursor-guide/prompts/` and gets copy-pasted into a Cursor session. Your team fills in the `[PLACEHOLDER]` parts.

---

## The Templates in This Repo

All templates are in [`prompts/`](prompts/):

| Template | Use case |
|---|---|
| [`new-backend-feature.md`](prompts/new-backend-feature.md) | Add any new CQRS query or command |
| [`new-frontend-screen.md`](prompts/new-frontend-screen.md) | Add a new Expo Router screen with data fetching |
| [`write-unit-tests.md`](prompts/write-unit-tests.md) | Generate xUnit tests for a class or feature |
| [`add-validation.md`](prompts/add-validation.md) | Add FluentValidation to a command |
| [`code-review.md`](prompts/code-review.md) | Pre-PR self-review |
| [`enable-2fa.md`](prompts/enable-2fa.md) | The live demo template for building 2FA |

---

## How to Use a Template

1. Open the template file
2. Copy the full contents
3. Open a new Cursor Agent or Ask session (CMD+I or CMD+L)
4. Paste the template
5. Fill in the `[PLACEHOLDER]` parts
6. Send

That's it. The AI has everything it needs upfront, so there's no back-and-forth to clarify conventions.

---

## GymBros 2FA Example

The `enable-2fa.md` template is the one we use during the live demo. Open it and notice how it:
- Points to existing examples in the codebase with @-mentions
- States the 2FA scope explicitly (TOTP only, no SMS)
- Lists the exact files to create
- Calls out the anti-patterns relevant to this feature (no logic in controller, register in DI)

Compare the output you get from using the template vs. this bare prompt:
```
Add 2FA to the backend
```

---

## How to Create a New Template

When you find yourself writing the same prompt structure repeatedly, extract it:

1. Copy your last successful prompt
2. Replace the specific details with `[PLACEHOLDERS]`
3. Add comments explaining each section
4. Commit it to `docs/cursor-guide/prompts/`

Over time, the prompts folder becomes the team's shared prompt library — a living document that improves as the team learns what works.

---

## Common Mistakes

- **Templates that are too long** — if the template is 200 lines, devs won't use it. Keep them focused.
- **Not updating templates** — when the codebase pattern changes (e.g. you adopt a new convention), update the template.
- **Skipping the @-mentions** — the most important part of any template is pointing to the nearest existing example.
- **Not committing templates to the repo** — templates in a personal notes file help nobody.
