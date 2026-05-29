// Preload script exposing app launcher IPC channels to the renderer

const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('launcherAPI', {
  getApps: () => ipcRenderer.invoke('get-apps'),
  addApp: (appEntry) => ipcRenderer.invoke('add-app', appEntry),
  removeApp: (id) => ipcRenderer.invoke('remove-app', id),
  launchApp: (exePath) => ipcRenderer.invoke('launch-app', exePath)
});
