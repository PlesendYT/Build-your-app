// Preload script exposes a safe API from main to renderer via contextBridge
// This keeps the renderer process sandboxed (no direct Node.js access)

const { contextBridge, ipcRenderer } = require("electron");

// Expose a `notes` API object on the window for the renderer to use
contextBridge.exposeInMainWorld("notes", {
  // Fetch all notes from the database
  getAll: () => ipcRenderer.invoke("get-notes"),

  // Create a new note with title and content
  add: (title, content) => ipcRenderer.invoke("add-note", title, content),

  // Update an existing note by its id
  update: (id, title, content) =>
    ipcRenderer.invoke("update-note", id, title, content),

  // Delete a note by its id
  delete: (id) => ipcRenderer.invoke("delete-note", id),
});
