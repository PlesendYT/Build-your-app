// Preload script exposing secure IPC channels to the renderer process
// Uses contextBridge to safely expose the password manager API

const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('passwordAPI', {
  // Verify the master password before granting access to the vault
  verifyMasterPassword: (password) => ipcRenderer.invoke('verify-master-password', password),

  // Retrieve the full list of stored credentials
  getPasswords: () => ipcRenderer.invoke('get-passwords'),

  // Add a new credential entry with site, username, and password fields
  addPassword: (entry) => ipcRenderer.invoke('add-password', entry),

  // Update an existing credential entry identified by its id
  updatePassword: (id, entry) => ipcRenderer.invoke('update-password', id, entry),

  // Delete a credential entry by its unique id
  deletePassword: (id) => ipcRenderer.invoke('delete-password', id)
});
