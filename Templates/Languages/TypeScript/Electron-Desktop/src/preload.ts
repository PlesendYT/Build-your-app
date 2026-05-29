// Preload script — exposes a safe API from the main process to the renderer via contextBridge
import { contextBridge, ipcRenderer } from 'electron';

contextBridge.exposeInMainWorld('electronAPI', {
  getVersion: (): Promise<string> => ipcRenderer.invoke('get-version'),
  getPlatform: (): Promise<string> => ipcRenderer.invoke('get-platform'),
});
