import { useState } from 'react';
import { StyleSheet, View, Text, TextInput, TouchableOpacity, Alert, ScrollView } from 'react-native';
import { colors, typography, spacing, borderRadius } from '@/constants/theme';
import { useAppStore } from '@/lib/store';
import { useQueryClient } from '@tanstack/react-query';

export default function SettingsScreen() {
  const { apiUrl, setApiUrl, deviceId } = useAppStore();
  const [tempUrl, setTempUrl] = useState(apiUrl);
  const queryClient = useQueryClient();

  const handleSaveUrl = () => {
    if (!tempUrl.trim()) {
      Alert.alert('Error', 'API URL cannot be empty');
      return;
    }
    setApiUrl(tempUrl.trim());
    queryClient.invalidateQueries();
    Alert.alert('Saved', 'API URL updated. Data will refresh.');
  };

  const handleResetUrl = () => {
    const defaultUrl = 'http://localhost:5032/api';
    setTempUrl(defaultUrl);
    setApiUrl(defaultUrl);
    queryClient.invalidateQueries();
    Alert.alert('Reset', 'API URL reset to default.');
  };

  const handleClearCache = () => {
    Alert.alert(
      'Clear Cache',
      'This will clear all cached data and refresh from the server.',
      [
        { text: 'Cancel', style: 'cancel' },
        { 
          text: 'Clear', 
          style: 'destructive',
          onPress: () => {
            queryClient.clear();
            Alert.alert('Done', 'Cache cleared.');
          }
        },
      ]
    );
  };

  return (
    <ScrollView style={styles.container} contentContainerStyle={styles.content}>
      {/* API Configuration */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>API Configuration</Text>
        <Text style={styles.sectionSubtitle}>
          Configure the backend server URL for development
        </Text>

        <View style={styles.inputGroup}>
          <Text style={styles.inputLabel}>API URL</Text>
          <TextInput
            style={styles.input}
            value={tempUrl}
            onChangeText={setTempUrl}
            placeholder="http://localhost:5032/api"
            placeholderTextColor={colors.textDim}
            autoCapitalize="none"
            autoCorrect={false}
            keyboardType="url"
          />
        </View>

        <View style={styles.buttonRow}>
          <TouchableOpacity style={styles.secondaryButton} onPress={handleResetUrl}>
            <Text style={styles.secondaryButtonText}>Reset</Text>
          </TouchableOpacity>
          <TouchableOpacity style={styles.primaryButton} onPress={handleSaveUrl}>
            <Text style={styles.primaryButtonText}>Save</Text>
          </TouchableOpacity>
        </View>
      </View>

      {/* Device Info */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Device Info</Text>
        
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Device ID</Text>
          <Text style={styles.infoValue} numberOfLines={1}>
            {deviceId || 'Loading...'}
          </Text>
        </View>
      </View>

      {/* Cache */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Data</Text>
        
        <TouchableOpacity style={styles.dangerButton} onPress={handleClearCache}>
          <Text style={styles.dangerButtonText}>Clear Cache</Text>
        </TouchableOpacity>
      </View>

      {/* About */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>About</Text>
        <Text style={styles.aboutText}>
          30-Day Workout Challenge{'\n'}
          A simple app to keep you moving every day.
        </Text>
        <Text style={styles.versionText}>Version 1.0.0</Text>
      </View>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: colors.background,
  },
  content: {
    padding: spacing.lg,
  },
  section: {
    marginBottom: spacing.xl,
  },
  sectionTitle: {
    ...typography.sectionTitle,
    marginBottom: spacing.xs,
  },
  sectionSubtitle: {
    ...typography.small,
    marginBottom: spacing.md,
  },
  inputGroup: {
    marginBottom: spacing.md,
  },
  inputLabel: {
    ...typography.caption,
    marginBottom: spacing.xs,
  },
  input: {
    backgroundColor: colors.surface,
    borderRadius: borderRadius.sm,
    padding: spacing.md,
    color: colors.text,
    fontSize: 14,
    borderWidth: 1,
    borderColor: colors.border,
  },
  buttonRow: {
    flexDirection: 'row',
    gap: spacing.sm,
  },
  primaryButton: {
    flex: 1,
    backgroundColor: colors.accent,
    paddingVertical: spacing.sm,
    borderRadius: borderRadius.sm,
    alignItems: 'center',
  },
  primaryButtonText: {
    ...typography.body,
    fontWeight: '600',
    color: colors.text,
  },
  secondaryButton: {
    flex: 1,
    backgroundColor: colors.surface,
    paddingVertical: spacing.sm,
    borderRadius: borderRadius.sm,
    alignItems: 'center',
    borderWidth: 1,
    borderColor: colors.border,
  },
  secondaryButtonText: {
    ...typography.body,
    fontWeight: '600',
    color: colors.textMuted,
  },
  dangerButton: {
    backgroundColor: colors.surface,
    paddingVertical: spacing.sm,
    borderRadius: borderRadius.sm,
    alignItems: 'center',
    borderWidth: 1,
    borderColor: colors.error,
  },
  dangerButtonText: {
    ...typography.body,
    fontWeight: '600',
    color: colors.error,
  },
  infoRow: {
    backgroundColor: colors.surface,
    borderRadius: borderRadius.sm,
    padding: spacing.md,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  infoLabel: {
    ...typography.caption,
  },
  infoValue: {
    ...typography.small,
    color: colors.text,
    flex: 1,
    textAlign: 'right',
    marginLeft: spacing.md,
  },
  aboutText: {
    ...typography.caption,
    lineHeight: 22,
    marginBottom: spacing.sm,
  },
  versionText: {
    ...typography.small,
  },
});
