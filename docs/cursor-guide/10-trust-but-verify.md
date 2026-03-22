# Practice 10 — Trust But Verify: Review Every Diff

## Why It Matters

The AI is a brilliant, fast, overconfident developer who doesn't know your codebase as well as you do. It will produce code that *looks* right, compiles, and follows most of your conventions — but has subtle omissions and mistakes that only a human reviewer catches.

Never accept a diff without reading it.

---

## The Review Workflow

```
Agent writes code
       ↓
Review the diff in Cursor (before accepting)
       ↓
Ask mode: "Does this follow our backend rules?" @.cursor/rules/backend.md
       ↓
If yes → accept
If no  → reject, fix the specific issue, try again (don't ask the Agent to "fix everything")
```

---

## The Most Common AI Omissions in This Repo

These are the things the AI most commonly gets wrong in the GymBros codebase, in order of frequency:

### 1. Forgetting DI Registration

The AI creates a new service (`TotpService`) but forgets to register it in `DependencyInjection.cs`. The code compiles. The app crashes at runtime with a "no service registered" error.

**Always check:** after adding any new service, interface, or behavior — is it registered?

The relevant file: [`backend/src/WorkoutApp.Application/DependencyInjection.cs`](../../backend/src/WorkoutApp.Application/DependencyInjection.cs)

### 2. Using `DateTime.Now` Instead of `IDateTimeProvider`

The rule says no `DateTime.Now` — inject `IDateTimeProvider`. The AI knows the rule but occasionally reverts. TOTP code verification involves timestamp comparison, so this is particularly relevant.

**Always check:** any handler or service that does time-based logic.

### 3. Putting Logic in the Controller

A common drift: the AI puts an `if` statement or a validation check in the controller instead of the handler or domain model.

**Always check:** controllers should contain only `_mediator.Send(...)` calls and HTTP response mapping.

### 4. Incorrect Return Codes

The rules specify `201 Created` for POST endpoints that create resources. The AI sometimes returns `200 OK` for everything.

**Always check:** POST endpoints that create something should return `201`.

### 5. Missing `AsNoTracking()` on Read Queries

All read-only queries should use `.AsNoTracking()` for performance. The AI sometimes omits it.

**Always check:** any handler that queries but doesn't modify data.

---

## The Ask Mode Review Trick

After an Agent session produces a diff, before accepting:

1. Keep the diff visible
2. Open CMD+L (Ask)
3. Paste or describe the change, then ask:

```
@.cursor/rules/backend.md
I just generated this code for [description]. Review it against our backend rules.
Specifically check:
- Is anything registered in DI that was added but not wired up?
- Is there any business logic in the controller?
- Is DateTime.Now used anywhere?
- Are all read queries using AsNoTracking?
- Do POST endpoints return 201?
```

The AI will catch its own omissions about 80% of the time when asked to review against the rules explicitly.

---

## GymBros 2FA Demo

**Live demo moment:** After the Agent generates the `TotpService` and `Enable2FACommand` handler (Step 3–4 of Practice 6), deliberately do not check `DependencyInjection.cs`.

Show the team what happens when you run `dotnet run` — the app crashes because `ITotpService` is not registered.

Then show the review prompt above catching it.

This is the most memorable teaching moment in the session: the code looked right, compiled, and still failed.

---

## What Rules Don't Catch

Rules prevent the majority of systematic errors. But they can't prevent:

- **Logic errors** — the handler calls `user.Disable2FA()` when it should call `user.ConfirmTwoFactor()`
- **Off-by-one errors** — code validates "greater than 5 characters" instead of "exactly 6"
- **Missing error cases** — handler doesn't handle the "user not found" case
- **Security issues** — timing attacks in TOTP verification (compare-in-constant-time)

These require human review. The AI is a fast junior developer — it needs a senior to sign off.

---

## Common Mistakes

- **Accepting a diff because it looks similar to the example** — similarity is not correctness.
- **Accepting a large diff all at once** — review file by file, not as one block.
- **Only checking if it compiles** — compilation is the lowest bar. Runtime correctness and security are higher bars.
- **Not using the Ask mode review trick** — it takes 30 seconds and catches most issues.
