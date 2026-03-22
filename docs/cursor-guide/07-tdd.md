# 07 — TDD with AI

**Principle:** Write the failing test first — the AI can't hallucinate a passing result.

---

## How

1. Write the test (define the expected behavior) — **RED**
2. Run it → confirm it fails because the code doesn't exist yet
3. Prompt Agent: "Make these tests pass. Do not modify the test file."
4. Run it → **GREEN**

- You own the tests. The AI owns the implementation.
- This is the most effective way to reduce AI-introduced bugs — a test is a machine-checkable spec
- One assertion per test. Name: `MethodName_Scenario_ExpectedResult`

## Demo

> In Ask mode, generate test stubs first:

```
@backend/src/WorkoutApp.Domain/Entities/UserProgress.cs

Write unit test stubs for UserProgress.CompleteCurrentWorkout().
Use xUnit + FluentAssertions. One test per behavior:
1. CurrentDayNumber increments by 1
2. When already at day 30, sets CompletedAt
3. When already completed, throws InvalidOperationException

Do not implement anything — stubs only, all marked with Assert.Fail("not implemented").
```

> Show the RED tests. Then run Agent to make them pass.

## Watch out for

- Letting the AI write both the tests and the implementation — it will write tests that pass its own code, not tests that verify your requirements
- Writing tests after implementation — defeats the purpose; tests should define behavior, not describe it
- Testing implementation details instead of observable behavior
