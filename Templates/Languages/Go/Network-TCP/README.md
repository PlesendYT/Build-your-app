# Go Network-TCP

🇩🇪 TCP-Server und -Client in Go mit Goroutines und Protokoll-Parsing.

🇬🇧 TCP server and client in Go with Goroutines and protocol parsing.

## Features
- TCP-Server mit Connection-Handling
- Concurrent Clients via Goroutines
- Einfaches Text-Protokoll
- Timeout-Behandlung
- Graceful Shutdown

## How to use
```bash
# Terminal 1: Server
go run . server

# Terminal 2: Client
go run . client --addr localhost:9000
```

## Tech stack
- Go 1.22
- Standard Library net

## Extension ideas
- Add TLS
- Add protobuf encoding
- Add rate limiting
- Add reconnection logic
