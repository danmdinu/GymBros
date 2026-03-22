# Prompt Template: Write Unit Tests

> Copy this template, fill in the [PLACEHOLDERS], and paste into a Cursor Ask session (CMD+L) first to generate test stubs, then Agent (CMD+I) to create the file.

---

## Usage

Use this template when adding unit tests for a domain entity method, command handler, or service.

Fill in:
- `[CLASS_NAME]` — the class being tested
- `[METHOD_NAME]` — the method being tested
- `[BEHAVIORS]` — list of behaviors to test (one test per behavior)
- `[TEST_FILE_PATH]` — where to create the test file

---

## Template

```
@backend/src/[PATH_TO_CLASS_BEING_TESTED]
@.cursor/rules/backend.md

Write unit tests for [CLASS_NAME].[METHOD_NAME]().

Test framework: xUnit + FluentAssertions + Moq
Pattern: Arrange / Act / Assert
Rule: One assertion per test method

Test these behaviors:
[BEHAVIORS]

Requirements:
- Test method names: [MethodName]_[Scenario]_[ExpectedResult]
  e.g. Enable2FA_WhenAlreadyEnabled_ShouldThrowInvalidOperationException
- Use FluentAssertions (x.Should().Be(), x.Should().Throw<>(), etc.)
- Use Moq for any dependencies that need to be mocked
- Do not test implementation details — test observable behavior only
- Do not modify the class being tested

Create the file at: [TEST_FILE_PATH]
```

---

## Example: Testing User.Enable2FA()

```
@backend/src/WorkoutApp.Domain/Entities/User.cs

Write unit tests for User.Enable2FA().

Test framework: xUnit + FluentAssertions + Moq
Pattern: Arrange / Act / Assert
Rule: One assertion per test method

Test these behaviors:
1. When called on a new user, it should return a non-empty TOTP secret string
2. When called, IsTwoFactorEnabled should remain false (pending verification)
3. When called on a user where 2FA is already enabled, it should throw InvalidOperationException
4. When called twice on the same user (not yet verified), it should reset the secret

Create the test file at: backend/src/WorkoutApp.Tests/Domain/UserTests.cs

Do not implement the methods on User.cs — only write the tests.
The tests should currently be RED (failing). That is correct.
```

---

## Example: Testing a Command Handler

```
@backend/src/WorkoutApp.Application/Features/TwoFactor/Commands/Verify2FA/Verify2FACommandHandler.cs
@backend/src/WorkoutApp.Domain/Entities/User.cs

Write unit tests for Verify2FACommandHandler.Handle().

Test framework: xUnit + FluentAssertions + Moq
Pattern: Arrange / Act / Assert

Test these behaviors:
1. When the code is valid (ITotpService.Verify returns true), should call user.ConfirmTwoFactor()
2. When the code is invalid (ITotpService.Verify returns false), should return false
3. When the user is not found, should throw NotFoundException
4. After successful verification, should call SaveChangesAsync

Mock: IAppDbContext, ITotpService, ICurrentUserService

Create at: backend/src/WorkoutApp.Tests/Application/Commands/Verify2FACommandHandlerTests.cs
```
