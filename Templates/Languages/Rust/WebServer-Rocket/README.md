# WebServer-Rocket

**Deutsch:** REST-API-Server mit Rocket 0.5. Nutzt Rocket-Makros für
deklaratives Routing und Built-in JSON-Support.

**English:** REST API server with Rocket 0.5. Uses Rocket macros for
declarative routing and built-in JSON support.

## Features
- Deklaratives Routing (DE) / Declarative routing (EN)
- State-Management (DE) / State management (EN)
- JSON-Serialisierung (DE) / JSON serialization (EN)

## Verwendung / Usage
```bash
cargo run
curl http://localhost:8000/health
curl http://localhost:8000/info
```

## Tech Stack
Rust 2024, Rocket 0.5, serde
