# Practice 7 — Iterative Prompting: One Layer at a Time

## Why It Matters

The bigger the prompt, the more the AI has to guess. When you ask for an entire feature in one shot, the AI fills in the blanks with assumptions — and those assumptions are often wrong.

Smaller prompts give you more control, faster feedback loops, and better output.

---

## The Anti-Pattern: The Big Bang Prompt

```
Add TOTP 2FA to GymBros. Update the User entity, create the service,
add the commands, update the controller, and create the mobile screen.
Make sure it works end to end.
```

What happens:
- The Agent generates 15 files simultaneously
- You can't review the changes coherently
- An error in the domain propagates to every layer
- The Agent makes assumptions at each layer that conflict with each other
- You end up accepting a large diff with bugs you won't catch until runtime

---

## The Pattern: One Layer at a Time

```
Prompt 1: Domain only — User entity changes
         → review diff → accept
Prompt 2: EF config only
         → review diff → accept
Prompt 3: Infrastructure service only
         → review diff → accept
Prompt 4: Commands + handlers only
         → review diff → accept
Prompt 5: Controller endpoints only
         → review diff → accept
Prompt 6: Mobile screen only
         → review diff → accept
```

Each accepted diff becomes **trusted context** for the next prompt. The Agent builds on solid ground at every step.

---

## The Snowball Effect

When you work iteratively, the Agent accumulates correct, reviewed context:

- After Step 1, it knows exactly how `User.Enable2FA()` is structured
- After Step 3, it knows `ITotpService.Verify()` returns a `bool`
- After Step 4, it knows `Enable2FAResponse` has `QrCodeUri` and `Secret` fields

The mobile screen in Step 6 will be correct because it's building on 5 reviewed layers, not 5 guessed ones.

---

## Rules of Thumb

| Signal | What to do |
|---|---|
| Prompt touches more than 4 files | Split it |
| You're not sure what the output should look like | Use Ask first |
| The diff is longer than ~100 lines | Consider breaking into smaller steps |
| The feature has conditional logic at multiple layers | One layer per session |
| You're adding a new concept (new service, new pattern) | Do it alone first, then reference it |

---

## GymBros 2FA Example

**How Practice 6 is structured** — notice each step ends with "Do not touch any other files." This is the key phrase. It scopes the Agent to exactly one layer and prevents it from making "helpful" changes elsewhere.

The 6 steps in Practice 6 are not arbitrary — they follow the dependency order of Clean Architecture:

```
Domain (no dependencies)
  → EF Config (depends on Domain)
    → Infrastructure Service (depends on Application interfaces)
      → Commands/Handlers (depends on Domain + Infrastructure interfaces)
        → Controller (depends on Application commands)
          → Mobile (depends on API contract)
```

Going in this order means each step can only reference things that have already been reviewed and accepted.

---

## Common Mistakes

- **"Just one more thing"** — adding "also add the controller endpoint" to a domain prompt. Resist this.
- **Not reviewing between steps** — the whole point of iterating is to catch issues early.
- **Re-opening the same Agent session** — start fresh for each step. Long sessions accumulate context from rejected changes that can confuse the Agent.
- **Combining unrelated concerns** — "add the handler AND register it in DI" is fine (2 lines). "Add the handler AND the controller AND the mobile hook" is not.
