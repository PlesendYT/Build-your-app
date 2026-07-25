// Electron main process for the Video Player application
// Handles window creation and file drop IPC for loading videos

const { app, BrowserWindow, ipcMain, dialog } = require('electron');
const path = require('path');

// IPC: open a file dialog for selecting video files
ipcMain.handle('open-video', async () => {
  const { canceled, filePaths } = await dialog.showOpenDialog({
    filters: [{ name: 'Video', extensions: ['mp4', 'webm', 'mkv', 'avi', 'mov'] }],
    properties: ['openFile', 'multiSelections']
  });
  return canceled ? null : filePaths;
});

function createWindow() {
  const win = new BrowserWindow({
    width: 1100, height: 750,
    webPreferences: {
      preload: path.join(__dirname, 'preload.js'),
      contextIsolation: true,
      nodeIntegration: false,
      webSecurity: false // Required for file:// protocol video playback
    }
  });
  win.loadFile('../src/renderer/index.html');
}

app.whenReady().then(createWindow);
app.on('window-all-closed', () => app.quit());
