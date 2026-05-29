// Electron main process — creates the application window and registers IPC handlers
import { app, BrowserWindow, ipcMain } from 'electron';
import * as path from 'node:path';

// Create the main browser window with context isolation enabled
function createWindow(): void {
  const win = new BrowserWindow({
    width: 800,
    height: 600,
    webPreferences: {
      preload: path.join(__dirname, 'preload.js'),
      contextIsolation: true,
      nodeIntegration: false,
    },
  });

  win.loadFile(path.join(__dirname, '..', 'renderer', 'index.html'));
}

// Launch the window once Electron is ready
app.whenReady().then(createWindow);

// Quit when all windows are closed (except on macOS)
app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') app.quit();
});

// Re-create window on macOS activate event if none exist
app.on('activate', () => {
  if (BrowserWindow.getAllWindows().length === 0) createWindow();
});

// IPC handlers for renderer process requests
ipcMain.handle('get-version', () => app.getVersion());
ipcMain.handle('get-platform', () => process.platform);
