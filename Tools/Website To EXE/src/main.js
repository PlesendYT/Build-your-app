const {
  app, BrowserWindow, Menu, shell, Tray, Notification,
  globalShortcut, session, ipcMain, nativeTheme
} = require('electron');
const path = require('path');
const fs   = require('fs');

// ── Config laden ─────────────────────────────────────────────────────────────
const configPath = path.join(__dirname, '..', 'config.json');
let config;
try {
  config = JSON.parse(fs.readFileSync(configPath, 'utf8'));
} catch (e) {
  console.error('Konnte config.json nicht lesen:', e.message);
  process.exit(1);
}

const C = config;
let mainWindow = null;
let tray       = null;
let splashWin  = null;

// ── Hilfsfunktion: Datei vorhanden? ──────────────────────────────────────────
function fileExists(relPath) {
  return relPath && fs.existsSync(path.join(__dirname, '..', relPath));
}

// ── Security: ignoriere Zertifikatsfehler wenn aktiviert ─────────────────────
if (C.network && C.network.ignoreCertificateErrors) {
  app.commandLine.appendSwitch('ignore-certificate-errors');
}

// ── Proxy ────────────────────────────────────────────────────────────────────
if (C.network && C.network.proxy && C.network.proxy.enabled) {
  app.commandLine.appendSwitch('proxy-server', C.network.proxy.server);
  if (C.network.proxy.bypass) {
    app.commandLine.appendSwitch('proxy-bypass-list', C.network.proxy.bypass);
  }
}

// ── Offline Mode ─────────────────────────────────────────────────────────────
if (C.network && C.network.offlineMode) {
  app.commandLine.appendSwitch('host-resolver-rules', 'MAP * ~NOTFOUND');
}

// ── Logging ──────────────────────────────────────────────────────────────────
function log(level, ...args) {
  if (!C.logging || !C.logging.enabled) return;
  const levels = ['debug', 'info', 'warn', 'error'];
  if (levels.indexOf(level) < levels.indexOf(C.logging.level)) return;
  const line = `[${new Date().toISOString()}] [${level.toUpperCase()}] ${args.join(' ')}\n`;
  console.log(line.trim());
  try {
    const logFile = path.join(__dirname, '..', C.logging.file || 'logs/app.log');
    fs.mkdirSync(path.dirname(logFile), { recursive: true });
    fs.appendFileSync(logFile, line);
  } catch {}
}

// ── Splash Screen ─────────────────────────────────────────────────────────────
function createSplash() {
  if (!C.splashScreen || !C.splashScreen.enabled) return;
  splashWin = new BrowserWindow({
    width:  C.splashScreen.width  || 500,
    height: C.splashScreen.height || 300,
    transparent: true,
    frame: false,
    alwaysOnTop: true,
    skipTaskbar: true,
    resizable: false,
  });
  const imgPath = path.join(__dirname, '..', C.splashScreen.image || 'src/splash.png');
  if (fs.existsSync(imgPath)) {
    splashWin.loadFile(imgPath);
  } else {
    // Fallback HTML Splash
    splashWin.loadURL(`data:text/html,<body style="margin:0;background:#1a1a2e;display:flex;align-items:center;justify-content:center;height:100vh;font-family:sans-serif;color:white;font-size:24px">${C.app.name}</body>`);
  }
  splashWin.center();
}

