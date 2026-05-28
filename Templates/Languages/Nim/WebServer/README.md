# WebServer

**Deutsch:** Ein asynchroner HTTP-Server in Nim mit Jester. Enthält Routing,
JSON-API, statische Dateien und Middleware.

**English:** An async HTTP server in Nim with Jester. Includes routing, JSON
API, static file serving, and middleware.

## Features
- AsyncDispatch Event-Loop (DE) / AsyncDispatch event loop (EN)
- RESTful JSON-Endpunkte (DE) / RESTful JSON endpoints (EN)
- Statische Dateiauslieferung (DE) / Static file serving (EN)
- CORS-Middleware (DE) / CORS middleware (EN)

## Verwendung / Usage
```bash
nimble run
# Server läuft auf http://localhost:8080
```

## Tech Stack
Nim 2.0+, Jester, asyncdispatch, json, strutils
