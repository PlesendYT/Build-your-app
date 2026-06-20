# Clap CLI Template

> Rust-CLI mit Clap v4. / Rust CLI with Clap v4.

## Features
- Sub-Commands per derive-Macro / Sub-commands via derive macro
- Argumente und Optionen / Arguments and options
- `--help` und Version werden automatisch generiert / `--help` and version generated automatically

## How to Use
```bash
cd CLI-Clap
cargo run -- greet --name World
cargo run -- add 1 2 3
```

## Tech Stack
- Rust 1.74+
- clap 4

## Extension Ideas
- `anyhow` und `thiserror` für Fehler / `anyhow` and `thiserror` for errors
- `serde` für Konfig-Dateien / `serde` for config files
- `tracing` für strukturiertes Logging / `tracing` for structured logging