// Electron main process with SQLite CRUD using better-sqlite3
// Run: npm install && npm start

const { app, BrowserWindow, ipcMain } = require("electron");
const Database = require("better-sqlite3");
const path = require("path");

// Open or create a local SQLite database file
const db = new Database(path.join(__dirname, "app.db"));
db.pragma("journal_mode = WAL"); // Enable WAL mode for better concurrency

// Create the notes table if it does not exist
db.exec(`
  CREATE TABLE IF NOT EXISTS notes (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    title TEXT NOT NULL,
    content TEXT,
    created_at TEXT DEFAULT (datetime('now'))
  )
`);

// IPC handler: return all notes ordered by newest first
ipcMain.handle("get-notes", () => {
  return db.prepare("SELECT * FROM notes ORDER BY created_at DESC").all();
});

// IPC handler: insert a new note and return it
ipcMain.handle("add-note", (_event, title, content) => {
  const stmt = db.prepare("INSERT INTO notes (title, content) VALUES (?, ?)");
  const result = stmt.run(title, content);
  return db.prepare("SELECT * FROM notes WHERE id = ?").get(result.lastInsertRowid);
});

// IPC handler: update an existing note by id
ipcMain.handle("update-note", (_event, id, title, content) => {
  db.prepare("UPDATE notes SET title = ?, content = ? WHERE id = ?").run(title, content, id);
  return db.prepare("SELECT * FROM notes WHERE id = ?").get(id);
});

// IPC handler: delete a note by id
ipcMain.handle("delete-note", (_event, id) => {
  db.prepare("DELETE FROM notes WHERE id = ?").run(id);
  return { success: true };
});

// Create the main application window
function createWindow() {
  const win = new BrowserWindow({
    width: 800,
    height: 600,
    webPreferences: {
      preload: path.join(__dirname, "preload.js"),
      contextIsolation: true,
      nodeIntegration: false,
    },
  });
  win.loadFile(path.join(__dirname, "../public/index.html"));
}

app.whenReady().then(createWindow);

app.on("window-all-closed", () => {
  db.close();
  app.quit();
});
