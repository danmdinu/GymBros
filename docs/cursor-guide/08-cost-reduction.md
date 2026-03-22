# Practice 8 — Cost Reduction: Ship Fast Without Burning Budget

## Why It Matters

Cursor with Claude is powerful but not free. A team of 8 developers using Agent mode carelessly can burn through budget fast. The good news: most of the cost comes from a handful of habits that are easy to change — and fixing them makes the AI *more accurate*, not less.

---

## The Biggest Levers

### 1. Control the Reasoning Budget

Claude has an "extended thinking" mode where it reasons through a problem step by step before responding. This produces better output for genuinely complex, open-ended problems — but it costs significantly more tokens and is often unnecessary.

**When extended thinking helps:**
- Designing architecture for an unfamiliar problem
- Debugging a tricky multi-layer issue
- Open-ended questions: "What's the best approach for X?"

**When it's wasted:**
- Implementing a well-defined task where the pattern is already in the codebase
- Following a prompt template you've used before
- Any task where the rules + template already define the expected output

**How to reduce it:**

For well-defined tasks, add this to your prompt:
```
The conventions for this task are already defined in @.cursor/rules/backend.md
and the example in @Features/Progress/Commands/CompleteWorkout/.
Use the pattern directly — no need for extended analysis.
```

Or be direct:
```
Implement [task]. Follow the existing pattern exactly. Do not analyze alternatives.
```

When the AI already has everything it needs, it doesn't need to think — it needs to type.

### 2. Use Tab Completion for Small Edits

Cursor Tab (the grey autocomplete suggestions) is free and instant. Use it constantly for:
- Filling in method arguments
- Completing repetitive patterns (e.g. adding fields to a DTO)
- Renaming variables
- Writing boilerplate (using statements, constructor assignments)

**Rule:** If the edit is less than 5 lines and follows an obvious pattern, use Tab. Don't open Agent.

### 3. Use Ask Mode Before Agent Mode

Ask mode (CMD+L) is significantly cheaper than Agent mode (CMD+I). Every token you spend in Ask to plan saves you from multiple expensive Agent attempts that go in the wrong direction.

A 3-message Ask conversation that produces a clear plan will almost always be cheaper than a 2-attempt Agent session with course corrections.

### 4. Write Surgical Edit Prompts

Instead of regenerating a large file, tell the AI exactly what to change:

**Expensive:**
```
Rewrite UserController.cs to add the 2FA endpoints.
```
→ AI regenerates the entire controller (hundreds of lines)

**Cheap:**
```
Add three endpoints to UserController.cs. Keep all existing endpoints unchanged.
Only add the following methods: [description]
```
→ AI makes targeted additions, no unnecessary regeneration

### 5. Close Unrelated Tabs

Every open file in your editor is silently included in the Agent context. With 10 tabs open, you might be paying for 3 relevant files and 7 irrelevant ones.

Before starting a session:
- Close everything unrelated to the current task
- Open only the files you'll @-mention

This can reduce your per-session token cost by 30–50%.

### 6. Rules Amortize Cost Over Every Session

Without rules, you spend tokens on every session re-explaining that `DateTime.Now` is forbidden and that handlers should call `IAppDbContext` instead of EF Core directly.

With a rule file, you pay that explanation cost once when writing the rule — and then it's free forever.

Calculate it this way: if a convention explanation costs 200 tokens and you have 10 developers doing 3 sessions per day, that's 6,000 tokens/day on re-explanation alone. A rule file reduces that to ~0.

### 7. Reuse Templates

Templates front-load everything the AI needs to avoid back-and-forth. A prompt without context produces a response that's wrong → you correct it → AI tries again. Each correction round costs tokens.

A well-written template reduces the average conversation to 1–2 exchanges instead of 5–6.

---

## Cost Hierarchy (Cheapest to Most Expensive)

| Action | Relative Cost |
|---|---|
| Tab completion | Free |
| Ask mode (CMD+L) | Low |
| Agent with surgical prompt + good context | Medium |
| Agent with vague prompt, many open files | High |
| Agent with extended thinking on a well-defined task | Very High |
| Repeated Agent corrections after a bad initial prompt | Very High |

---

## Quick Checklist Before Starting an Agent Session

- [ ] Do I have a plan (from Ask mode)?
- [ ] Have I closed unrelated tabs?
- [ ] Am I using a template or a precise prompt?
- [ ] Am I @-mentioning specific files, not whole directories?
- [ ] Is this task well-defined enough that the AI doesn't need to reason through options?

---

## Common Mistakes

- **Using Agent for exploration** — curiosity questions belong in Ask.
- **Leaving `@codebase` in every prompt** — expensive; use specific @-mentions instead.
- **Accepting a bad response and asking the AI to fix it** — better to cancel and write a better prompt.
- **Long Agent sessions** — session context accumulates. Start fresh between distinct tasks.
