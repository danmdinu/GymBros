import { StyleSheet, View, Text } from 'react-native';
import { colors, typography, spacing, borderRadius } from '@/constants/theme';
import type { WorkoutExerciseDto } from '@/lib/types';

interface ExerciseRowProps {
  exercise: WorkoutExerciseDto;
  showSuperset?: boolean;
}

function formatMetric(metric: WorkoutExerciseDto['metric']): string {
  if (metric.type === 'reps') {
    return `${metric.reps} reps`;
  }
  return `${metric.seconds}s`;
}

export function ExerciseRow({ exercise, showSuperset = true }: ExerciseRowProps) {
  const hasSuperset = showSuperset && exercise.supersetGroup;

  return (
    <View style={styles.container}>
      {hasSuperset && (
        <View style={styles.supersetIndicator}>
          <Text style={styles.supersetText}>{exercise.supersetGroup}</Text>
        </View>
      )}
      <View style={styles.content}>
        <View style={styles.header}>
          <Text style={styles.name}>{exercise.exerciseName}</Text>
          <Text style={styles.metric}>
            {exercise.sets} × {formatMetric(exercise.metric)}
          </Text>
        </View>
        {exercise.notes && (
          <Text style={styles.notes}>{exercise.notes}</Text>
        )}
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    paddingVertical: spacing.sm,
    paddingHorizontal: spacing.md,
    backgroundColor: colors.surface,
    borderRadius: borderRadius.sm,
    marginBottom: spacing.xs,
  },
  supersetIndicator: {
    width: 24,
    height: 24,
    borderRadius: 12,
    backgroundColor: colors.accent,
    alignItems: 'center',
    justifyContent: 'center',
    marginRight: spacing.sm,
  },
  supersetText: {
    ...typography.small,
    color: colors.text,
    fontWeight: '700',
  },
  content: {
    flex: 1,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  name: {
    ...typography.body,
    flex: 1,
  },
  metric: {
    ...typography.caption,
    color: colors.accent,
    fontWeight: '600',
  },
  notes: {
    ...typography.small,
    marginTop: spacing.xs,
    fontStyle: 'italic',
  },
});
