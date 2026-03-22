# Practice 13 — Multi-File / Cross-Layer Changes

## Why It Matters

Some changes genuinely need to touch multiple files at once. The Agent handles this well — but only when you give it a clear, bounded scope. Without explicit boundaries, it will "helpfully" touch files you didn't intend, propagate mistakes across layers, and produce a diff that's impossible to review coherently.

The key phrase is: **"Do not touch any other files."**

---

## When Multi-File Agent Sessions Are Appropriate

| Situation | Approach |
|---|---|
| Adding a field to an entity + its EF config | 2 files, one session |
| Adding a command + its handler (same folder) | 2 files, one session |
| Renaming a method across all call sites | Use "replace all" with careful context |
| Adding a new layer interface + its implementation | 2 files, one session |
| A full feature (entity + service + commands + controller) | Split into separate sessions (see Practice 7) |

---

## How to Scope Multi-File Sessions

### List the exact files

Don't describe what to change — list the files:

```
Make changes to these two files only. Do not touch anything else.
1. backend/src/WorkoutApp.Domain/Entities/User.cs
2. backend/src/WorkoutApp.Infrastructure/Persistence/Configurations/UserConfiguration.cs

In User.cs: add TotpSecret (string?, nullable) and IsTwoFactorEnabled (bool, default false)
In UserConfiguration.cs: map these fields with Fluent API. TotpSecret max length 64.
```

### Use "Do not touch any other files"

This phrase consistently produces more contained diffs. Without it, the Agent may update related files it "thinks" should change.

### Specify what to leave unchanged

If the file has existing code you want preserved:

```
Add these three endpoints to UserController.cs.
Keep all existing endpoints (GetMe, etc.) exactly as they are.
Only add the new methods at the bottom of the class.
```

---

## The Checkpoint Pattern

For larger multi-file changes, use checkpoints:

1. Run Agent for File 1 and File 2 only
2. Review and accept
3. Run a new Agent session for File 3 and File 4 (which can now reference the accepted changes)
4. Continue

This is better than one giant session because:
- Each diff is small and reviewable
- Mistakes in early files are caught before they propagate
- You can stop and pivot if the approach isn't working

---

## GymBros 2FA Example

**Appropriate as a single session** — updating `User.cs` + `UserConfiguration.cs` together:

```
@backend/src/WorkoutApp.Domain/Entities/User.cs
@backend/src/WorkoutApp.Infrastructure/Persistence/Configurations/UserConfiguration.cs

Update these two files for TOTP 2FA support. Do not touch any other files.

In User.cs:
- Add TotpSecret (string?, nullable, private setter)
- Add IsTwoFactorEnabled (bool, private setter, default false)
- Add Enable2FA() → returns string (the secret), throws if already enabled
- Add ConfirmTwoFactor() → sets IsTwoFactorEnabled = true
- Add Disable2FA() → clears TotpSecret, sets IsTwoFactorEnabled = false

In UserConfiguration.cs:
- Map TotpSecret as nullable string with max length 64
- Map IsTwoFactorEnabled as non-nullable bool with default value false
```

**Not appropriate as a single session** — all 6 layers of the 2FA feature. That's Practice 6's job: 6 separate sessions.

---

## Reviewing Multi-File Diffs

Cursor shows multi-file diffs as separate file tabs. Review each file independently:

1. Open the first file's diff — does it look right in isolation?
2. Open the second file's diff — does it reference the first file correctly?
3. Are there any files changed that you didn't expect? (This is a red flag — reject and re-prompt with tighter scope)

---

## Common Mistakes

- **Not listing the exact files** — "update the entity and its config" gives the AI too much discretion about what "its config" means.
- **Accepting without checking for unexpected file changes** — always check the diff list for surprise changes.
- **Multi-file session for a full feature** — this always produces lower quality than iterating layer by layer.
- **Reopening the same session to add more files** — start a fresh session. Long sessions accumulate context from both accepted and rejected changes.
