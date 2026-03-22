# Practice 6 — Building a Feature End-to-End (The Main Demo)

## Why It Matters

This is the core of the session. We build TOTP-based 2FA from scratch using every practice covered so far: rules for conventions, Ask to plan, @-mentions for surgical context, the prompt template, and TDD for the domain logic.

The point is not just to build 2FA — it's to show how AI-assisted development feels when you do it *right*.

---

## What We're Building

TOTP (Time-based One-Time Password) 2FA, layered on top of the existing Firebase Auth:

1. User goes to settings, taps "Enable 2FA"
2. Backend generates a TOTP secret, returns it + a QR code URI
3. User scans the QR code with an authenticator app (Google Authenticator, Authy, etc.)
4. User enters the 6-digit code to confirm setup
5. 2FA is now active on their account
6. On future logins, after Firebase Auth, the user must also verify a TOTP code

---

## The Build: Step by Step

Each step below is a **separate, scoped Agent session**. Review and accept each step before moving to the next.

---

### Step 1 — Domain Entity

**What:** Add `TotpSecret` and `IsTwoFactorEnabled` to `User`, plus behavior methods.

**Context to include:**
- `@backend/src/WorkoutApp.Domain/Entities/User.cs`

**Prompt:**
```
Add TOTP 2FA support to the User entity. Following the existing rich domain model pattern:

1. Add fields: TotpSecret (string?, nullable), IsTwoFactorEnabled (bool, default false)
2. Add method: Enable2FA() → generates a 32-character base32 TOTP secret, stores it,
   sets IsTwoFactorEnabled to false (pending verification), returns the secret.
   Throw InvalidOperationException if 2FA is already enabled.
3. Add method: ConfirmTwoFactor() → sets IsTwoFactorEnabled to true.
   Throw InvalidOperationException if Enable2FA hasn't been called (no secret).
4. Add method: Disable2FA() → clears TotpSecret, sets IsTwoFactorEnabled to false.

Use private setters. No business logic outside the entity.
Do not touch any other files.
```

**Review:** Check that private setters are used, that the methods throw on invalid state, and that no EF or service dependencies crept in.

---

### Step 2 — EF Core Configuration

**What:** Update `UserConfiguration.cs` to map the new fields.

**Context to include:**
- `@backend/src/WorkoutApp.Infrastructure/Persistence/Configurations/UserConfiguration.cs`

**Prompt:**
```
Update UserConfiguration to map the new User entity fields:
- TotpSecret: string column, nullable, max length 64
- IsTwoFactorEnabled: bool column, not nullable, default false

Follow the existing Fluent API configuration style in this file. No data annotations.
Do not touch any other files.
```

---

### Step 3 — Infrastructure: TotpService

**What:** Add `ITotpService` interface in Application, implement `TotpService` in Infrastructure.

**Context to include:**
- `@backend/src/WorkoutApp.Application/Common/Interfaces/`
- `@backend/src/WorkoutApp.Infrastructure/`

**Prompt:**
```
Add a TOTP verification service:

1. In WorkoutApp.Application/Common/Interfaces/, create ITotpService:
   - bool Verify(string base32Secret, string code) — verifies a 6-digit TOTP code
   - string GenerateQrCodeUri(string email, string base32Secret) — returns an otpauth:// URI

2. In WorkoutApp.Infrastructure/Services/, create TotpService implementing ITotpService.
   Use the OTP.NET NuGet package (Otp.NET). The issuer name should be "GymBros".

3. Register TotpService as ITransient in Infrastructure's DependencyInjection.cs.

Do not touch Application layer DI.
```

---

### Step 4 — Commands and Handlers

**What:** Three new commands: `Enable2FA`, `Verify2FA`, `Disable2FA`.

**Context to include:**
- `@backend/src/WorkoutApp.Application/Features/Progress/Commands/CompleteWorkout/`
- `@backend/src/WorkoutApp.Application/Common/Interfaces/`

**Prompt:**
```
Create three CQRS commands following the exact pattern in CompleteWorkout:

1. Enable2FACommand (no input beyond user ID from ICurrentUserService)
   Response: Enable2FAResponse { string QrCodeUri, string Secret }
   Handler: calls user.Enable2FA(), then ITotpService.GenerateQrCodeUri(), saves, returns response

2. Verify2FACommand { string Code }
   Response: bool (success)
   Handler: loads user, calls ITotpService.Verify(), if valid calls user.ConfirmTwoFactor(), saves

3. Disable2FACommand (no input)
   Response: none (void / Unit)
   Handler: calls user.Disable2FA(), saves

Place each in Features/TwoFactor/Commands/[CommandName]/
Do not put any logic in controllers or handlers beyond orchestration.
```

---

### Step 5 — Controller Endpoints

**What:** Add 3 endpoints to `UserController`.

**Context to include:**
- `@backend/src/WorkoutApp.Api/Controllers/UserController.cs`

**Prompt:**
```
Add three 2FA endpoints to UserController. All require [Authorize].
Follow the existing controller pattern (thin, only calls _mediator.Send):

POST   /api/user/2fa/enable   → sends Enable2FACommand, returns 200 with Enable2FAResponse
POST   /api/user/2fa/verify   → body: { code: string }, sends Verify2FACommand, returns 200 or 400
DELETE /api/user/2fa/disable  → sends Disable2FACommand, returns 204

No business logic in the controller.
```

---

### Step 6 — Mobile Settings Screen

**What:** Add a 2FA settings screen to the mobile app.

**Context to include:**
- `@mobile/app/(tabs)/settings.tsx`
- `@mobile/lib/queries.ts`

**Prompt:**
```
Add 2FA setup to the mobile settings screen:

1. In lib/queries.ts, add three mutations:
   - useEnable2FA() → POST /api/user/2fa/enable
   - useVerify2FA() → POST /api/user/2fa/verify with { code }
   - useDisable2FA() → DELETE /api/user/2fa/disable

2. In app/(tabs)/settings.tsx, add a "Two-Factor Authentication" section below the existing settings:
   - If 2FA is disabled: show an "Enable 2FA" button
   - After tapping: show a QR code (use react-native-qrcode-svg) + a 6-digit code input + "Verify" button
   - If 2FA is enabled: show a "Disable 2FA" button

Follow the existing component patterns and theme constants (Colors, spacing).
```

---

## The Finished Architecture

```
User.cs                         ← TotpSecret, IsTwoFactorEnabled, Enable2FA(), ConfirmTwoFactor(), Disable2FA()
UserConfiguration.cs            ← maps new fields
ITotpService.cs                 ← Verify(), GenerateQrCodeUri()
TotpService.cs                  ← OTP.NET implementation
Enable2FACommand + Handler      ← orchestrates entity + service
Verify2FACommand + Handler
Disable2FACommand + Handler
UserController.cs               ← 3 thin HTTP endpoints
lib/queries.ts                  ← 3 React Query mutations
app/(tabs)/settings.tsx         ← QR code + verify UI
```

---

## Key Observations During the Demo

Watch for these during the live session — they're teaching moments:

1. **The Agent stays in-lane** — because of the rules and scoped context, it doesn't put logic in controllers
2. **Step 3 requires DI registration** — watch whether the Agent includes it. This is a common omission (covered in Practice 10)
3. **Step 4 handlers are thin** — if a handler has more than ~15 lines, something is wrong
4. **The TDD tests from Practice 5 now pass** — run `dotnet test` after Step 1 to confirm
