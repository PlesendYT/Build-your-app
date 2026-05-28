# CLI-Tool

**Deutsch:** Ein modernes Kommandozeilenwerkzeug in Swift. Nutzt `ArgumentParser`
für Kommandozeilenargumente und `async/await` für asynchrone Operationen.

**English:** A modern command-line tool in Swift. Uses `ArgumentParser` for
command-line arguments and `async/await` for asynchronous operations.

## Features
- Subcommand-basierte CLI mit ArgumentParser (DE) / Subcommand-based CLI with ArgumentParser (EN)
- Asynchrone Dateiverarbeitung (DE) / Async file processing (EN)
- Strukturierte Fehlerbehandlung (DE) / Structured error handling (EN)

## Verwendung / Usage
```bash
swift run cli-tool process input.txt -o output.txt
swift run cli-tool status
swift run cli-tool analyze . --verbose
```

## Tech Stack
Swift 5.9+, ArgumentParser, Foundation, async/await
