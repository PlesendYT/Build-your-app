# Embedded

**Deutsch:** Ein eingebettetes System-Projekt in Zig für Mikrocontroller (ARM
Cortex-M). Nutzt `std.Target` für Cross-Kompilierung und bitweises I/O.

**English:** An embedded system project in Zig for microcontrollers (ARM
Cortex-M). Uses `std.Target` for cross-compilation and bitwise I/O.

## Features
- memory-mapped I/O Register (DE) / Memory-mapped I/O registers (EN)
- Externer Interrupt-Controller (DE) / External interrupt controller (EN)
- UART-Kommunikation (DE) / UART communication (EN)
- Comptime-Konfiguration für Chip-Typ (DE) / Comptime config for chip type (EN)

## Verwendung / Usage
```bash
zig build run   # Simuliert embedded Logik auf Host
zig build       # Cross-kompiliert für ARM Cortex-M
```

## Tech Stack
Zig 0.13+, std.Target, inline assembly, comptime
