import { contextBridge, ipcRenderer } from 'electron';

contextBridge.exposeInMainWorld('electronAPI', {
  getVersion: (): Promise<string> => ipcRenderer.invoke('get-version'),
  getPlatform: (): Promise<string> => ipcRenderer.invoke('get-platform'),
});
