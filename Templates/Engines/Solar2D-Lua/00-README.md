# Solar2D Lua Templates

Solar2D-Vorlagen für 2D-Spiele mit Lua-Skripten (ehemals Corona SDK). Kompatibel mit Solar2D 2024+.

Solar2D templates for 2D games using Lua scripts (formerly Corona SDK). Compatible with Solar2D 2024+.

## Vorlagen / Templates

| Genre | Beschreibung / Description |
|-------|--------------------------|
| 2D-Platformer | Klassisches Jump'n'Run / Classic platformer |
| 2D-TopDown | Top-Down-Abenteuer / Top-down adventure |
| 2D-Puzzle | Rätselspiel / Puzzle game |
| 2D-EndlessRunner | Endlosläufer / Endless runner |
| 2D-CardGame | Kartenspiel / Card game |
| 2D-ShootEmUp | Schießspiel / Shooting game |

## Verwendung / Usage

Jede Vorlage enthält Lua-Quellcodedateien für Solar2D.
`main.lua` als Einstiegspunkt, weitere Module mit `require()` einbinden.

Each template contains Lua source code files for Solar2D.
`main.lua` as entry point, additional modules via `require()`.

## Konventionen / Conventions

- `display.newImageRect()` für Grafiken
- `Runtime:addEventListener()` für Events
- `transition.to()` für Animationen
- `physics.addBody()` für Physik
- `audio.play()` für Sound
- `composer.gotoScene()` für Szenenwechsel
- Objektorientierung mit Modulen (Modul-Pattern)
