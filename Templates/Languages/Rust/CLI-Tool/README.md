# CLI-Tool

**Deutsch:** Ein modernes Kommandozeilenwerkzeug in Rust. Nutzt `clap` für
Argumentenparsing, `serde` für Konfigurationsdateien und `anyhow` für
Fehlerbehandlung.

**English:** A modern command-line tool in Rust. Uses `clap` for argument parsing,
`serde` for configuration files, and `anyhow` for error handling.

## Features
- Subcommand-basierte CLI (DE) / Subcommand-based CLI (EN)
- JSON-Konfigurationsdateien (DE) / JSON configuration files (EN)
- Ausführliche Fehlerbehandlung (DE) / Detailed error handling (EN)

## Verwendung / Usage
```bash
cargo run -- status
cargo run -- process input.txt -o output.txt
cargo run -- -c config.json process input.txt
```

## Tech Stack
Rust 2024, clap 4, anyhow, serde, chrono
