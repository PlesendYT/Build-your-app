# MongoDB + Express Starter

> Express-Server mit MongoDB-Anbindung über Mongoose. / Express server with MongoDB via Mongoose.

## Features
- Healthcheck-Route / Healthcheck route
- Mongoose-Modell mit Validierung / Mongoose model with validation
- docker-compose mit Mongo und Web / docker-compose with Mongo and web

## How to Use
```bash
cd MongoDB-Express-Starter
docker compose up -d
curl localhost:3000/health
```

## Tech Stack
- Express, Mongoose
- MongoDB 7

## Extension Ideas
- Aggregation-Pipelines / Aggregation pipelines
- Authentication mit Passport / Authentication with Passport
- OpenAPI via swagger-jsdoc / OpenAPI via swagger-jsdoc