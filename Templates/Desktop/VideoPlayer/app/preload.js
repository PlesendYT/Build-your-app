// Preload script exposing video file IPC channel to the renderer

const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('videoAPI', {
  openVideo: () => ipcRenderer.invoke('open-video')
});
