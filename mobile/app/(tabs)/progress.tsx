import { StyleSheet, View, Text, TouchableOpacity, ActivityIndicator } from 'react-native';
import { colors, typography, spacing, borderRadius } from '@/constants/theme';
import { useProgress, useStartPlan } from '@/lib/queries';

export default function ProgressScreen() {
  const { data: progress, isLoading } = useProgress();
  const startPlan = useStartPlan();

  if (isLoading) {
    return (
      <View style={styles.loadingContainer}>
        <ActivityIndicator size="large" color={colors.accent} />
      </View>
    );
  }

  // No progress yet
  if (!progress) {
    return (
      <View style={styles.container}>
        <View style={styles.emptyState}>
          <Text style={styles.emptyIcon}>📊</Text>
          <Text style={styles.emptyTitle}>No Progress Yet</Text>
          <Text style={styles.emptySubtitle}>
            Start your workout plan to track your progress!
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

  const completedDays = progress.currentDayNumber - 1;
  const totalDays = 30;
  const progressPercent = Math.round((completedDays / totalDays) * 100);
  const isCompleted = progress.isCompleted;

  return (
    <View style={styles.container}>
      {/* Progress Card */}
      <View style={styles.progressCard}>
        <Text style={styles.progressTitle}>Your Journey</Text>
        
        {/* Progress Circle */}
        <View style={styles.progressCircle}>
          <Text style={styles.progressPercent}>{progressPercent}%</Text>
          <Text style={styles.progressLabel}>Complete</Text>
        </View>

        {/* Progress Bar */}
        <View style={styles.progressBarContainer}>
          <View style={styles.progressBarBg}>
            <View style={[styles.progressBarFill, { width: `${progressPercent}%` }]} />
          </View>
          <Text style={styles.progressBarText}>
            {completedDays} of {totalDays} days
          </Text>
        </View>
      </View>

      {/* Stats Grid */}
      <View style={styles.statsGrid}>
        <View style={styles.statCard}>
          <Text style={styles.statValue}>{progress.currentDayNumber}</Text>
          <Text style={styles.statLabel}>Current Day</Text>
        </View>
        <View style={styles.statCard}>
          <Text style={styles.statValue}>{completedDays}</Text>
          <Text style={styles.statLabel}>Workouts Done</Text>
        </View>
        <View style={styles.statCard}>
          <Text style={styles.statValue}>{totalDays - completedDays}</Text>
          <Text style={styles.statLabel}>Remaining</Text>
        </View>
      </View>

      {/* Status */}
      <View style={[styles.statusCard, isCompleted && styles.statusCardComplete]}>
        <Text style={styles.statusIcon}>{isCompleted ? '🏆' : '🔥'}</Text>
        <View style={styles.statusContent}>
          <Text style={styles.statusTitle}>
            {isCompleted ? 'Challenge Complete!' : 'Keep Going!'}
          </Text>
          <Text style={styles.statusSubtitle}>
            {isCompleted 
              ? "You've completed the 30-day challenge!"
              : `${totalDays - completedDays} more days to go. You've got this!`
            }
          </Text>
        </View>
      </View>

      {/* Last workout info */}
      {progress.lastWorkoutCompletedAt && (
        <View style={styles.lastWorkout}>
          <Text style={styles.lastWorkoutLabel}>Last completed:</Text>
          <Text style={styles.lastWorkoutDate}>
            {new Date(progress.lastWorkoutCompletedAt).toLocaleDateString('en-US', {
              weekday: 'long',
              month: 'short',
              day: 'numeric',
              hour: 'numeric',
              minute: '2-digit',
            })}
          </Text>
        </View>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: colors.background,
    padding: spacing.lg,
  },
  loadingContainer: {
    flex: 1,
    backgroundColor: colors.background,
    alignItems: 'center',
    justifyContent: 'center',
  },
  progressCard: {
    backgroundColor: colors.surface,
    borderRadius: borderRadius.lg,
    padding: spacing.xl,
    alignItems: 'center',
    marginBottom: spacing.lg,
  },
  progressTitle: {
    ...typography.sectionTitle,
    marginBottom: spacing.lg,
  },
  progressCircle: {
    width: 140,
    height: 140,
    borderRadius: 70,
    borderWidth: 8,
    borderColor: colors.accent,
    alignItems: 'center',
    justifyContent: 'center',
    marginBottom: spacing.lg,
  },
  progressPercent: {
    fontSize: 36,
    fontWeight: '700',
    color: colors.text,
  },
  progressLabel: {
    ...typography.small,
  },
  progressBarContainer: {
    width: '100%',
  },
  progressBarBg: {
    height: 8,
    backgroundColor: colors.surfaceLight,
    borderRadius: 4,
    overflow: 'hidden',
    marginBottom: spacing.xs,
  },
  progressBarFill: {
    height: '100%',
    backgroundColor: colors.accent,
    borderRadius: 4,
  },
  progressBarText: {
    ...typography.small,
    textAlign: 'center',
  },
  statsGrid: {
    flexDirection: 'row',
    gap: spacing.sm,
    marginBottom: spacing.lg,
  },
  statCard: {
    flex: 1,
    backgroundColor: colors.surface,
    borderRadius: borderRadius.md,
    padding: spacing.md,
    alignItems: 'center',
  },
  statValue: {
    fontSize: 28,
    fontWeight: '700',
    color: colors.accent,
  },
  statLabel: {
    ...typography.small,
    marginTop: spacing.xs,
  },
  statusCard: {
    flexDirection: 'row',
    backgroundColor: colors.surface,
    borderRadius: borderRadius.md,
    padding: spacing.md,
    alignItems: 'center',
    borderLeftWidth: 4,
    borderLeftColor: colors.accent,
    marginBottom: spacing.lg,
  },
  statusCardComplete: {
    borderLeftColor: colors.success,
  },
  statusIcon: {
    fontSize: 32,
    marginRight: spacing.md,
  },
  statusContent: {
    flex: 1,
  },
  statusTitle: {
    ...typography.body,
    fontWeight: '600',
    marginBottom: spacing.xs,
  },
  statusSubtitle: {
    ...typography.small,
  },
  lastWorkout: {
    alignItems: 'center',
  },
  lastWorkoutLabel: {
    ...typography.small,
  },
  lastWorkoutDate: {
    ...typography.caption,
    color: colors.text,
    marginTop: spacing.xs,
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
