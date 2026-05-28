# 2D-Platformer (Solar2D Lua)

## Beschreibung / Description

**DE:** Ein Solar2D-Plattformer mit Spieler-Steuerung, Gegnern und Münzen.

**EN:** A Solar2D platformer with player controls, enemies, and coins.

## Features

- Bewegung mit Beschleunigung / Movement with acceleration
- Sprung und Doppelsprung / Jump and double jump
- Gegner-Patrouille / Enemy patrol
- Münzen sammeln / Coin collecting
- Kollision mit Plattformen / Platform collision

## How to Use

1. `main.lua` als Einstiegspunkt
2. Grafiken in `display.newImageRect()` referenzieren
3. `Runtime:addEventListener` für Input

## Tech Stack

- Solar2D 2024+ / Corona SDK API
- `display.newImageRect`, `physics.addBody`, `transition.to`
- `Runtime:addEventListener`, `composer.gotoScene`

## Extension Ideas

- Power-Ups
- Level-Auswahl
- Gleiten
- Rücksetzpunkte
