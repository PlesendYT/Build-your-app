// Electron main process for the File Explorer application
// Provides file system navigation and file operation handlers

const { app, BrowserWindow, ipcMain } = require('electron');
const path = require('path');
const fs = require('fs');
const os = require('os');

// IPC: return directory listing with file stats for the given path
ipcMain.handle('list-directory', (_, dirPath) => {
  const entries = fs.readdirSync(dirPath, { withFileTypes: true });
  return entries.map(e => ({
    name: e.name,
    isDirectory: e.isDirectory(),
    size: e.isFile() ? fs.statSync(path.join(dirPath, e.name)).size : 0,
    path: path.join(dirPath, e.name)
  })).sort((a, b) => b.isDirectory - a.isDirectory || a.name.localeCompare(b.name));
});

// IPC: return the home directory as the default starting path
ipcMain.handle('get-home-dir', () => os.homedir());

// IPC: create a new directory at the given parent path
ipcMain.handle('create-directory', (_, parentPath, name) => {
  fs.mkdirSync(path.join(parentPath, name));
  return true;
});

// IPC: delete a file or directory at the given path
ipcMain.handle('delete-entry', (_, targetPath) => {
  fs.rmSync(targetPath, { recursive: true, force: true });
  return true;
});

// IPC: read and return the content of a text file
ipcMain.handle('read-file', (_, filePath) => {
  return fs.readFileSync(filePath, 'utf8');
});

function createWindow() {
  const win = new BrowserWindow({
    width: 1000, height: 700,
    webPreferences: { preload: path.join(__dirname, 'preload.js'), contextIsolation: true }
  });
  win.loadFile('../src/renderer/index.html');
}

app.whenReady().then(createWindow);
app.on('window-all-closed', () => app.quit());
