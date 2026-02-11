import { create } from 'zustand';
import * as Crypto from 'expo-crypto';
import AsyncStorage from '@react-native-async-storage/async-storage';

interface AppStore {
  deviceId: string | null;
  apiUrl: string;
  setApiUrl: (url: string) => void;
  initializeDeviceId: () => Promise<string>;
}

const DEVICE_ID_KEY = '@workout_device_id';
const API_URL_KEY = '@workout_api_url';
const DEFAULT_API_URL = 'http://localhost:5032/api';

export const useAppStore = create<AppStore>((set, get) => ({
  deviceId: null,
  apiUrl: DEFAULT_API_URL,

  setApiUrl: async (url: string) => {
    await AsyncStorage.setItem(API_URL_KEY, url);
    set({ apiUrl: url });
  },

  initializeDeviceId: async () => {
    // Try to load existing device ID
    let deviceId = await AsyncStorage.getItem(DEVICE_ID_KEY);
    
    if (!deviceId) {
      // Generate new device ID
      deviceId = Crypto.randomUUID();
      await AsyncStorage.setItem(DEVICE_ID_KEY, deviceId);
    }

    // Also load saved API URL
    const savedApiUrl = await AsyncStorage.getItem(API_URL_KEY);
    
    set({ 
      deviceId,
      apiUrl: savedApiUrl || DEFAULT_API_URL 
    });
    
    return deviceId;
  },
}));
