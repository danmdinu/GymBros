# Prompt Template: New Frontend Screen (Expo Router)

> Copy this template, fill in the [PLACEHOLDERS], and paste into a Cursor Agent session (CMD+I).

---

## Usage

Use this template when adding a new screen to the GymBros mobile app.

Fill in:
- `[SCREEN_NAME]` — e.g. "TwoFactorSettings", "ExerciseDetail"
- `[FILE_PATH]` — the Expo Router file path, e.g. `app/(tabs)/settings/2fa.tsx`
- `[ENDPOINT]` — the backend endpoint this screen calls
- `[DATA_DESCRIPTION]` — what data the screen displays or mutates
- `[UI_DESCRIPTION]` — what the screen looks like

---

## Template

```
@mobile/app/(tabs)/[NEAREST_EXISTING_SCREEN].tsx
@mobile/lib/queries.ts
@mobile/constants/Colors.ts
@mobile/constants/theme.ts

Add a new screen called [SCREEN_NAME] at [FILE_PATH].

Description: [UI_DESCRIPTION]

Follow the patterns in the existing screen above. Steps:

1. In lib/queries.ts, add the data hook(s):
   - [HOOK_NAME]: [useQuery | useMutation] calling [ENDPOINT]
   - Follow the existing queryKeys pattern
   - Add to the queryKeys constants object

2. Create the screen file at [FILE_PATH]:
   - Default export: React component named [SCREEN_NAME]Screen
   - Uses the hook from lib/queries.ts
   - Loading state: show ActivityIndicator
   - Error state: show error message
   - [UI_DESCRIPTION]

3. Theme requirements:
   - Use colors from Colors.ts (dark-first)
   - Use spacing and typography from theme.ts
   - Use StyleSheet.create — no inline styles
   - No hardcoded hex colors

Do not:
- Fetch data with fetch() directly — use TanStack Query hooks from lib/queries.ts
- Use Zustand for server state — Zustand is for client state only (API URL, deviceId)
- Hardcode colors or spacing values
- Create new files outside of the listed paths
```

---

## Example (filled in)

```
@mobile/app/(tabs)/settings.tsx
@mobile/lib/queries.ts
@mobile/constants/Colors.ts

Add a new screen called TwoFactorSettings at app/(tabs)/settings/2fa.tsx.

Description: A settings screen where users can enable or disable TOTP-based 2FA.
Shows a QR code + code input when enabling, and a disable button when enabled.

1. In lib/queries.ts add:
   - useEnable2FA: useMutation → POST /api/user/2fa/enable
     returns { qrCodeUri: string, secret: string }
   - useVerify2FA: useMutation → POST /api/user/2fa/verify with { code: string }
   - useDisable2FA: useMutation → DELETE /api/user/2fa/disable

2. Create app/(tabs)/settings/2fa.tsx:
   - If 2FA disabled: show "Enable 2FA" button → triggers useEnable2FA
   - After enable: show QR code (react-native-qrcode-svg) + 6-digit TextInput + "Verify" button
   - If 2FA enabled: show "Disable 2FA" button
   - All mutations show loading state on their respective button

Do not touch settings.tsx or any other files.
```
