import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { api } from './api';

export const queryKeys = {
  progress: ['progress'] as const,
  workout: (day: number) => ['workout', day] as const,
};

export function useProgress() {
  return useQuery({
    queryKey: queryKeys.progress,
    queryFn: () => api.getProgress(),
    staleTime: 1000 * 60, // 1 minute
  });
}

export function useWorkout(dayNumber: number) {
  return useQuery({
    queryKey: queryKeys.workout(dayNumber),
    queryFn: () => api.getWorkout(dayNumber),
    enabled: dayNumber > 0,
    staleTime: 1000 * 60 * 5, // 5 minutes (workouts don't change often)
  });
}

export function useStartPlan() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: () => api.startPlan(),
    onSuccess: (data) => {
      queryClient.setQueryData(queryKeys.progress, data);
    },
  });
}

export function useCompleteWorkout() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: () => api.completeWorkout(),
    onSuccess: (data) => {
      queryClient.setQueryData(queryKeys.progress, data);
      // Invalidate workout query to refetch for new day
      queryClient.invalidateQueries({ queryKey: ['workout'] });
    },
  });
}
