# Prompt Template: Enable 2FA — Live Demo Template

> This is the template used during the live session demo (Practice 6).
> Each section corresponds to one Agent session (one step at a time).

---

## Step 1 — Domain Entity

```
@backend/src/WorkoutApp.Domain/Entities/User.cs
@.cursor/rules/backend.md

Add TOTP 2FA support to the User entity. Follow the existing rich domain model pattern
(private setters, behavior methods, no external dependencies).

Add fields:
- TotpSecret: string?, nullable, private setter
- IsTwoFactorEnabled: bool, private setter, default false

Add methods:
- Enable2FA() → string
  Generates a 20-byte random base32 TOTP secret.
  Stores it in TotpSecret.
  Sets IsTwoFactorEnabled to false (pending verification).
  Returns the secret as a base32 string.
  Throws InvalidOperationException if IsTwoFactorEnabled is already true.

- ConfirmTwoFactor()
  Sets IsTwoFactorEnabled = true.
  Throws InvalidOperationException if TotpSecret is null (Enable2FA not called yet).

- Disable2FA()
  Sets TotpSecret = null.
  Sets IsTwoFactorEnabled = false.

Do not touch any other files.
```

---

## Step 2 — EF Core Configuration

```
@backend/src/WorkoutApp.Infrastructure/Persistence/Configurations/UserConfiguration.cs

Update UserConfiguration to map the new User entity fields.
Follow the existing Fluent API style in this file. No data annotations.

- TotpSecret: nullable string, max length 64
- IsTwoFactorEnabled: non-nullable bool, HasDefaultValue(false)

Do not touch any other files.
```

---

## Step 3 — Infrastructure Service

```
@backend/src/WorkoutApp.Application/Common/Interfaces/
@backend/src/WorkoutApp.Infrastructure/
@backend/src/WorkoutApp.Infrastructure/DependencyInjection.cs

Add a TOTP service.

1. In Application/Common/Interfaces/, create ITotpService.cs:
   - bool Verify(string base32Secret, string code)
     Verifies a 6-digit TOTP code against a base32 secret.
   - string GenerateQrCodeUri(string accountName, string base32Secret)
     Returns an otpauth:// URI for QR code generation.
     Issuer name: "GymBros"

2. In Infrastructure/Services/, create TotpService.cs implementing ITotpService.
   Use the OTP.NET NuGet package (Otp.NET by kspearrin).
   - Verify: use Totp.ValidateTime() with a 1-step window
   - GenerateQrCodeUri: build the otpauth:// URI manually or use OtpUri class

3. Register in Infrastructure/DependencyInjection.cs:
   services.AddTransient<ITotpService, TotpService>();

Do not touch Application/DependencyInjection.cs.
Do not touch any other files.
```

---

## Step 4 — Commands and Handlers

```
@backend/src/WorkoutApp.Application/Features/Progress/Commands/CompleteWorkout/
@backend/src/WorkoutApp.Application/Common/Interfaces/IAppDbContext.cs
@backend/src/WorkoutApp.Application/Common/Interfaces/ICurrentUserService.cs

Create three CQRS commands following the exact pattern in CompleteWorkout.

1. Features/TwoFactor/Commands/Enable2FA/
   - Enable2FACommand: record IRequest<Enable2FAResponse>
   - Enable2FAResponse: record { string QrCodeUri, string Secret }
   - Enable2FACommandHandler:
     * Load user via ICurrentUserService.UserId + IAppDbContext
     * Call user.Enable2FA() → get secret
     * Call ITotpService.GenerateQrCodeUri(user.Email, secret)
     * SaveChangesAsync
     * Return Enable2FAResponse

2. Features/TwoFactor/Commands/Verify2FA/
   - Verify2FACommand: record IRequest<bool> { string Code }
   - Verify2FACommandHandler:
     * Load user, call ITotpService.Verify(user.TotpSecret!, command.Code)
     * If valid: call user.ConfirmTwoFactor(), SaveChangesAsync, return true
     * If invalid: return false

3. Features/TwoFactor/Commands/Disable2FA/
   - Disable2FACommand: record IRequest<Unit>
   - Disable2FACommandHandler:
     * Load user, call user.Disable2FA(), SaveChangesAsync

No business logic in handlers. No EF Core directly — use IAppDbContext.
Do not touch any other files.
```

---

## Step 5 — Controller Endpoints

```
@backend/src/WorkoutApp.Api/Controllers/UserController.cs

Add three 2FA endpoints to UserController. All require [Authorize].
The controller is thin — only calls _mediator.Send(). No logic.

POST   /api/user/2fa/enable
  → sends Enable2FACommand
  → returns 200 with Enable2FAResponse { qrCodeUri, secret }

POST   /api/user/2fa/verify
  → body: { code: string }
  → sends Verify2FACommand
  → if true: returns 200
  → if false: returns 400 with message "Invalid verification code."

DELETE /api/user/2fa/disable
  → sends Disable2FACommand
  → returns 204 No Content

Do not touch any other files.
```

---

## Step 6 — Mobile Screen

```
@mobile/app/(tabs)/settings.tsx
@mobile/lib/queries.ts
@mobile/constants/Colors.ts
@mobile/constants/theme.ts

Add 2FA management to the mobile app.

1. In lib/queries.ts, add three mutations:
   - useEnable2FA(): useMutation → POST /api/user/2fa/enable
     returns { qrCodeUri: string, secret: string }
   - useVerify2FA(): useMutation → POST /api/user/2fa/verify, body: { code: string }
     returns boolean
   - useDisable2FA(): useMutation → DELETE /api/user/2fa/disable

2. Create app/(tabs)/settings/2fa.tsx:
   State machine:
   - idle: show "Enable 2FA" button
   - setup: show QR code (react-native-qrcode-svg) + 6-digit input + "Verify" button
   - enabled: show "2FA is active" + "Disable 2FA" button

   Use theme colors from Colors.ts. StyleSheet.create only. No inline styles.
   Show loading state on all buttons during mutation.

Do not modify settings.tsx or any other existing file.
```