// ── Hauptfenster ─────────────────────────────────────────────────────────────
function createWindow() {
  const w = C.window || {};
  const sec = C.security || {};

  const iconPath = fileExists(C.app.icon)
    ? path.join(__dirname, '..', C.app.icon)
    : undefined;

  const winOptions = {
    width:          w.width          || 1280,
    height:         w.height         || 800,
    minWidth:       w.minWidth       || 0,
    minHeight:      w.minHeight      || 0,
    resizable:      w.resizable      !== false,
    fullscreen:     w.fullscreen     || false,
    fullscreenable: w.fullscreenable !== false,
    center:         w.center         !== false,
    frame:          w.frame          !== false,
    transparent:    w.transparent    || false,
    alwaysOnTop:    w.alwaysOnTop    || false,
    skipTaskbar:    w.skipTaskbar    || false,
    kiosk:          w.kiosk          || false,
    hasShadow:      w.hasShadow      !== false,
    backgroundColor: w.backgroundColor || '#ffffff',
    opacity:        w.opacity        !== undefined ? w.opacity : 1.0,
    title:          w.title          || C.app.name,
    icon:           iconPath,
    show: false, // erst nach ready-to-show zeigen
    webPreferences: {
      nodeIntegration:             sec.nodeIntegration             || false,
      contextIsolation:            sec.contextIsolation            !== false,
      webSecurity:                 sec.webSecurity                 !== false,
      allowRunningInsecureContent: sec.allowRunningInsecureContent || false,
      sandbox:                     sec.sandbox                     || false,
      spellcheck:                  C.browser && C.browser.spellcheck !== false,
      images:                      C.browser ? C.browser.images !== false : true,
      javascript:                  C.browser ? C.browser.javascript !== false : true,
      webgl:                       C.browser ? C.browser.webgl !== false : true,
      plugins:                     C.browser && C.browser.plugins || false,
      experimentalFeatures:        C.browser && C.browser.experimentalFeatures || false,
      zoomFactor:                  C.browser && C.browser.zoomFactor || 1.0,
    },
  };

  // Maximale Größe nur setzen wenn > 0
  if (w.maxWidth  && w.maxWidth  > 0) winOptions.maxWidth  = w.maxWidth;
  if (w.maxHeight && w.maxHeight > 0) winOptions.maxHeight = w.maxHeight;

  mainWindow = new BrowserWindow(winOptions);

  // ── User-Agent ──────────────────────────────────────────────────────────────
  if (C.network && C.network.userAgent) {
    mainWindow.webContents.setUserAgent(C.network.userAgent);
  }

  // ── Website laden ───────────────────────────────────────────────────────────
  const url = w.url || 'https://example.com';
  mainWindow.loadURL(url);
  log('info', 'Lade URL:', url);

  // ── Fenster anzeigen (nach Splash oder sofort) ──────────────────────────────
  mainWindow.once('ready-to-show', () => {
    if (splashWin) {
      const delay = (C.splashScreen && C.splashScreen.duration) || 3000;
      setTimeout(() => {
        if (splashWin) { splashWin.close(); splashWin = null; }
        showMainWindow();
      }, delay);
    } else {
      showMainWindow();
    }
  });

  function showMainWindow() {
    if (w.maximized) {
      mainWindow.maximize();
    } else {
      mainWindow.show();
    }
  }

  // ── DevTools ─────────────────────────────────────────────────────────────────
  const dt = C.devTools || {};
  if (dt.enabled && dt.openOnStart) {
    const mode = dt.mode || 'detach';
    mainWindow.webContents.openDevTools({ mode });
  }

  // ── Smooth Scrolling ─────────────────────────────────────────────────────────
  if (C.browser && C.browser.smoothScrolling === false) {
    mainWindow.webContents.executeJavaScript(
      'document.documentElement.style.scrollBehavior = "auto"'
    );
  }

  // ── Zoom ─────────────────────────────────────────────────────────────────────
  if (C.browser && !C.browser.zoom) {
    mainWindow.webContents.on('before-input-event', (e, input) => {
      if (input.control && ['+', '-', '=', '0'].includes(input.key)) {
        e.preventDefault();
      }
    });
  }
  if (C.browser && C.browser.minZoom) {
    mainWindow.webContents.setVisualZoomLevelLimits(
      C.browser.minZoom,
      C.browser.maxZoom || 3.0
    );
  }

  // ── Kontextmenü ──────────────────────────────────────────────────────────────
  if (C.navigation && C.navigation.contextMenu === false || 
      C.browser && C.browser.contextMenu === false) {
    mainWindow.webContents.on('context-menu', (e) => e.preventDefault());
  }

  // ── Menüleiste ────────────────────────────────────────────────────────────────
  if (!C.navigation || C.navigation.menuBar === false) {
    Menu.setApplicationMenu(null);
    mainWindow.setMenuBarVisibility(false);
  }

  // ── Neue Fenster / Popups ─────────────────────────────────────────────────────
  mainWindow.webContents.setWindowOpenHandler(({ url: newUrl }) => {
    const nav = C.navigation || {};
    // Geblockte Hosts
    if (nav.blockedHosts && nav.blockedHosts.length > 0) {
      try {
        const hostname = new URL(newUrl).hostname;
        if (nav.blockedHosts.some(h => hostname.includes(h))) {
          log('info', 'Blockiert:', newUrl);
          return { action: 'deny' };
        }
      } catch {}
    }
    // Externe Links im Browser öffnen
    if (nav.externalLinksInBrowser) {
      shell.openExternal(newUrl);
      return { action: 'deny' };
    }
    // Neue Fenster erlaubt?
    if (nav.allowNewWindows) {
      return { action: 'allow' };
    }
    return { action: 'deny' };
  });

  // ── Navigation zu anderen Hosts blockieren ────────────────────────────────────
  if (C.navigation && C.navigation.allowedHosts && C.navigation.allowedHosts.length > 0) {
    mainWindow.webContents.on('will-navigate', (e, navUrl) => {
      try {
        const hostname = new URL(navUrl).hostname;
        const allowed = C.navigation.allowedHosts;
        if (!allowed.some(h => hostname.includes(h))) {
          e.preventDefault();
          log('info', 'Navigation blockiert:', navUrl);
        }
      } catch {}
    });
  }

  // ── Blocked Hosts für Navigation ──────────────────────────────────────────────
  if (C.navigation && C.navigation.blockedHosts && C.navigation.blockedHosts.length > 0) {
    mainWindow.webContents.on('will-navigate', (e, navUrl) => {
      try {
        const hostname = new URL(navUrl).hostname;
        if (C.navigation.blockedHosts.some(h => hostname.includes(h))) {
          e.preventDefault();
          log('info', 'Navigation blockiert:', navUrl);
        }
      } catch {}
    });
  }

  // ── Tray ──────────────────────────────────────────────────────────────────────
  if (C.tray && C.tray.enabled) {
    setupTray();
  }

  // ── Minimize to Tray ──────────────────────────────────────────────────────────
  mainWindow.on('minimize', (e) => {
    if (C.tray && C.tray.enabled && C.tray.minimizeToTray) {
      e.preventDefault();
      mainWindow.hide();
    }
  });

  // ── Close to Tray ────────────────────────────────────────────────────────────
  mainWindow.on('close', (e) => {
    if (C.tray && C.tray.enabled && C.tray.closeToTray && !app.isQuiting) {
      e.preventDefault();
      mainWindow.hide();
    }
  });

  // ── Session: Cache & Cookies beim Beenden löschen ────────────────────────────
  app.on('before-quit', async () => {
    const ses = session.defaultSession;
    if (C.session && C.session.clearCookiesOnExit) {
      await ses.clearStorageData({ storages: ['cookies'] });
      log('info', 'Cookies gelöscht');
    }
    if (C.session && C.session.clearCacheOnExit) {
      await ses.clearCache();
      log('info', 'Cache gelöscht');
    }
  });

  log('info', 'Hauptfenster erstellt');
}

