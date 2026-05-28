# Dart RESTAPI

🇩🇪 REST-API mit Shelf, Router, Aqueduct-ähnlicher Architektur und Datenbank.

🇬🇧 REST API with Shelf, Router, Aqueduct-like architecture, and database.

## Features
- CRUD-REST-Endpunkte
- JSON-Serialisierung
- In-Memory-Speicher (austauschbar)
- Validierung
- Fehlerbehandlung

## How to use
```bash
dart run bin/server.dart
# http://localhost:8080/api/users
```

## Tech stack
- Dart 3
- shelf, shelf_router
- JSON

## Extension ideas
- Add PostgreSQL (postgresql-dart)
- Add JWT auth
- Add Swagger UI
- Add Rate Limiting
