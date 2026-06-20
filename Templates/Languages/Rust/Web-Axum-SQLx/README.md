# Axum + SQLx Template

> Async-Webservice mit Axum und SQLx (SQLite). / Async web service with Axum and SQLx (SQLite).

## Features
- CRUD-Endpunkte / CRUD endpoints
- SQLx mit Compile-Time-Queries (oder Runtime-Queries als Fallback) / SQLx with compile-time or runtime queries
- JSON-Serialisierung mit serde / JSON serialization with serde
- SQLite-Datei oder In-Memory-DB / SQLite file or in-memory DB

## How to Use
```bash
cd Web-Axum-SQLx
cargo run
# curl -X POST localhost:8080/items -d '{"name":"first"}' -H 'content-type: application/json'
```

## Tech Stack
- Axum 0.7
- SQLx 0.7 (sqlite)
- Tokio

## Extension Ideas
- OpenAPI via utoipa / OpenAPI via utoipa
- Tower-Middleware für Auth / Tower middleware for auth
- Postgres statt SQLite / Postgres instead of SQLite