// ── Tray Setup ────────────────────────────────────────────────────────────────
function setupTray() {
  if (!C.tray) return;
  const iconPath = fileExists(C.tray.icon)
    ? path.join(__dirname, '..', C.tray.icon)
    : path.join(__dirname, '..', C.app.icon);

  try {
    tray = new Tray(iconPath);
    tray.setToolTip(C.tray.tooltip || C.app.name);
    const contextMenu = Menu.buildFromTemplate([
      { label: 'Öffnen', click: () => { mainWindow.show(); mainWindow.focus(); } },
      { type: 'separator' },
      { label: 'Beenden', click: () => { app.isQuiting = true; app.quit(); } }
    ]);
    tray.setContextMenu(contextMenu);
    tray.on('click', () => {
      if (mainWindow.isVisible()) {
        mainWindow.hide();
      } else {
        mainWindow.show();
        mainWindow.focus();
      }
    });
  } catch (e) {
    log('warn', 'Tray konnte nicht erstellt werden:', e.message);
  }
}

// ── Globale Shortcuts registrieren ───────────────────────────────────────────
function registerShortcuts() {
  const sc = C.shortcuts;
  if (!sc || !sc.enabled) return;

  if (sc.reload) {
    globalShortcut.register('F5', () => mainWindow && mainWindow.webContents.reload());
  }
  if (sc.forceReload) {
    globalShortcut.register('CommandOrControl+Shift+R', () => mainWindow && mainWindow.webContents.reloadIgnoringCache());
  }
  if (sc.devTools && C.devTools && C.devTools.enabled) {
    globalShortcut.register('F12', () => {
      if (mainWindow) mainWindow.webContents.toggleDevTools();
    });
    globalShortcut.register('CommandOrControl+Shift+I', () => {
      if (mainWindow) mainWindow.webContents.toggleDevTools();
    });
  }
  if (sc.fullscreen) {
    globalShortcut.register('F11', () => {
      if (mainWindow) mainWindow.setFullScreen(!mainWindow.isFullScreen());
    });
  }
  if (sc.minimize) {
    globalShortcut.register('CommandOrControl+M', () => {
      if (mainWindow) mainWindow.minimize();
    });
  }
  if (sc.close) {
    globalShortcut.register('CommandOrControl+W', () => {
      if (mainWindow) mainWindow.close();
    });
  }

  // Custom Shortcuts
  if (sc.custom && sc.custom.length > 0) {
    sc.custom.forEach(({ key, action }) => {
      try {
        globalShortcut.register(key, () => {
          if (mainWindow && action) {
            mainWindow.webContents.executeJavaScript(action);
          }
        });
      } catch (e) {
        log('warn', 'Custom Shortcut fehlgeschlagen:', key, e.message);
      }
    });
  }
}

// ── Session konfigurieren ─────────────────────────────────────────────────────
function configureSession() {
  const ses = session.defaultSession;

  // Cache Größe
  if (C.session && C.session.cacheSize > 0) {
    ses.setStoragePath && log('info', 'Cache-Größe:', C.session.cacheSize);
  }

  // Partition
  // (wird beim BrowserWindow gesetzt, nicht global)

  // Notifications
  if (C.notifications && !C.notifications.enabled) {
    ses.setPermissionRequestHandler((webContents, permission, callback) => {
      if (permission === 'notifications') return callback(false);
      callback(true);
    });
  }

  // WebGL deaktivieren falls nötig
  if (C.browser && !C.browser.webgl) {
    app.commandLine.appendSwitch('disable-gpu');
  }
}

// ── App Start ─────────────────────────────────────────────────────────────────
app.whenReady().then(() => {
  configureSession();

  if (C.splashScreen && C.splashScreen.enabled) {
    createSplash();
  }

  createWindow();
  registerShortcuts();

  app.on('activate', () => {
    if (BrowserWindow.getAllWindows().length === 0) createWindow();
  });
});

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    globalShortcut.unregisterAll();
    app.quit();
  }
});

app.on('will-quit', () => {
  globalShortcut.unregisterAll();
});
