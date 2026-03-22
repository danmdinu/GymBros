# Practice 12 — AI-Assisted Code Review

## Why It Matters

Code review is where most bugs are caught — but reviewers are busy, context-switching is expensive, and it's easy to miss things. AI can do a first pass before you even open a PR, catching the obvious issues and freeing your human reviewers to focus on the deeper ones.

This is not a replacement for human review. It's a complement that raises the baseline quality of every PR.

---

## How to Do It

### Before Opening a PR

1. Get the diff of your changes:

```bash
git diff main...HEAD
```

2. Open Ask mode (CMD+L)

3. Use the `code-review.md` template from `prompts/`:

```
@.cursor/rules/backend.md
@[relevant feature folder]

Here is the diff for my 2FA feature PR:

[paste diff here]

Review this code for:
1. Logic errors — does the implementation match the intended behavior?
2. Missing edge cases — what inputs or states are not handled?
3. Rule violations — anything that breaks our backend conventions?
4. Missing DI registrations — any new service that isn't wired up?
5. Security issues — anything that could be exploited?
6. Test coverage gaps — what should have a test but doesn't?

Be specific. Reference line numbers if possible.
```

4. Review the AI's findings. Fix the legitimate ones before pushing.

---

## What AI Code Review Catches Well

| Category | Examples |
|---|---|
| Rule violations | `DateTime.Now` instead of `IDateTimeProvider`, logic in controller |
| Missing registrations | New service not added to DI |
| Null reference risks | Method doesn't check if entity was found before calling methods on it |
| Incorrect HTTP codes | POST returning 200 instead of 201 |
| Missing validation | Command with no validator for user-supplied input |
| Obvious logic errors | Guard condition inverted, off-by-one in loops |

## What AI Code Review Misses

| Category | Why |
|---|---|
| Business requirement correctness | AI doesn't know your product spec |
| Performance at scale | Can spot N+1 queries but not real-world bottlenecks |
| Security subtleties | Timing attacks, subtle auth bypass patterns |
| Team conventions beyond rules | Undocumented tribal knowledge |
| Impact on other features | Cross-feature regressions |

---

## GymBros 2FA Review Example

After implementing the `Enable2FACommand` handler, paste the handler and ask:

```
@.cursor/rules/backend.md
Review this Enable2FACommand handler:

[paste handler code]

Check:
- Does it handle the case where the user is not found in the database?
- Does it handle the case where 2FA is already enabled?
- Is the TOTP secret returned securely (not logged)?
- Is SaveChangesAsync called?
- Is there any business logic that should be on the User entity instead?
```

The AI will often catch that:
- The handler doesn't throw `NotFoundException` if the user isn't found
- The secret might be logged if there's a generic logging behavior on all handler responses
- A business rule check (already enabled) belongs on `User.Enable2FA()`, not in the handler

---

## Adding Review to Your PR Template

Consider adding to your GitHub PR template:

```markdown
## AI Pre-Review
- [ ] Ran AI code review against backend rules
- [ ] Fixed identified issues before pushing
```

This makes the practice visible and accountable.

---

## Common Mistakes

- **Pasting the entire codebase for review** — paste only the changed files or the diff.
- **Treating AI review as a substitute for human review** — it's a first pass, not a final gate.
- **Not providing the rules file as context** — without the rules, the AI reviews against generic conventions, not yours.
- **Asking "is this code good?"** — too vague. Ask specific questions about specific concerns.
