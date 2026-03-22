# Practice 2 — Plan (Ask) Before You Execute (Agent)

## Why It Matters

The single biggest mistake teams make with Cursor is jumping straight into Agent mode and typing "add 2FA to this app." The Agent will write code immediately — across multiple files, making architectural decisions on your behalf, potentially in ways that don't fit the project.

Two modes, two very different jobs:

| Mode | Shortcut | What it does | Cost |
|---|---|---|---|
| **Ask** (Chat) | CMD+L | Explores, explains, plans — reads code but writes nothing | Low |
| **Agent** | CMD+I | Executes — creates and modifies files | Higher |

Ask is your architect. Agent is your developer. You don't hire a developer before the architect has a plan.

---

## How to Do It

### Step 1 — Explore in Ask mode

Before writing a single line, open CMD+L and ask the AI to map the territory:

```
@backend/src I want to add TOTP-based 2FA to this project.
What files would need to change? Walk me through the layers
(Domain → Application → Infrastructure → API → Mobile) and
list exactly which files need to be created or modified.
Do not write any code yet.
```

The AI will give you a precise list. You can catch misunderstandings here — before they become code.

### Step 2 — Validate the plan

Review the list. Ask follow-up questions:

```
You said to modify UserController.cs. Should we add the 2FA endpoints there
or create a dedicated TwoFactorController? What fits better with the existing pattern?
```

### Step 3 — Switch to Agent for execution

Once you're satisfied with the plan, open CMD+I. Now you give the Agent a scoped, targeted instruction — not a vague feature request.

```
Add TotpSecret (string, nullable) and IsTwoFactorEnabled (bool, default false) fields
to the User entity in backend/src/WorkoutApp.Domain/Entities/User.cs.
Follow the existing entity patterns (private setters, factory methods).
Do not touch any other files.
```

---

## GymBros 2FA Example

**Bad prompt (straight to Agent):**
```
Add 2-factor authentication to GymBros using TOTP.
```
→ Agent invents its own architecture, ignores your conventions, creates 12 files you didn't ask for.

**Good flow:**
1. Ask: "What files need to change to add TOTP 2FA?" → get the list
2. Ask: "Does TotpService belong in Infrastructure or should it be abstracted behind an interface in Application?" → get the answer
3. Agent: "Add `TotpSecret` and `IsTwoFactorEnabled` to `User.cs`. Nothing else." → scoped, reviewable change

---

## Common Mistakes

- **Using Agent to explore** — if you're not sure what to do, use Ask first.
- **Treating Ask answers as gospel** — Ask tells you a plan; you still validate it before executing.
- **Mixing planning and execution in the same Agent session** — keep them separate. An Agent session that starts with "what should I do?" tends to hallucinate.
- **Skipping the plan for "small" features** — 2FA seemed small until it touched 12 files.
