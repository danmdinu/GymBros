# Practice 9 — Cross-Cutting Concerns: One at a Time

## Why It Matters

Cross-cutting concerns are things that should apply to every command, every endpoint, or every entity — but aren't implemented yet. FluentValidation, structured error handling, logging, rate limiting, audit trails.

The temptation is to ask the AI to "add validation to all commands." Don't. Instead:

1. Do one perfectly
2. Review it
3. Use it as the template for the rest

This gives you control over the pattern before it propagates everywhere.

---

## What This Repo Needs

The backend rule file lists FluentValidation as part of the stack, but **no validators exist yet**. The `Verify2FACommand` is the perfect place to introduce them — it has a clear validation rule: the code must be exactly 6 digits.

---

## How to Do It

### Step 1 — Add FluentValidation to one command

Use Ask mode to get oriented:

```
@backend/src/WorkoutApp.Application
FluentValidation is listed in the backend rules but hasn't been implemented yet.
What's the standard way to wire it up with MediatR in a .NET 8 Clean Architecture project?
What do I need to: 1) install, 2) create in Application layer, 3) register in DI?
```

Then use Agent for the implementation:

```
@backend/src/WorkoutApp.Application/Features/TwoFactor/Commands/Verify2FA/
Add FluentValidation to Verify2FACommand:

1. Create Verify2FACommandValidator : AbstractValidator<Verify2FACommand>
   - Code must not be empty
   - Code must be exactly 6 characters
   - Code must be numeric only

2. Register the validator via AddValidatorsFromAssembly in Application/DependencyInjection.cs

3. Add a ValidationBehavior<TRequest, TResponse> pipeline behavior to the MediatR pipeline
   (only if one doesn't already exist in the project).

Do not add validators to any other commands yet.
```

### Step 2 — Review and approve the pattern

Check:
- Does the validator live next to the command it validates?
- Is it registered via assembly scanning (not manually)?
- Does the pipeline behavior short-circuit with a 400 response on failure?
- Does the error response match the format in the backend rules?

### Step 3 — Replicate to other commands using the template

Once the pattern is reviewed and approved, open `add-validation.md` and use it to add validators to the remaining commands one by one.

---

## GymBros 2FA Example

For `Verify2FACommand`, the validation is clear:

```csharp
public class Verify2FACommandValidator : AbstractValidator<Verify2FACommand>
{
    public Verify2FACommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .Length(6).WithMessage("Code must be exactly 6 digits.")
            .Matches(@"^\d{6}$").WithMessage("Code must contain only digits.");
    }
}
```

For `Enable2FACommand` — no input to validate (user identity comes from the JWT via `ICurrentUserService`).

For `Disable2FACommand` — same, no input.

This is a case where the AI might try to add validators where they're not needed. The prompt should be specific about which commands need validation.

---

## Other Cross-Cutting Concerns to Add Later (Same Pattern)

| Concern | Where | How |
|---|---|---|
| Structured logging | All handlers | Add logging via MediatR pipeline behavior |
| Performance metrics | All handlers | Add timing behavior, emit to OTEL |
| Authorization checks | Protected commands | Add `ICurrentUserService` guard at handler entry |
| Rate limiting | API endpoints | Add rate limit policy in `Program.cs` |

Each one: implement for one feature → review → propagate.

---

## Common Mistakes

- **"Add validation to all commands"** — the AI will produce inconsistent validators across the board without a reviewed template.
- **Putting validation logic in the handler** — it belongs in a validator class. The handler should trust that if it's executing, the input is already valid.
- **Forgetting to register the pipeline behavior in DI** — the validator exists but does nothing without the MediatR pipeline behavior wiring.
- **Validating in the controller** — the controller doesn't know business rules. Validation in the Application layer means it works regardless of how the command is triggered.
