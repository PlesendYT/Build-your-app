// Preload script exposing file system IPC channels to the renderer
// Provides open/save capabilities via contextBridge

const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('editorAPI', {
  // Open a markdown file via system dialog, returns content and path
  openFile: () => ipcRenderer.invoke('open-file'),

  // Save content to the given path, returns the saved file path
  saveFile: (content, filePath) => ipcRenderer.invoke('save-file', content, filePath)
});
