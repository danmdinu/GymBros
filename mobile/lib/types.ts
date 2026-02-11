// API Types matching backend DTOs

export interface ExerciseMetric {
  type: 'reps' | 'time';
  reps?: number;
  seconds?: number;
}

export interface WorkoutExerciseDto {
  id: string;
  order: number;
  supersetGroup: string | null;
  sets: number;
  metric: ExerciseMetric;
  notes: string | null;
  exerciseName: string;
  exerciseDescription: string | null;
}

export interface WorkoutSectionDto {
  id: string;
  order: number;
  type: 'WarmUp' | 'Main' | 'CoolDown';
  name: string;
  exercises: WorkoutExerciseDto[];
}

export interface WorkoutDto {
  id: string;
  dayNumber: number;
  name: string;
  description: string | null;
  sections: WorkoutSectionDto[];
}

export interface ProgressDto {
  id: string;
  deviceId: string;
  currentDayNumber: number;
  startedAt: string;
  completedAt: string | null;
  lastWorkoutCompletedAt: string | null;
  isCompleted: boolean;
}
