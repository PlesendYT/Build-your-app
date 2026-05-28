# CLI-Tool

**Deutsch:** Ein modernes Kommandozeilenwerkzeug in Zig. Nutzt `std.process.ArgIterator`
für Argumentenparsing und `std.heap.GeneralPurposeAllocator` für Speicherverwaltung.

**English:** A modern command-line tool in Zig. Uses `std.process.ArgIterator`
for argument parsing and `std.heap.GeneralPurposeAllocator` for memory management.

## Features
- Allgemeiner Allokator mit Leckprüfung (DE) / General-purpose allocator with leak detection (EN)
- Datei-I/O mit error unions (DE) / File I/O with error unions (EN)
- Comptime-Konfiguration (DE) / Comptime configuration (EN)

## Verwendung / Usage
```bash
zig build run -- process input.txt -o output.txt
zig build run -- status
zig build run -- analyze . --verbose
```

## Tech Stack
Zig 0.13+, std.heap, std.fs, comptime
