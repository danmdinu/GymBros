# Cursor + Claude — Team Best Practices Guide

> A live session guide for the GymBros engineering team. Every practice is grounded in this codebase with real examples.

---

## What This Is

This guide contains **15 practices** for using Cursor with Claude effectively as a team. It's designed for a live walk-through session but doubles as a leave-behind reference.

The thread running through every practice is a single feature we build together: **TOTP-based 2-Factor Authentication**. This feature touches every architectural layer — Domain, Application, Infrastructure, API, and Mobile — making it the perfect demo canvas.

---

## Session Agenda (~90 minutes)

| # | Practice | Time | Type |
|---|---|---|---|
| 1 | Cursor Rules — set context once, benefit forever | 5 min | Setup |
| 2 | Plan (Ask) before you execute (Agent) | 5 min | Concept |
| 3 | Context and @-mentions — surgical, not broad | 5 min | Concept |
| 4 | Prompt templates — standardize the team's vocabulary | 10 min | Setup |
| 5 | TDD with AI — tests as a machine-checkable contract | 10 min | Demo |
| 6 | Building 2FA end-to-end (the main live demo) | 20 min | Demo |
| 7 | Iterative prompting — one layer at a time | 5 min | Concept |
| 8 | Cost reduction + reasoning budget control | 5 min | Concept |
| 9 | Cross-cutting concerns (FluentValidation) | 5 min | Demo |
| 10 | Trust but verify — review every diff | 5 min | Concept |
| 11 | Onboarding with AI | 5 min | Demo |
| 12 | AI-assisted code review | 5 min | Demo |
| 13 | Multi-file / cross-layer changes | 5 min | Concept |
| 14 | Memory files vs Skills files | 5 min | Concept |
| 15 | Dos and Don'ts cheat sheet | 5 min | Reference |

---

## The 2FA Demo Feature

Every practice uses the same running example: adding TOTP-based 2-Factor Authentication to GymBros.

```
Domain layer      →  User entity gets TotpSecret + IsTwoFactorEnabled
Application layer →  Enable2FACommand, Verify2FACommand, Disable2FACommand + Handlers
Infrastructure    →  TotpService using OTP.NET
API layer         →  3 new endpoints on UserController
Mobile            →  New 2FA settings screen (QR code + 6-digit input)
```

We do **not** actually merge this into main during the session — we use it as a live sandbox to demonstrate each practice.

---

## How to Use This Guide

- Each practice is in its own numbered file: `01-cursor-rules.md` through `15-dos-and-donts.md`
- Each file has the same structure: **Why → How → GymBros Example → Common Mistakes**
- Ready-to-use prompt templates are in [`prompts/`](prompts/)
- The cheat sheet (`15-dos-and-donts.md`) is worth bookmarking

---

## Prerequisites

- Cursor installed with a Claude model selected (claude-sonnet recommended)
- The GymBros repo cloned and open in Cursor
- Familiarity with the project stack: .NET 8 / Clean Architecture / React Native / Expo

---

## File Index

| File | Summary |
|---|---|
| [01-cursor-rules.md](01-cursor-rules.md) | Rules, Memory files, Skills files |
| [02-plan-before-execute.md](02-plan-before-execute.md) | Ask mode vs Agent mode |
| [03-context-and-mentions.md](03-context-and-mentions.md) | @-mentions, context scoping |
| [04-prompt-templates.md](04-prompt-templates.md) | Shared, versioned prompt templates |
| [05-tdd-with-ai.md](05-tdd-with-ai.md) | Test-Driven Development with AI |
| [06-building-a-feature.md](06-building-a-feature.md) | 2FA end-to-end demo walkthrough |
| [07-iterative-prompting.md](07-iterative-prompting.md) | Small steps, not big bangs |
| [08-cost-reduction.md](08-cost-reduction.md) | Tokens, reasoning budget, Tab completion |
| [09-cross-cutting-concerns.md](09-cross-cutting-concerns.md) | FluentValidation on 2FA commands |
| [10-trust-but-verify.md](10-trust-but-verify.md) | Review workflow |
| [11-onboarding-with-ai.md](11-onboarding-with-ai.md) | Understanding a codebase fast |
| [12-code-review.md](12-code-review.md) | Pre-PR self-review |
| [13-multi-file-changes.md](13-multi-file-changes.md) | Cross-layer Agent sessions |
| [14-memory-and-skills.md](14-memory-and-skills.md) | Persistent AI intelligence |
| [15-dos-and-donts.md](15-dos-and-donts.md) | Quick-reference cheat sheet |
| [prompts/](prompts/) | 6 ready-to-use prompt templates |
