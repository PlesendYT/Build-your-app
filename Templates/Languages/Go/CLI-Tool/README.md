# Go CLI-Tool

🇩🇪 Kommandozeilen-Tool in Go mit Cobra, Farbausgabe und Subcommands.

🇬🇧 Command-line tool in Go with Cobra, colored output, and subcommands.

## Features
- Subcommand-Architektur mit Cobra
- Farbige Konsolenausgabe
- Umfassende Fehlerbehandlung
- Konfiguration via Flags

## How to use
```bash
go run . greet --name Welt
go run . calc add 5 3
go build -o cli && ./cli help
```

## Tech stack
- Go 1.22
- Cobra CLI
- Standard Library

## Extension ideas
- Add Viper config
- Add shell completion
- Add HTTP client commands
- Add JSON/YAML output
