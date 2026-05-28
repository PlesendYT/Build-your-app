# Go Automation

🇩🇪 Automatisierungs-Tool in Go: Datei-Watcher, Scheduler und Task-Runner.

🇬🇧 Automation tool in Go: file watcher, scheduler, and task runner.

## Features
- Dateisystem-Überwachung (fsnotify)
- Cron-ähnlicher Scheduler
- Shell-Befehle ausführen
- Logging mit Rotation
- Konfiguration via YAML

## How to use
```bash
go run . watch ./verzeichnis
go run . schedule "*/5 * * * *" echo "Hallo"
```

## Tech stack
- Go 1.22
- fsnotify
- robfig/cron
- Standard Library

## Extension ideas
- Add Webhook-Benachrichtigungen
- Add E-Mail-Benachrichtigungen
- Add Plugin-System
- Add Dashboard
