# Prompt Template: Add FluentValidation to a Command

> Copy this template, fill in the [PLACEHOLDERS], and paste into a Cursor Agent session (CMD+I).

---

## Usage

Use this template when adding a FluentValidation validator to an existing command.

Fill in:
- `[COMMAND_NAME]` — e.g. "Verify2FACommand"
- `[COMMAND_PATH]` — folder path to the command
- `[VALIDATION_RULES]` — the specific rules to enforce

---

## Template

```
@backend/src/WorkoutApp.Application/Features/[COMMAND_PATH]/[COMMAND_NAME].cs
@backend/src/WorkoutApp.Application/DependencyInjection.cs
@.cursor/rules/backend.md

Add FluentValidation to [COMMAND_NAME].

1. Create [COMMAND_NAME]Validator.cs in the same folder as [COMMAND_NAME].cs:
   - Class: [COMMAND_NAME]Validator : AbstractValidator<[COMMAND_NAME]>
   - Rules: [VALIDATION_RULES]

2. In Application/DependencyInjection.cs:
   - If not already present, add: services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly)
   - If not already present, add a ValidationBehavior<TRequest, TResponse> to the MediatR pipeline

3. The ValidationBehavior should:
   - Run before the handler
   - Collect all validation failures
   - Throw a ValidationException if any exist
   - The API layer should translate ValidationException to a 400 response

Place the validator file next to the command file. Do not touch any other files.
```

---

## Example: Verify2FACommand Validator

```
@backend/src/WorkoutApp.Application/Features/TwoFactor/Commands/Verify2FA/Verify2FACommand.cs
@backend/src/WorkoutApp.Application/DependencyInjection.cs

Add FluentValidation to Verify2FACommand.

1. Create Verify2FACommandValidator.cs next to the command:
   Rules:
   - Code must not be empty (WithMessage: "Verification code is required.")
   - Code must be exactly 6 characters (WithMessage: "Code must be exactly 6 digits.")
   - Code must match regex ^\d{6}$ (WithMessage: "Code must contain only digits.")

2. In DependencyInjection.cs:
   - Add services.AddValidatorsFromAssembly(...)
   - Add ValidationBehavior to MediatR pipeline if not present

Do not add validators to Enable2FA or Disable2FA — they have no user input to validate.
Do not touch any other files.
```

---

## Notes

- Validators live **next to the command they validate** (same folder)
- Registration uses **assembly scanning** — never register validators manually
- The **ValidationBehavior** is a MediatR pipeline behavior — add it once, it runs for all commands
- After adding the first validator + behavior, subsequent validators only need Step 1 (the class itself)
