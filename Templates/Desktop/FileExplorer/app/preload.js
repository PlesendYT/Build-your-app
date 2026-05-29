// Preload script exposing file system IPC channels to the renderer

const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('fileAPI', {
  listDirectory: (dirPath) => ipcRenderer.invoke('list-directory', dirPath),
  getHomeDir: () => ipcRenderer.invoke('get-home-dir'),
  createDirectory: (parentPath, name) => ipcRenderer.invoke('create-directory', parentPath, name),
  deleteEntry: (targetPath) => ipcRenderer.invoke('delete-entry', targetPath),
  readFile: (filePath) => ipcRenderer.invoke('read-file', filePath)
});
