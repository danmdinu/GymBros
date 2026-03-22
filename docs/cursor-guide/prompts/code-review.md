# Prompt Template: AI-Assisted Code Review

> Copy this template, fill in the [PLACEHOLDERS], and paste into a Cursor Ask session (CMD+L).
> Do NOT use Agent — this is a read-only review, not an edit session.

---

## Usage

Use this before opening a PR. Paste the git diff of your changes and ask the AI to review.

Get your diff:
```bash
git diff main...HEAD
```

Or for a single file:
```bash
git diff main...HEAD -- path/to/file.cs
```

---

## Template

```
@.cursor/rules/backend.md

I'm about to open a PR for [FEATURE_DESCRIPTION]. Please review the following diff:

---
[PASTE GIT DIFF HERE]
---

Check for:

1. Logic errors
   - Does the implementation match the stated intent?
   - Are there any conditions that are inverted or off-by-one?
   - Are all code paths handled (including the null/not-found case)?

2. Rule violations (@.cursor/rules/backend.md)
   - Is there any business logic in a controller?
   - Is DateTime.Now used anywhere?
   - Are entities returned directly instead of DTOs?
   - Is EF Core used directly in the Application layer?

3. Missing DI registrations
   - Are any new services, validators, or behaviors created but not registered?

4. Security issues
   - Is any sensitive data (secrets, tokens) logged or returned in an insecure way?
   - Are there any authorization gaps?

5. Test coverage gaps
   - Which behaviors in this diff have no test coverage?
   - What's the most important thing to test that isn't tested?

6. Edge cases
   - What inputs or states could cause unexpected behavior?
   - What happens if the database operation fails?

Be specific. Reference the relevant code when pointing out issues.
```

---

## Example (2FA feature review)

```
@.cursor/rules/backend.md

I'm about to open a PR for the TOTP 2FA feature (Enable2FA + Verify2FA + Disable2FA).
Please review the following diff:

---
[paste diff here]
---

Check for:
1. Logic errors — especially in the TOTP verification flow
2. Rule violations per our backend.md
3. Missing DI registrations — TotpService, validators, ValidationBehavior
4. Security: is the TOTP secret ever logged? Is it returned in a secure way?
5. Edge cases: what if 2FA is already enabled when Enable2FA is called?
   What if the user calls Verify2FA before Enable2FA?
6. Test coverage: which domain methods have no tests?
```

---

## After the Review

For each issue the AI raises:
1. Decide if it's a real issue or a false positive
2. Fix real issues before pushing
3. For false positives, note why they're acceptable (this becomes future MEMORY.md content)

Do not ask the AI to fix the issues in the same Ask session — start a new targeted Agent session for each fix.
