// Main process — entry point for the Electron application
// Creates the browser window, handles app lifecycle, and registers IPC handlers

const { app, BrowserWindow, ipcMain } = require('electron');
const path = require('node:path');

/** Creates the main application window with preload script for secure context bridging */
function createWindow() {
  const win = new BrowserWindow({
    width: 800,
    height: 600,
    webPreferences: {
      preload: path.join(__dirname, 'preload.js'),
      contextIsolation: true,
      nodeIntegration: false,
    },
  });

  win.loadFile(path.join(__dirname, 'renderer', 'index.html'));
}

// Create window when Electron is ready
app.whenReady().then(createWindow);

// Quit when all windows are closed (except on macOS)
app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') app.quit();
});

// Re-create window on macOS if dock icon is clicked and no windows exist
app.on('activate', () => {
  if (BrowserWindow.getAllWindows().length === 0) createWindow();
});

// IPC handlers — respond to renderer requests
ipcMain.handle('get-version', () => app.getVersion());

ipcMain.handle('get-platform', () => process.platform);
