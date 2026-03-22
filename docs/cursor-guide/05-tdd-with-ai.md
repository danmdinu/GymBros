# Practice 5 — TDD with AI: Tests as a Machine-Checkable Contract

## Why It Matters

When you ask an AI agent to implement a feature without tests, it guesses. It guesses at edge cases, at what "success" looks like, at how error states should behave. The code often looks right but is subtly wrong — and you won't know until production.

TDD flips this. When you write a failing test first and then ask the Agent to make it pass, you give it a **machine-verifiable contract**. The AI can't hallucinate a passing test result. Either the test passes or it doesn't.

This is the most effective way to reduce AI-introduced bugs.

---

## The TDD Loop with AI

```
1. You write the test (define the expected behavior)
2. Run it → RED (fails because the code doesn't exist yet)
3. Agent writes the implementation to make it pass
4. Run it → GREEN
5. Agent refactors if needed → still GREEN
6. Repeat for the next behavior
```

The key: **you own the test**. The AI owns the implementation. This separation is powerful.

---

## How to Do It

### Step 1 — Scaffold the test project first

This repo has no test project yet. Ask the Agent to create one:

```
@backend/src Create a new xUnit test project called WorkoutApp.Tests.
Add it to the solution. Add references to WorkoutApp.Domain and WorkoutApp.Application.
Add the Moq and FluentAssertions NuGet packages.
Follow standard .NET project conventions.
```

### Step 2 — Write the tests yourself (or with Ask mode)

Write what you expect the code to do. If you're unsure of the exact assertion syntax, use Ask (not Agent):

```
I want to write unit tests for User.Enable2FA() in WorkoutApp.Domain.
The method should: generate a TOTP secret, set IsTwoFactorEnabled to false
(pending verification), and return the secret. Give me the test stubs
in xUnit + FluentAssertions style. Do not implement the method itself.
```

### Step 3 — Run → confirm RED

```bash
dotnet test backend/
```

The tests should fail because `Enable2FA()` doesn't exist yet. This confirms your tests are actually testing something.

### Step 4 — Prompt Agent to make them pass

```
@backend/src/WorkoutApp.Tests/Domain/UserTests.cs
@backend/src/WorkoutApp.Domain/Entities/User.cs

The tests in UserTests.cs are currently failing because Enable2FA() and Verify2FA()
are not implemented on the User entity. Implement these methods on User.cs to make
all the tests pass. Do not modify the test file. Follow the rich domain model
pattern already used in this entity (private setters, behavior methods).
```

### Step 5 — Run → confirm GREEN

```bash
dotnet test backend/
```

---

## GymBros 2FA Example

### Tests to write first (for `User.Enable2FA()`)

```csharp
public class UserTests
{
    [Fact]
    public void Enable2FA_ShouldGenerateTotpSecret()
    {
        var user = User.Create("uid", "test@example.com");
        var secret = user.Enable2FA();
        secret.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Enable2FA_ShouldNotEnable2FAUntilVerified()
    {
        var user = User.Create("uid", "test@example.com");
        user.Enable2FA();
        user.IsTwoFactorEnabled.Should().BeFalse();
    }

    [Fact]
    public void Enable2FA_WhenAlreadyEnabled_ShouldThrow()
    {
        var user = User.Create("uid", "test@example.com");
        user.Enable2FA();
        user.ConfirmTwoFactor(); // assume this enables it
        var act = () => user.Enable2FA();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Verify2FA_WithCorrectCode_ShouldEnable()
    {
        var user = User.Create("uid", "test@example.com");
        user.Enable2FA();
        // actual verification is tested at integration level with real TOTP
        user.IsTwoFactorEnabled.Should().BeFalse(); // until confirmed
    }
}
```

These tests define the behavior contract precisely. The Agent now implements `Enable2FA()` on the `User` entity to make them pass — there's no ambiguity.

### What TDD catches that "build it then test it" misses

- **No guard for "already enabled"** — without a test for this, the Agent won't think to add it
- **Unclear return value** — should `Enable2FA()` return the secret or void? The test makes it explicit
- **Edge case: what if the secret generation fails?** — write a test for it and the AI must handle it

---

## Common Mistakes

- **Letting the Agent write the tests and the implementation** — this is the opposite of TDD. The Agent will write tests that pass its own implementation, not tests that define your requirements.
- **Writing tests after implementation** — defeats the purpose. The value is in writing them first.
- **Testing implementation details instead of behavior** — test what the method *does*, not how it does it internally.
- **One giant test** — one assertion per test. When something fails, you know exactly what broke.
