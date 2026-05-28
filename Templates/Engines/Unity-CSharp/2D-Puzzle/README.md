# 2D-Puzzle Template

## Beschreibung
Ein tile-basiertes Puzzlespiel mit verschiebbaren Blöcken, Druckplatten und Zielbereichen. Ideal für Denk- und Knobelspiele.

## Description
A tile-based puzzle game with movable blocks, pressure plates, and target areas. Ideal for brain teasers and puzzle games.

## Features
- Grid-basiertes Bewegungsystem / Grid-based movement system
- Verschiedene Tile-Typen (Wand, Block, Ziel) / Various tile types (wall, block, target)
- Druckplatten und Schalter / Pressure plates and switches
- Level-Management / Level management
- Zug-Zähler / Move counter

## How to Use
1. Attach `GridManager` to an empty GameObject
2. Configure grid dimensions and tile prefabs in GridManager
3. Attach `PuzzleManager` to manage game state
4. Create tile prefabs with appropriate colliders

## Tech Stack
Unity 2022.3+, C#, 2D Physics, Tilemaps, Unity Input System

## Extension Ideas
- Timer-Modus / Timer mode
- Level-Editor / Level editor
- Verschiedene Tile-Eigenschaften (Eis, Teleporter) / Various tile properties (ice, teleporter)
- Bewegliche Gegner / Moving enemies
