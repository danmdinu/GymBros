// Dark theme for workout app
export const colors = {
  background: '#0D0D0D',
  surface: '#1A1A1A',
  surfaceLight: '#252525',
  accent: '#FF6B35',
  accentMuted: '#CC5529',
  text: '#FFFFFF',
  textMuted: '#888888',
  textDim: '#555555',
  success: '#4CAF50',
  error: '#F44336',
  border: '#333333',
};

// For backward compatibility with template
export default {
  light: {
    text: colors.text,
    background: colors.background,
    tint: colors.accent,
    tabIconDefault: colors.textMuted,
    tabIconSelected: colors.accent,
  },
  dark: {
    text: colors.text,
    background: colors.background,
    tint: colors.accent,
    tabIconDefault: colors.textMuted,
    tabIconSelected: colors.accent,
  },
};
