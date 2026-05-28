# WebServer-Axum

**Deutsch:** REST-API-Server mit Axum 0.7. Modernes, modulares Routing mit
Tower-Middleware und integrierter CORS-Unterstützung.

**English:** REST API server with Axum 0.7. Modern, modular routing with
Tower middleware and built-in CORS support.

## Features
- Type-sicherer State (DE) / Type-safe state (EN)
- CORS-Unterstützung (DE) / CORS support (EN)
- Tracing/Logging (DE/EN)

## Verwendung / Usage
```bash
cargo run
curl http://localhost:3000/health
curl http://localhost:3000/info
```

## Tech Stack
Rust 2024, Axum 0.7, tokio, serde, tower-http, uuid
