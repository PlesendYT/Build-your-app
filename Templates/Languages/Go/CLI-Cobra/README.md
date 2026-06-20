# Cobra Template

> Strukturierte Go-CLI mit Cobra. / Structured Go CLI with Cobra.

## Features
- Sub-Commands mit Flags / Sub-commands with flags
- Cobra-Generator kompatibel / Compatible with Cobra generator
- Eingebauter Help-Generator / Built-in help generator

## How to Use
```bash
cd CLI-Cobra
go mod tidy
go run . greet --name World
go run . add 1 2 3
```

## Tech Stack
- Go 1.22+
- Cobra

## Extension Ideas
- Viper für Konfiguration / Viper for configuration
- Tests mit testify / Tests with testify
- Releaser via GoReleaser / Release via GoReleaser