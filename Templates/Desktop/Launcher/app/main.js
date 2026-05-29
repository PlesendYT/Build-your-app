// Electron main process for the App Launcher
// Manages a grid of installed apps with search functionality

const { app, BrowserWindow, ipcMain, shell } = require('electron');
const path = require('path');
const fs = require('fs');
const os = require('os');

const CONFIG_PATH = path.join(app.getPath('userData'), 'launcher.json');

// Load the app list from the JSON config file
function loadApps() {
  try { return JSON.parse(fs.readFileSync(CONFIG_PATH, 'utf8')); }
  catch { return []; }
}

// Save the app list to the JSON config file
function saveApps(apps) {
  fs.writeFileSync(CONFIG_PATH, JSON.stringify(apps, null, 2));
}

// IPC: return the current app list
ipcMain.handle('get-apps', () => loadApps());

// IPC: add a new app entry with name and executable path
ipcMain.handle('add-app', (_, appEntry) => {
  const apps = loadApps();
  apps.push({ id: Date.now(), ...appEntry });
  saveApps(apps);
  return apps;
});

// IPC: remove an app entry by its id
ipcMain.handle('remove-app', (_, id) => {
  const apps = loadApps().filter(a => a.id !== id);
  saveApps(apps);
  return apps;
});

// IPC: launch an app by its executable path
ipcMain.handle('launch-app', (_, exePath) => {
  shell.openPath(exePath);
});

function createWindow() {
  const win = new BrowserWindow({
    width: 900, height: 650,
    webPreferences: { preload: path.join(__dirname, 'preload.js'), contextIsolation: true }
  });
  win.loadFile('../src/renderer/index.html');
}

app.whenReady().then(createWindow);
app.on('window-all-closed', () => app.quit());
