# 09 — Trust but Verify

**Principle:** The AI is a fast, overconfident junior developer — review every diff before accepting.

---

## How

- Read every diff before accepting — compiling is the minimum bar, not the review
- After an Agent session, open Ask (CMD+L) and run a quick diff review:

```
@.cursor/rules/backend.md
Review this diff against our backend rules. Check for:
- Any business logic in a controller?
- DateTime.Now used anywhere?
- Any new service created but not registered in DI?
- All read queries using AsNoTracking?
```

- Fix issues by starting a **new** targeted Agent session — not by continuing the same one

### Most Common AI Omissions in This Repo

| Issue | Where to check |
|---|---|
| New service not registered in DI | `Application/DependencyInjection.cs` |
| Logic in controller instead of handler | `Api/Controllers/` |
| `DateTime.Now` instead of `IDateTimeProvider` | Any handler doing time operations |
| Missing `AsNoTracking()` on read queries | Query handlers |
| POST returning 200 instead of 201 | Controller endpoints |

## Demo

> After the live demo in Practice 06, run the Ask review prompt above against the diff.
> The AI will usually catch at least one minor issue — this is the teaching moment.

## Watch out for

- Accepting large diffs in one go — review file by file
- Asking the AI to "fix all the issues" in the same session — cancel and re-prompt with the specific fix
- Relying only on compilation — runtime correctness and security require human eyes
