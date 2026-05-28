# Electron-Desktop — Electron Desktop App

## Beschreibung
Desktop-Anwendung mit Electron. Enthält Fensterverwaltung, Haupt- und Renderer-Prozess sowie Preload-Script.

## Description
Desktop application built with Electron. Includes window management, main and renderer process, and preload script.

---

## Features
- Electron Hauptprozess
- Renderer-Prozess mit HTML/CSS/JS
- Preload-Script für sichere IPC
- Context Bridge für API-Exposition
- Native Menüs (optional)

## How to use / Verwendung
```bash
cd Electron-Desktop
npm install
npm start
```

## Tech Stack
- Electron 33
- Node.js 20+
- ESM (via esm oder CommonJS)

## Extension ideas / Erweiterungsideen
- Tray-Icon
- Auto-Updater
- Native Dialogs
- System-Tray
- IPC für Dateioperationen
