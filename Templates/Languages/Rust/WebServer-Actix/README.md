# WebServer-Actix

**Deutsch:** REST-API-Server mit Actix-Web 4. Enthält Gesundheitsendpunkt,
Informationsendpunkt mit Zähler und Item-Erstellung.

**English:** REST API server with Actix-Web 4. Includes health endpoint,
info endpoint with counter, and item creation.

## Features
- JSON-API (DE/EN)
- State-Management (DE) / State management (EN)
- Fehlerbehandlung (DE) / Error handling (EN)

## Verwendung / Usage
```bash
cargo run
curl http://localhost:8080/health
curl http://localhost:8080/info
curl -X POST http://localhost:8080/items -H 'Content-Type: application/json' -d '{"name":"test","value":"123"}'
```

## Tech Stack
Rust 2024, actix-web 4, serde, tokio
