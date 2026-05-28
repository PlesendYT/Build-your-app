# Defold Lua Templates

Defold-Vorlagen für 2D-Spiele mit Lua-Skripten. Kompatibel mit Defold Engine 1.6+.

Defold templates for 2D games using Lua scripts. Compatible with Defold Engine 1.6+.

## Vorlagen / Templates

| Genre | Beschreibung / Description |
|-------|--------------------------|
| 2D-Platformer | Klassisches Jump'n'Run / Classic platformer |
| 2D-TopDown | Top-Down-Abenteuer / Top-down adventure |
| 2D-Puzzle | Rätselspiel / Puzzle game |
| 2D-Roguelike | Roguelike mit prozeduralen Levels / Roguelike with procedural levels |
| 2D-TowerDefense | Turmverteidigung / Tower defense |
| 2D-CardGame | Kartenspiel / Card game |

## Verwendung / Usage

Jede Vorlage enthält `.script`-Dateien für Defold.
Skripte an Game Objects anhängen und im Collection-Editor konfigurieren.

Each template contains `.script` files for Defold.
Attach scripts to game objects and configure in the Collection editor.

## Konventionen / Conventions

- `go.property()` für Eigenschaften
- `msg.post()` für Nachrichten
- `vmath.vector3()` für Vektoren
- `init()`, `update()`, `on_message()` Lebenszyklus
- `go.set()` / `go.get()` für Eigenschaftszugriff
- `factory.create()` für Objekterstellung
