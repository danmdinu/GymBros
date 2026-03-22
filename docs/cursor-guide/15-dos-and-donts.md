# Practice 15 — Dos and Don'ts: Quick-Reference Cheat Sheet

Keep this open during pairing sessions. Print it. Put it on the wall.

---

## Mode Selection

| Do | Don't |
|---|---|
| Use Ask (CMD+L) to explore and plan | Jump straight to Agent for anything non-trivial |
| Switch to Agent (CMD+I) only once you have a plan | Use Agent to answer "how should I..." questions |
| Use Tab completion for edits under 5 lines | Open Agent for one-liner changes |

---

## Prompting

| Do | Don't |
|---|---|
| Use a template from `docs/cursor-guide/prompts/` | Start from scratch every time |
| Specify exactly which files to create or modify | Say "update the relevant files" |
| Include "Do not touch any other files" | Let the Agent roam freely |
| Point to an existing example with @-mentions | Describe what you want abstractly |
| State what NOT to do (anti-patterns) | Assume the AI knows your preferences |
| Break features into one-layer-at-a-time prompts | Ask for a full feature in one prompt |

---

## Context Management

| Do | Don't |
|---|---|
| Close unrelated tabs before starting a session | Leave 10 unrelated files open |
| @-mention specific files or small folders | @codebase on every prompt |
| Use @codebase for discovery, then narrow down | Use @codebase for implementation |
| Use @docs for library documentation | Describe library behavior from memory |

---

## Cost Control

| Do | Don't |
|---|---|
| Plan in Ask before executing in Agent | Execute blind — planning is cheap |
| Write surgical edit prompts ("only change X") | Ask for file rewrites when adding small things |
| Tell the AI to skip extended reasoning for well-defined tasks | Let extended thinking run on boilerplate |
| Maintain rules to avoid re-explaining conventions | Re-explain the same conventions every session |
| Reuse templates to avoid clarification back-and-forth | Free-form prompt for repeatable tasks |

---

## TDD

| Do | Don't |
|---|---|
| Write failing tests before asking the Agent to implement | Ask the Agent to write both tests and implementation |
| Define test behavior based on your requirements | Accept tests that just mirror the implementation |
| One assertion per test | Pack multiple assertions into one test |
| Confirm tests are RED before asking Agent to make them GREEN | Skip the RED step |

---

## Reviewing AI Output

| Do | Don't |
|---|---|
| Read every diff before accepting | Accept diffs because they "look right" |
| Check DI registrations after adding any new service | Assume the Agent remembered to wire things up |
| Use Ask to review the diff against the rules | Skip review on small changes |
| Reject and re-prompt when something is wrong | Ask the Agent to "fix the issues" in the same session |

---

## Memory and Context Files

| Do | Don't |
|---|---|
| Update MEMORY.md when architectural decisions change | Let MEMORY.md go stale |
| Reference MEMORY.md from your rule file | Create the file but never link it |
| Put stable conventions in rules | Put current WIP state in rules |
| Create skills for specialized, infrequent tasks | Create skills for things that belong in rules |

---

## The 30-Second Pre-Session Checklist

Before every Agent session:

- [ ] Do I have a plan? (from Ask mode or from the team)
- [ ] Do I have the right @-mentions ready? (specific files, not broad folders)
- [ ] Have I closed unrelated tabs?
- [ ] Am I using a prompt template or a precise custom prompt?
- [ ] Do I know which files this session should touch? (list them)
- [ ] Do I know what this session should NOT touch?

---

## Prompt Quality Scoring

Before sending a prompt, score it:

| Criterion | Score |
|---|---|
| Specific files listed or @-mentioned | +2 |
| Points to an existing example | +2 |
| Includes anti-patterns / what NOT to do | +1 |
| "Do not touch any other files" included | +1 |
| Uses a template | +2 |
| Vague ("add 2FA to the app") | -3 |
| No context | -2 |

**8+ points** → send it.  
**4–7 points** → add a template or an @-mention.  
**Below 4** → use Ask to plan first, then write a better prompt.

---

## The One Rule

> If you wouldn't accept this code from a junior developer without reading it, don't accept it from the AI without reading it.
