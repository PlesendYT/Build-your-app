# Lua WebServer

🇩🇪 HTTP-Server in Lua mit Socket-API und einfachem Routing.

🇬🇧 HTTP server in Lua with socket API and simple routing.

## Features
- TCP-Server mit LuaSocket
- Request-Routing
- JSON-Antworten
- Statische Dateien ausliefern
- Middleware-Support

## How to use
```bash
lua src/server.lua
# http://localhost:8080
```

## Tech stack
- Lua 5.4
- LuaSocket
- LuaFileSystem

## Extension ideas
- Add Template-Engine
- Add WebSocket
- Add Datenbank (lsqlite3)
- Add SSL (luasec)
