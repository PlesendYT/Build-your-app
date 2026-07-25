// Electron main process for the Music Player application
// Handles audio file scanning, library management, and playback IPC

const { app, BrowserWindow, ipcMain, dialog } = require('electron');
const path = require('path');
const fs = require('fs');
const os = require('os');

// Scan a directory recursively for audio files and return their metadata
function scanDirectory(dirPath) {
  const results = [];
  try {
    const entries = fs.readdirSync(dirPath, { withFileTypes: true });
    for (const e of entries) {
      const fullPath = path.join(dirPath, e.name);
      if (e.isDirectory()) results.push(...scanDirectory(fullPath));
      else if (e.name.match(/\.(mp3|wav|flac|ogg|m4a)$/i)) {
        const stats = fs.statSync(fullPath);
        results.push({ name: e.name.replace(/\.[^.]+$/, ''), path: fullPath, size: stats.size });
      }
    }
  } catch {}
  return results;
}

// IPC: open a directory dialog and scan it for audio files
ipcMain.handle('scan-library', async () => {
  const { canceled, filePaths } = await dialog.showOpenDialog({ properties: ['openDirectory'] });
  if (canceled) return [];
  return scanDirectory(filePaths[0]);
});

// IPC: return the home directory for default library location
ipcMain.handle('get-home', () => os.homedir());

function createWindow() {
  const win = new BrowserWindow({
    width: 1000, height: 700,
    webPreferences: {
      preload: path.join(__dirname, 'preload.js'),
      contextIsolation: true,
      nodeIntegration: false,
      webSecurity: false // Required for file:// protocol audio playback
    }
  });
  win.loadFile('../src/renderer/index.html');
}

app.whenReady().then(createWindow);
app.on('window-all-closed', () => app.quit());
