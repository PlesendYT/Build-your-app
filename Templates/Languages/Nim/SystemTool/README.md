# SystemTool

**Deutsch:** Ein Systemüberwachungswerkzeug in Nim. Zeigt CPU, Speicher,
Prozesse und Netzwerkinformationen an.

**English:** A system monitoring tool in Nim. Displays CPU, memory, process,
and network information.

## Features
- Prozessliste (DE) / Process list (EN)
- Speicher- und CPU-Info (DE) / Memory and CPU info (EN)
- Netzwerkstatistiken (DE) / Network statistics (EN)
- Export als JSON (DE) / JSON export (EN)

## Verwendung / Usage
```bash
nimble run
nimble run -- --json
nimble run -- cpu
```

## Tech Stack
Nim 2.0+, os, strutils, json, times
