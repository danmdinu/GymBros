import { StyleSheet, View, Text } from 'react-native';
import { colors, typography, spacing, borderRadius } from '@/constants/theme';
import type { WorkoutSectionDto } from '@/lib/types';
import { ExerciseRow } from './ExerciseRow';

interface SectionCardProps {
  section: WorkoutSectionDto;
}

function getSectionIcon(type: WorkoutSectionDto['type']): string {
  switch (type) {
    case 'WarmUp': return '🔥';
    case 'Main': return '💪';
    case 'CoolDown': return '❄️';
    default: return '📋';
  }
}

function getSectionColor(type: WorkoutSectionDto['type']): string {
  switch (type) {
    case 'WarmUp': return '#FF9800';
    case 'Main': return colors.accent;
    case 'CoolDown': return '#2196F3';
    default: return colors.textMuted;
  }
}

export function SectionCard({ section }: SectionCardProps) {
  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Text style={styles.icon}>{getSectionIcon(section.type)}</Text>
        <Text style={[styles.title, { color: getSectionColor(section.type) }]}>
          {section.name}
        </Text>
        <Text style={styles.count}>
          {section.exercises.length} exercise{section.exercises.length !== 1 ? 's' : ''}
        </Text>
      </View>
      <View style={styles.exercises}>
        {section.exercises.map((exercise) => (
          <ExerciseRow key={exercise.id} exercise={exercise} />
        ))}
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    marginBottom: spacing.lg,
  },
  header: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: spacing.sm,
    paddingHorizontal: spacing.xs,
  },
  icon: {
    fontSize: 18,
    marginRight: spacing.sm,
  },
  title: {
    ...typography.sectionTitle,
    flex: 1,
  },
  count: {
    ...typography.small,
  },
  exercises: {
    gap: spacing.xs,
  },
});
