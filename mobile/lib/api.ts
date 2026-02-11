import { useAppStore } from './store';
import type { WorkoutDto, ProgressDto } from './types';

class ApiClient {
  private getBaseUrl(): string {
    return useAppStore.getState().apiUrl;
  }

  private getDeviceId(): string {
    const deviceId = useAppStore.getState().deviceId;
    if (!deviceId) throw new Error('Device ID not initialized');
    return deviceId;
  }

  async getWorkout(dayNumber: number): Promise<WorkoutDto | null> {
    const response = await fetch(`${this.getBaseUrl()}/workouts/day/${dayNumber}`);
    if (response.status === 404) return null;
    if (!response.ok) throw new Error('Failed to fetch workout');
    return response.json();
  }

  async getProgress(): Promise<ProgressDto | null> {
    const deviceId = this.getDeviceId();
    const response = await fetch(`${this.getBaseUrl()}/progress/${deviceId}`);
    if (response.status === 404) return null;
    if (!response.ok) throw new Error('Failed to fetch progress');
    return response.json();
  }

  async startPlan(): Promise<ProgressDto> {
    const deviceId = this.getDeviceId();
    const response = await fetch(`${this.getBaseUrl()}/progress/${deviceId}/start`, {
      method: 'POST',
    });
    if (!response.ok) throw new Error('Failed to start plan');
    return response.json();
  }

  async completeWorkout(): Promise<ProgressDto> {
    const deviceId = this.getDeviceId();
    const response = await fetch(`${this.getBaseUrl()}/progress/${deviceId}/complete`, {
      method: 'POST',
    });
    if (!response.ok) throw new Error('Failed to complete workout');
    return response.json();
  }
}

export const api = new ApiClient();
