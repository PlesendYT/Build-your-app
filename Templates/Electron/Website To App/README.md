# Electron App Template

Einfaches Template: Config bearbeiten → build.bat doppelklicken → fertige .exe

---

## Schnellstart

### 1. Voraussetzungen
- [Node.js](https://nodejs.org) installieren (LTS-Version empfohlen)
- Das war's!

### 2. config.json anpassen

Öffne `config.json` und passe die Werte an:

```json
{
  "app": {
    "name": "MeineApp",          // Name der App
    "version": "1.0.0",          // Versionsnummer
    "author": "Dein Name",
    "description": "...",
    "icon": "src/icon.ico"       // Pfad zum Icon (.ico Datei)
  },

  "window": {
    "width": 1280,               // Fensterbreite in Pixel
    "height": 800,               // Fensterhöhe in Pixel
    "minWidth": 800,
    "minHeight": 600,
    "resizable": true,           // Größe änderbar?
    "fullscreenable": true,
    "title": "Meine App",        // Fenstertitel
    "url": "https://example.com" // ← DEINE WEBSITE URL
  },

  "tools": {
    "devTools": false,           // F12 DevTools ein/aus
    "contextMenu": true,         // Rechtsklick-Menü ein/aus
    "zoom": true,                // Strg+/- Zoom ein/aus
    "navigation": false,         // Navigationsleiste ein/aus
    "externalLinks": true        // Externe Links im Browser öffnen
  },

  "build": {
    "outputDir": "dist",         // Ausgabe-Ordner
    "portable": true,            // Portable .exe (kein Installer)
    "installer": false,          // Installer .exe zusätzlich bauen
    "targetArch": "x64"          // x64 oder ia32
  }
}
```

### 3. build.bat doppelklicken

```
build.bat doppelklicken
    ↓
Dependencies werden automatisch installiert (nur beim ersten Mal)
    ↓
config.json wird eingelesen
    ↓
.exe wird gebaut
    ↓
Fertig! → dist/ Ordner
```

---

## Icon hinzufügen

Lege eine `.ico` Datei unter `src/icon.ico` ab.  
Kostenloses Tool zum Erstellen: https://www.icoconverter.com

## Testen ohne Build

```
npm install
npm start
```

---

## Ordnerstruktur

```
electron-template/
├── config.json      ← Hier alles einstellen
├── build.bat        ← Doppelklicken zum Bauen
├── package.json     ← Wird automatisch befüllt
├── src/
│   ├── main.js      ← Electron Hauptprozess
│   └── icon.ico     ← App-Icon (selbst hinzufügen)
└── dist/            ← Hier landet die fertige .exe
```
