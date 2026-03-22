# 13 — Adversarial Prompts

**Principle:** Framing the AI as a skeptic, red-teamer, or bounty hunter produces dramatically more critical output than asking it to "review the code."

---

## How

- Standard review prompts ("is this code good?") get optimistic answers — the AI tends to confirm what's there
- Incentive framing shifts the model's posture from *helpful* to *critical* — it actively hunts for problems
- Role-play personas (security researcher, skeptical senior, QA) unlock different failure modes
- Pre-mortem framing ("this already broke") bypasses the AI's tendency to be optimistic about code it just wrote

## Demo

Pick one and paste it into Ask (CMD+L) against the live demo diff from Practice 06:

**Bounty hunter:**
```
I'll give you $100 if you find a real bug in this code. Look hard.
[paste code]
```

**Skeptical senior:**
```
You are a senior engineer reviewing this PR. You are looking for reasons
to reject it. What are your strongest objections?
[paste diff]
```

**Pre-mortem:**
```
This feature shipped yesterday and caused a production incident.
Looking at this code, what most likely went wrong?
[paste code]
```

**Security researcher:**
```
You are a security researcher. Find every way a malicious user could
exploit this endpoint. Assume nothing is safe by default.
[paste controller method]
```

**Future maintainer:**
```
What are the top 3 things most likely to break about this implementation
in 6 months when requirements change?
[paste feature code]
```

## Watch out for

- The AI sometimes manufactures bugs that don't exist — always verify its findings against the actual code
- Works best on code *you* wrote, not code the AI just generated in the same session (it will be too lenient)
- Use these prompts for review only — not for implementation
