# JavaScript Templates — Übersicht / Overview

Diese Vorlagen bieten fertige Startprojekte für verschiedene JavaScript-Anwendungen. Jede Vorlage enthält modernes ES2022+ mit ESM-Imports und async/await.

These templates provide ready-to-use starter projects for various JavaScript applications. Each template includes modern ES2022+ with ESM imports and async/await.

---

## Verzeichnisse / Directories

| Verzeichnis | Beschreibung | Description |
|---|---|---|
| **Web-Vanilla** | Vanilla HTML/CSS/JS App | Einfache Webanwendung ohne Framework |
| **Web-React** | React SPA | Moderne React-App mit JSX |
| **Web-Vue** | Vue 3 SPA | Vue 3 Composition API App |
| **Web-Angular** | Angular Komponente | AngularJS (1.x) Komponente |
| **Web-Svelte** | Svelte Komponente | Svelte Single-File Komponente |
| **CLI-Node** | Node.js CLI Tool | Kommandozeilenwerkzeug |
| **RESTAPI-Express** | Express.js REST API | Express REST-Schnittstelle |
| **RESTAPI-Fastify** | Fastify REST API | Fastify REST-Schnittstelle |
| **Game-Canvas2D** | HTML5 Canvas Spiel | 2D-Spiel auf Canvas-Basis |
| **Game-Phaser** | Phaser 3 Spiel | Phaser 3 Game Engine |
| **BrowserExtension** | Manifest V3 Extension | Browser-Erweiterung (Chromium) |
| **DiscordBot** | Discord Bot | discord.js Bot |
| **Electron-Desktop** | Electron Desktop App | Desktop-Anwendung |

## Verwendung / Usage

Jedes Verzeichnis enthält eine `package.json` und Quelldateien:

```bash
cd <template-name>
npm install
npm start       # oder npm run dev
```

Alle Vorlagen verwenden ESM (`"type": "module"`).
