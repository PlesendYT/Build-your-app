# Go SystemTool

🇩🇪 System-Utility in Go: Datei-Operationen, Prozess-Info und System-Monitoring.

🇬🇧 System utility in Go: file operations, process info, and system monitoring.

## Features
- Datei- und Verzeichnis-Operationen
- Prozess-Informationen
- System-Statistiken (CPU, RAM, Disk)
- JSON-Ausgabe
- Logging

## How to use
```bash
go run . info
go run . files ./verzeichnis
go run . ps
```

## Tech stack
- Go 1.22
- Standard Library
- gopsutil (optional)

## Extension ideas
- Add Netzwerk-Monitoring
- Add Alerting
- Add Web-Dashboard
- Add Plugin-System
