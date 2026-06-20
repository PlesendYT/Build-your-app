# Gin + JWT Template

> REST-API mit Gin und JWT-Auth. / REST API with Gin and JWT auth.

## Features
- Login und geschützte Route / Login and protected route
- HS256 JWT-Signierung / HS256 JWT signing
- Middleware-basierte Authentifizierung / Middleware-based auth
- In-Memory-Benutzer für Demo / In-memory user for demo

## How to Use
```bash
cd Web-Gin-JWT
go mod tidy
go run .
# POST /login { "user": "alice", "pass": "wonderland" }
# GET /api/me with `Authorization: Bearer <token>`
```

## Tech Stack
- Gin
- golang-jwt/jwt

## Extension Ideas
- GORM + PostgreSQL / GORM + PostgreSQL
- Refresh-Token-Pattern / Refresh-token pattern
- Swagger via swaggo / Swagger via swaggo