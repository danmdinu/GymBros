# Frontend Rules — React Native / Expo

## Overview

This is the mobile frontend for a 30-day fitness challenge app. Built with React Native and Expo, using file-based routing.

---

## Tech Stack

- **Framework**: React Native 0.81 + Expo ~54
- **Navigation**: Expo Router v6 (file-based routing)
- **Server state**: TanStack React Query v5
- **Client state**: Zustand v5
- **Persistence**: AsyncStorage (API URL, device ID)
- **Animations**: React Native Reanimated v4
- **Language**: TypeScript 5.9

---

## Folder Structure

```
mobile/
├── app/
│   ├── (tabs)/          ← Tab navigator screens (index, progress, settings)
│   ├── _layout.tsx      ← Root layout (TanStack Query provider, store init)
│   └── modal.tsx        ← Modal screen example
├── components/          ← Reusable UI components
├── constants/
│   ├── Colors.ts        ← Color palette (dark-first)
│   └── theme.ts         ← Spacing, typography scale, border radii
└── lib/
    ├── api.ts           ← Raw fetch functions (no React, no hooks)
    ├── queries.ts       ← All TanStack Query hooks (useQuery, useMutation)
    ├── store.ts         ← Zustand store (client state only)
    └── types.ts         ← Shared TypeScript types
```

---

## Key Conventions

### Screens

- Every screen is a default export React component in `app/`
- Tab screens live in `app/(tabs)/`
- Screen names end in `Screen`: `export default function WorkoutScreen()`
- Loading state: always show `<ActivityIndicator />` while data is fetching
- Error state: always show a user-facing error message

### Styling

- **Always use `StyleSheet.create`** — no inline style objects
- **Never hardcode colors** — use `colors` from `@/constants/theme`
- **Never hardcode spacing** — use `spacing` from `@/constants/theme`
- **Dark-first** — the app uses a dark theme. Default background: `colors.background`
- Import theme like this: `import { colors, typography, spacing, borderRadius } from '@/constants/theme'`

### Data Fetching (TanStack Query)

- All API calls go through `lib/api.ts` — raw async functions, no React dependencies
- All hooks go in `lib/queries.ts` — never fetch directly inside a component
- Always add a `queryKey` to the `queryKeys` object at the top of `queries.ts`
- Always set a `staleTime` on `useQuery` hooks (workouts: 5 min, progress: 1 min)
- For mutations, use `onSuccess` to update or invalidate related query cache
- Never use `fetch()` directly inside a component

```typescript
// Good — query
export function useWorkout(dayNumber: number) {
  return useQuery({
    queryKey: queryKeys.workout(dayNumber),
    queryFn: () => api.getWorkout(dayNumber),
    staleTime: 1000 * 60 * 5,
  });
}

// Good — mutation
export function useCompleteWorkout() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: () => api.completeWorkout(),
    onSuccess: (data) => {
      queryClient.setQueryData(queryKeys.progress, data);
      queryClient.invalidateQueries({ queryKey: ['workout'] });
    },
  });
}
```

### State Management

- **TanStack Query** = server state (anything that comes from the API)
- **Zustand** (`lib/store.ts`) = client state only (API URL, device ID — things that never hit the server)
- Never put server data in Zustand
- Never put client-only state in React Query

### Components

- Reusable components live in `components/`
- Components receive data as props — they do not fetch data themselves
- Use `StyleSheet.create` at the bottom of each component file

---

## Things to Avoid

❌ **Don't** fetch data with `fetch()` inside a component — use a hook from `lib/queries.ts`  
❌ **Don't** put API URL or server data in Zustand — Zustand is for client state  
❌ **Don't** use inline styles — always `StyleSheet.create`  
❌ **Don't** hardcode hex color values — use `colors` from theme  
❌ **Don't** use `console.log` for debugging — remove before committing  
❌ **Don't** create new query hooks outside of `lib/queries.ts`  
❌ **Don't** use `any` in TypeScript — define proper types in `lib/types.ts`  
❌ **Don't** use `useEffect` for data fetching — that's what TanStack Query is for  

---

## Location

Mobile code lives in the `/mobile` folder.
