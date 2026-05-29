// Electron main process for the Note Taking application
// Manages folders, notes, and persists data to JSON storage

const { app, BrowserWindow, ipcMain } = require('electron');
const path = require('path');
const fs = require('fs');

const DATA_PATH = path.join(app.getPath('userData'), 'notes.json');

// Load the notes database from the JSON file, returning empty defaults
function loadData() {
  try {
    return JSON.parse(fs.readFileSync(DATA_PATH, 'utf8'));
  } catch {
    return { folders: ['General'], notes: [] };
  }
}

// Save the entire notes database to the JSON file
function saveData(data) {
  fs.writeFileSync(DATA_PATH, JSON.stringify(data, null, 2));
}

// IPC: return the full notes data including folders and notes
ipcMain.handle('get-notes', () => loadData());

// IPC: add a new folder to the folder list
ipcMain.handle('add-folder', (_, name) => {
  const data = loadData();
  if (!data.folders.includes(name)) data.folders.push(name);
  saveData(data);
  return data;
});

// IPC: add a new note with title, content, and folder assignment
ipcMain.handle('add-note', (_, note) => {
  const data = loadData();
  data.notes.push({ id: Date.now(), title: note.title, content: note.content, folder: note.folder, updated: new Date().toISOString() });
  saveData(data);
  return data;
});

// IPC: update an existing note's fields by id
ipcMain.handle('update-note', (_, id, fields) => {
  const data = loadData();
  const note = data.notes.find(n => n.id === id);
  if (note) Object.assign(note, fields, { updated: new Date().toISOString() });
  saveData(data);
  return data;
});

// IPC: delete a note by its id
ipcMain.handle('delete-note', (_, id) => {
  const data = loadData();
  data.notes = data.notes.filter(n => n.id !== id);
  saveData(data);
  return data;
});

function createWindow() {
  const win = new BrowserWindow({
    width: 1100, height: 750,
    webPreferences: { preload: path.join(__dirname, 'preload.js'), contextIsolation: true }
  });
  win.loadFile('../src/renderer/index.html');
}

app.whenReady().then(createWindow);
app.on('window-all-closed', () => app.quit());
