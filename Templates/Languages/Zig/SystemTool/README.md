# SystemTool

**Deutsch:** Ein Systemüberwachungswerkzeug in Zig. Zeigt Prozessinformationen,
Speichernutzung, CPU-Auslastung und Festplattenstatistiken an.

**English:** A system monitoring tool in Zig. Displays process information,
memory usage, CPU load, and disk statistics.

## Features
- Prozessliste mit PID und Namen (DE) / Process list with PID and name (EN)
- Speichernutzung (DE) / Memory usage (EN)
- CPU-Informationen (DE) / CPU information (EN)
- Plattformunabhängig (Linux/macOS) (DE) / Cross-platform (Linux/macOS) (EN)

## Verwendung / Usage
```bash
zig build run
```

## Tech Stack
Zig 0.13+, std.os, std.fs, std.heap
