// Preload script exposing music library IPC channels to the renderer

const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('musicAPI', {
  scanLibrary: () => ipcRenderer.invoke('scan-library'),
  getHome: () => ipcRenderer.invoke('get-home')
});
