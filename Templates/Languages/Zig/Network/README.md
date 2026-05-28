# Network

**Deutsch:** Ein asynchroner TCP-Server/Client in Zig. Demonstriert Event-Loop,
non-blocking I/O und Protokoll-Parsing mit std.net.

**English:** An async TCP server/client in Zig. Demonstrates event loop,
non-blocking I/O, and protocol parsing with std.net.

## Features
- TCP-Server mit Verbindungsverwaltung (DE) / TCP server with connection management (EN)
- TCP-Client mit Verbindungswiederherstellung (DE) / TCP client with reconnection (EN)
- Echo-Protokoll und benutzerdefinierte Nachrichten (DE) / Echo protocol and custom messages (EN)
- Fehlerbehandlung mit error unions (DE) / Error handling with error unions (EN)

## Verwendung / Usage
```bash
zig build run -- server 8080
zig build run -- client 127.0.0.1 8080
```

## Tech Stack
Zig 0.13+, std.net, std.os, std.io
