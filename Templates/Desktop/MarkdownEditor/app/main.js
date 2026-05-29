// Electron main process for the Markdown Editor application
// Manages file open/save dialogs and the preview window

const { app, BrowserWindow, ipcMain, dialog } = require('electron');
const path = require('path');
const fs = require('fs');

// IPC: open a file dialog and return the selected markdown file content
ipcMain.handle('open-file', async () => {
  const { canceled, filePaths } = await dialog.showOpenDialog({ filters: [{ name: 'Markdown', extensions: ['md', 'markdown'] }] });
  if (canceled) return null;
  return { content: fs.readFileSync(filePaths[0], 'utf8'), filePath: filePaths[0] };
});

// IPC: save content to the given file path, or prompt for a save dialog
ipcMain.handle('save-file', async (_, content, filePath) => {
  if (!filePath) {
    const { canceled, filePath: newPath } = await dialog.showSaveDialog({ filters: [{ name: 'Markdown', extensions: ['md'] }] });
    if (canceled) return null;
    filePath = newPath;
  }
  fs.writeFileSync(filePath, content, 'utf8');
  return filePath;
});

// Create the main application window with preload script
function createWindow() {
  const win = new BrowserWindow({
    width: 1200, height: 800,
    webPreferences: { preload: path.join(__dirname, 'preload.js'), contextIsolation: true }
  });
  win.loadFile('../src/renderer/index.html');
}

app.whenReady().then(createWindow);
app.on('window-all-closed', () => app.quit());
