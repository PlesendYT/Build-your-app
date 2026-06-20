# Microservices Starter (Node) Template

> Zwei kleine Express-Services hinter einem einfachen Reverse-Proxy. / Two small Express services behind a simple reverse proxy.

## Features
- Zwei unabhängige Services / Two independent services
- docker-compose mit allen Teilen / docker-compose with everything
- Healthcheck pro Service / Healthcheck per service

## How to Use
```bash
cd Microservices-Starter
docker compose up --build
curl localhost:8080/api-a/health
curl localhost:8080/api-b/health
```

## Tech Stack
- Node.js, Express
- nginx, Docker

## Extension Ideas
- Service-Discovery mit Consul / Service discovery with Consul
- Tracing mit OpenTelemetry / Tracing with OpenTelemetry
- JWT zwischen Services / JWT between services