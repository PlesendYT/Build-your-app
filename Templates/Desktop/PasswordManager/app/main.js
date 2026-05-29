// Electron main process for the Password Manager application
// Handles window creation, IPC communication, and encrypted credential storage

const { app, BrowserWindow, ipcMain } = require('electron');
const path = require('path');
const crypto = require('crypto');
const fs = require('fs');

// Path to the encrypted vault file and encryption algorithm constants
const STORAGE_PATH = path.join(app.getPath('userData'), 'vault.enc');
const ALGORITHM = 'aes-256-gcm';
const MASTER_KEY = crypto.createHash('sha256').update('default-master-password').digest();

// Decrypt and load the password vault from the encrypted storage file
function loadVault() {
  try {
    const data = fs.readFileSync(STORAGE_PATH);
    const iv = data.slice(0, 16);
    const tag = data.slice(16, 32);
    const encrypted = data.slice(32);
    const decipher = crypto.createDecipheriv(ALGORITHM, MASTER_KEY, iv);
    decipher.setAuthTag(tag);
    return JSON.parse(decipher.update(encrypted) + decipher.final('utf8'));
  } catch {
    return [];
  }
}

// Encrypt and save the password vault array to the storage file
function saveVault(vault) {
  const iv = crypto.randomBytes(16);
  const cipher = crypto.createCipheriv(ALGORITHM, MASTER_KEY, iv);
  const encrypted = Buffer.concat([cipher.update(JSON.stringify(vault), 'utf8'), cipher.final()]);
  fs.writeFileSync(STORAGE_PATH, Buffer.concat([iv, cipher.getAuthTag(), encrypted]));
}

// IPC: verify the provided master password against the default hash
ipcMain.handle('verify-master-password', (_, password) => {
  return crypto.createHash('sha256').update(password).digest().equals(MASTER_KEY);
});

// IPC: return all stored credentials from the decrypted vault
ipcMain.handle('get-passwords', () => loadVault());

// IPC: add a new credential entry to the vault and persist it
ipcMain.handle('add-password', (_, entry) => {
  const vault = loadVault();
  vault.push({ id: Date.now(), ...entry });
  saveVault(vault);
  return vault;
});

// IPC: update an existing credential entry by id
ipcMain.handle('update-password', (_, id, entry) => {
  const vault = loadVault();
  const idx = vault.findIndex(e => e.id === id);
  if (idx !== -1) vault[idx] = { ...vault[idx], ...entry };
  saveVault(vault);
  return vault;
});

// IPC: delete a credential entry from the vault by id
ipcMain.handle('delete-password', (_, id) => {
  const vault = loadVault().filter(e => e.id !== id);
  saveVault(vault);
  return vault;
});

// Create the main application window with a fixed size
function createWindow() {
  const win = new BrowserWindow({
    width: 900, height: 700,
    webPreferences: { preload: path.join(__dirname, 'preload.js'), contextIsolation: true }
  });
  win.loadFile('../src/renderer/index.html');
}

app.whenReady().then(createWindow);
app.on('window-all-closed', () => app.quit());
