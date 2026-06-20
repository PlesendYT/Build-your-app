# Docker Compose Stack Template

> Beispiel-Stack mit Webapp, Datenbank und Cache. / Sample stack with web app, database, and cache.

## Features
- docker-compose.yml mit Services / docker-compose.yml with services
- Beispiel-Webapp mit Nginx / Sample web app with Nginx
- Healthchecks und Restart-Policies / Health checks and restart policies

## How to Use
```bash
cd Docker-Compose-Stack
docker compose up -d
docker compose logs -f web
```

## Tech Stack
- Docker / Docker Compose
- Node.js, Postgres, Redis

## Extension Ideas
- Traefik als Reverse-Proxy / Traefik as reverse proxy
- Prometheus + Grafana / Prometheus + Grafana
- Volumes und Backups / Volumes and backups