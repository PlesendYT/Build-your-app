# Spring Boot REST Template

> Spring-Boot-3-Webservice mit Maven. / Spring Boot 3 web service with Maven.

## Features
- GET/POST/DELETE für /items / GET/POST/DELETE for /items
- In-Memory-Liste für Demo / In-memory list for demo
- Maven-Wrapper inklusive / Maven wrapper included

## How to Use
```bash
cd SpringBoot-REST
mvn spring-boot:run
curl -X POST localhost:8080/items -H "content-type: application/json" -d "{\"name\":\"first\"}"
```

## Tech Stack
- Java 17+
- Spring Boot 3

## Extension Ideas
- Spring Data JPA + H2 / Spring Data JPA + H2
- Bean Validation / Bean Validation
- OpenAPI via springdoc / OpenAPI via springdoc