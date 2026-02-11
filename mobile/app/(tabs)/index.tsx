import { StyleSheet, View, Text, ScrollView, TouchableOpacity, ActivityIndicator } from 'react-native';
import { colors, typography, spacing, borderRadius } from '@/constants/theme';
import { useProgress, useWorkout, useStartPlan, useCompleteWorkout } from '@/lib/queries';
import { SectionCard } from '@/components/SectionCard';

export default function WorkoutScreen() {
  const { data: progress, isLoading: progressLoading } = useProgress();
  const currentDay = progress?.currentDayNumber ?? 1;
  const { data: workout, isLoading: workoutLoading } = useWorkout(currentDay);
  const startPlan = useStartPlan();
  const completeWorkout = useCompleteWorkout();

  const isLoading = progressLoading || workoutLoading;
  const isCompleting = completeWorkout.isPending;

  // No progress yet - show start button
  if (!progressLoading && !progress) {
    return (
      <View style={styles.container}>
        <View style={styles.emptyState}>
          <Text style={styles.emptyIcon}>🏋️</Text>
          <Text style={styles.emptyTitle}>Ready to Start?</Text>
          <Text style={styles.emptySubtitle}>
            Begin your 30-day workout journey today!
          </Text>
          <TouchableOpacity 
            style={styles.startButton}
            onPress={() => startPlan.mutate()}
            disabled={startPlan.isPending}
          >
            {startPlan.isPending ? (
              <ActivityIndicator color={colors.text} />
            ) : (
              <Text style={styles.startButtonText}>Start Plan</Text>
            )}
          </TouchableOpacity>
        </View>
      </View>
    );
  }

  // Plan completed
  if (progress?.isCompleted) {
    return (
      <View style={styles.container}>
        <View style={styles.emptyState}>
          <Text style={styles.emptyIcon}>🏆</Text>
          <Text style={styles.emptyTitle}>Congratulations!</Text>
          <Text style={styles.emptySubtitle}>
            You've completed the 30-day challenge!
          </Text>
        </View>
      </View>
    );
  }

  if (isLoading) {
    return (
      <View style={styles.loadingContainer}>
        <ActivityIndicator size="large" color={colors.accent} />
        <Text style={styles.loadingText}>Loading workout...</Text>
      </View>
    );
  }

  if (!workout) {
    return (
      <View style={styles.container}>
        <View style={styles.emptyState}>
          <Text style={styles.emptyIcon}>❓</Text>
          <Text style={styles.emptyTitle}>Workout Not Found</Text>
          <Text style={styles.emptySubtitle}>
            Day {currentDay} doesn't have a workout yet.
          </Text>
        </View>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <ScrollView 
        style={styles.scrollView}
        contentContainerStyle={styles.scrollContent}
        showsVerticalScrollIndicator={false}
      >
        {/* Header */}
        <View style={styles.header}>
          <Text style={styles.dayBadge}>Day {workout.dayNumber} of 30</Text>
          <Text style={styles.title}>{workout.name}</Text>
          {workout.description && (
            <Text style={styles.description}>{workout.description}</Text>
          )}
        </View>

        {/* Sections */}
        {workout.sections.map((section) => (
          <SectionCard key={section.id} section={section} />
        ))}

        {/* Spacer for button */}
        <View style={{ height: 100 }} />
      </ScrollView>

      {/* Complete Button */}
      <View style={styles.buttonContainer}>
        <TouchableOpacity 
          style={[styles.completeButton, isCompleting && styles.completeButtonDisabled]}
          onPress={() => completeWorkout.mutate()}
          disabled={isCompleting}
        >
          {isCompleting ? (
            <ActivityIndicator color={colors.text} />
          ) : (
            <>
              <Text style={styles.completeButtonText}>Complete Workout</Text>
              <Text style={styles.completeButtonIcon}>✓</Text>
            </>
          )}
        </TouchableOpacity>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: colors.background,
  },
  loadingContainer: {
    flex: 1,
    backgroundColor: colors.background,
    alignItems: 'center',
    justifyContent: 'center',
  },
  loadingText: {
    ...typography.caption,
    marginTop: spacing.md,
  },
  scrollView: {
    flex: 1,
  },
  scrollContent: {
    padding: spacing.lg,
  },
  header: {
    marginBottom: spacing.xl,
  },
  dayBadge: {
    ...typography.small,
    color: colors.accent,
    fontWeight: '600',
    textTransform: 'uppercase',
    letterSpacing: 1,
    marginBottom: spacing.xs,
  },
  title: {
    ...typography.title,
    marginBottom: spacing.sm,
  },
  description: {
    ...typography.caption,
    lineHeight: 20,
  },
  buttonContainer: {
    position: 'absolute',
    bottom: 0,
    left: 0,
    right: 0,
    padding: spacing.lg,
    paddingBottom: spacing.xl,
    backgroundColor: colors.background,
    borderTopWidth: 1,
    borderTopColor: colors.border,
  },
  completeButton: {
    backgroundColor: colors.success,
    paddingVertical: spacing.md,
    paddingHorizontal: spacing.xl,
    borderRadius: borderRadius.md,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: spacing.sm,
  },
  completeButtonDisabled: {
    opacity: 0.7,
  },
  completeButtonText: {
    ...typography.body,
    fontWeight: '700',
    color: colors.text,
  },
  completeButtonIcon: {
    fontSize: 18,
    color: colors.text,
  },
  emptyState: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    padding: spacing.xl,
  },
  emptyIcon: {
    fontSize: 64,
    marginBottom: spacing.lg,
  },
  emptyTitle: {
    ...typography.heading,
    textAlign: 'center',
    marginBottom: spacing.sm,
  },
  emptySubtitle: {
    ...typography.caption,
    textAlign: 'center',
    marginBottom: spacing.xl,
  },
  startButton: {
    backgroundColor: colors.accent,
    paddingVertical: spacing.md,
    paddingHorizontal: spacing.xl * 2,
    borderRadius: borderRadius.md,
  },
  startButtonText: {
    ...typography.body,
    fontWeight: '700',
    color: colors.text,
  },
});
