# CLI-Tool

**Deutsch:** Ein modernes Kommandozeilenwerkzeug in Nim. Nutzt `parseopt` für
Kommandozeilenargumente und `os` für Dateioperationen.

**English:** A modern command-line tool in Nim. Uses `parseopt` for command-line
argument parsing and `os` for file operations.

## Features
- Optionen-Parsing mit parseopt (DE) / Option parsing with parseopt (EN)
- Asynchrone Dateiverarbeitung (DE) / Async file processing (EN)
- Strukturierte Fehlerbehandlung mit Result (DE) / Structured error handling with Result (EN)

## Verwendung / Usage
```bash
nimble run process input.txt -o output.txt
nimble run status
nimble run analyze . --verbose
```

## Tech Stack
Nim 2.0+, parseopt, os, strutils, json
