# Practice 3 — Context and @-Mentions: Surgical, Not Broad

## Why It Matters

Every file you put in context costs tokens. More importantly, **irrelevant context confuses the AI** — it tries to reconcile every file you've given it, which leads to less precise answers and suggestions that drift from what you actually need.

The goal: give the AI exactly what it needs to answer your question, and nothing more.

---

## The @-Mention Types

| Mention | What it includes | When to use it |
|---|---|---|
| `@filename.cs` | That single file | When you're working on one specific file |
| `@folder/` | All files in the folder | When a feature spans a whole directory |
| `@codebase` | Semantic search across the whole repo | When you don't know where something lives |
| `@docs` | External documentation (URLs or local docs) | When referencing library/framework docs |
| `@git` | Recent git diff | When asking about changes you just made |

---

## How to Do It

### Be specific about scope

Instead of attaching the whole Application layer when adding a new command, attach only the nearest relevant example:

```
@backend/src/WorkoutApp.Application/Features/Progress/Commands/CompleteWorkout/
Add a Verify2FACommand following the same pattern as CompleteWorkoutCommand.
```

The AI now has a concrete example and nothing else. It will pattern-match exactly.

### Use `@codebase` for discovery, then narrow down

```
@codebase Where is the User entity defined and what fields does it have?
```

The AI searches semantically. Once it tells you it's in `WorkoutApp.Domain/Entities/User.cs`, your next prompt uses the specific file:

```
@WorkoutApp.Domain/Entities/User.cs Add TotpSecret and IsTwoFactorEnabled fields.
```

### Close unrelated tabs

Cursor automatically includes your open editor tabs in context. If you have `WorkoutDto.cs`, `AppDbContextSeed.cs`, and `index.html` open while working on 2FA, those files are silently included in every message. Close what you're not using.

---

## GymBros 2FA Examples

**Too broad (costs more, less accurate):**
```
@backend/src Implement the Enable2FA command handler
```
→ The AI is now reading 30+ files including exercises, workouts, and seed data.

**Just right:**
```
@backend/src/WorkoutApp.Application/Features/Progress/Commands/CompleteWorkout/CompleteWorkoutCommandHandler.cs
@backend/src/WorkoutApp.Application/Common/Interfaces/IAppDbContext.cs
Create Enable2FACommandHandler following the same pattern. The handler should:
1. Load the current user by ID from IAppDbContext
2. Call user.Enable2FA() which returns the TOTP secret
3. Save changes
4. Return the secret as a base32 string for QR code generation
```

**Using `@docs` for the OTP.NET library:**
```
@docs https://github.com/kspearrin/Otp.NET
Implement TotpService using OTP.NET. It should verify a 6-digit code against a base32 secret.
```

---

## The Open Files Trap

Cursor includes any file that is **open in your editor** as part of the context, even if you don't @-mention it. This is a hidden cost many developers don't know about.

Before starting an Agent or Ask session:
1. Close all tabs unrelated to the current task (CMD+W)
2. Open only the files you need as reference
3. Then start the session

This can cut your token usage by 30–50% on complex sessions.

---

## Common Mistakes

- **@codebase on every prompt** — use it for discovery, then switch to file-level mentions.
- **Forgetting about open tabs** — close unrelated files before starting a session.
- **Under-specifying context** — if the AI gives a generic answer, you probably need to @-mention the relevant existing example from your codebase.
- **Over-specifying context** — attaching the whole `backend/src/` for a single-file change.
