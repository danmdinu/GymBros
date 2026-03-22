# 12 — Cheat Sheet

Bookmark this. Keep it open during pairing sessions.

---

## Mode Selection

| Use Ask (CMD+L) | Use Agent (CMD+I) |
|---|---|
| Exploring the codebase | Implementing a planned feature |
| Planning what to build | Creating or modifying files |
| Reviewing a diff | Fixing a specific known issue |
| Asking "how should I..." | Executing "do exactly this" |

---

## Prompting

| Do | Don't |
|---|---|
| Point to the nearest existing example with @-mention | Describe what you want abstractly |
| List exact files to create and modify | Say "update the relevant files" |
| Include "Do not touch any other files" | Let the Agent roam |
| Break into one layer per session | Ask for a full feature in one shot |
| Use `prompts/build-feature.md` | Start from scratch every time |

---

## Context

| Do | Don't |
|---|---|
| Close unrelated tabs before a session | Leave 10 tabs open |
| @-mention specific files or small folders | `@codebase` for implementation |
| Use `@codebase` for discovery only | Use it on every prompt |

---

## Cost

| Do | Don't |
|---|---|
| Tab completion for edits under 5 lines | Open Agent for one-liners |
| Ask before Agent — planning is cheap | Jump to Agent without a plan |
| Tell AI to skip extended reasoning on well-defined tasks | Let thinking run on boilerplate |
| Write surgical prompts ("only add X") | Ask AI to rewrite whole files |

---

## Review

| Do | Don't |
|---|---|
| Read every diff before accepting | Accept because it "looks right" |
| Check DI registrations after any new service | Assume the Agent wired things up |
| Use Ask to review diff against the rules | Skip review on small changes |
| Start a new session to fix issues | Ask the same session to "fix everything" |

---

## Pre-Session Checklist

- [ ] Do I have a plan (from Ask mode)?
- [ ] Have I closed unrelated tabs?
- [ ] Am I using the `build-feature.md` template or a precise custom prompt?
- [ ] Do I have the right @-mentions? (specific files, not broad folders)
- [ ] Do I know exactly which files this session should and should NOT touch?

---

## Prompt Quality Check

Before sending, score your prompt:

| Has this | Points |
|---|---|
| @-mention pointing to an existing example | +2 |
| Exact file list (create + modify) | +2 |
| "Do not touch any other files" | +1 |
| Numbered requirements | +1 |
| Explicit constraints / anti-patterns | +1 |
| Vague description only | −3 |
| No @-mentions at all | −2 |

**6+ → send it. Below 6 → add an example @-mention or use the template.**

---

## The One Rule

> If you wouldn't accept this code from a junior developer without reading it,  
> don't accept it from the AI without reading it.
