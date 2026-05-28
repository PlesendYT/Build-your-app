# Lua Automation

🇩🇪 Automatisierungs-Skripte in Lua: Datei-Watcher, Scheduler und Task-Ausführung.

🇬🇧 Automation scripts in Lua: file watcher, scheduler, and task execution.

## Features
- Dateisystem-Überwachung
- Zeitgesteuerte Aufgaben
- Shell-Befehle ausführen
- Logging
- Konfiguration per Lua-Tabelle

## How to use
```bash
lua src/automation.lua watch ./verzeichnis
lua src/automation.lua schedule "*/5 * * * *" echo "Hallo"
```

## Tech stack
- Lua 5.4
- lua-fs (Dateisystem)
- Standard Libraries

## Extension ideas
- Add Webhook-Benachrichtigungen
- Add E-Mail-Versand
- Add Datenbank-Logging
- Add SSH-Remote-Befehle
