# Go Network-WebSocket

🇩🇪 WebSocket-Server/-Client mit Gorilla WebSocket und JSON-Nachrichten.

🇬🇧 WebSocket server/client with Gorilla WebSocket and JSON messages.

## Features
- WebSocket-Upgrade-Handler
- Broadcast an alle Clients
- JSON-Nachrichtenformat
- Ping/Pong Keepalive
- Concurrent Clients

## How to use
```bash
go run .
# http://localhost:8080 für WebSocket-Verbindungen
```

## Tech stack
- Go 1.22
- Gorilla WebSocket
- Standard Library net/http

## Extension ideas
- Add rooms/channels
- Add authentication
- Add message history
- Add binary messages
