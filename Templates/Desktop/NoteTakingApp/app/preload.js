// Preload script exposing note-taking IPC channels to the renderer

const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('notesAPI', {
  getNotes: () => ipcRenderer.invoke('get-notes'),
  addFolder: (name) => ipcRenderer.invoke('add-folder', name),
  addNote: (note) => ipcRenderer.invoke('add-note', note),
  updateNote: (id, fields) => ipcRenderer.invoke('update-note', id, fields),
  deleteNote: (id) => ipcRenderer.invoke('delete-note', id)
});
