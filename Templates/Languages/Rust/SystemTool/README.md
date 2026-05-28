# SystemTool

**Deutsch:** Systemüberwachungswerkzeug. Zeigt CPU-, Speicher- und
Festplatteninformationen an. Unterstützt JSON-Ausgabe.

**English:** System monitoring tool. Displays CPU, memory, and disk
information. Supports JSON output.

## Features
- CPU-Auslastung (DE) / CPU usage (EN)
- Speicherinfo (DE) / Memory info (EN)
- Festplatteninfo (DE) / Disk info (EN)
- JSON-Ausgabe (DE) / JSON output (EN)

## Verwendung / Usage
```bash
cargo run
cargo run -- cpu
cargo run -- memory
cargo run -- disk
```

## Tech Stack
Rust 2024, sysinfo, clap, serde, chrono
