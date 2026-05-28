# 2D-TowerDefense Template

## Beschreibung
Ein Tower-Defense-Spiel mit Wellen-basierten Gegnern, platzierbaren Türmen und Upgrade-System. Grundlage für Strategie-Tower-Defense-Spiele.

## Description
A tower defense game with wave-based enemies, placeable towers, and upgrade system. Foundation for strategy tower defense games.

## Features
- Wellen-basierte Gegner-Spawns / Wave-based enemy spawning
- Mehrere Turm-Typen / Multiple tower types
- Turm-Upgrade-System / Tower upgrade system
- Wegpunkt-Pfadsystem / Waypoint path system
- Ressourcen-Management / Resource management
- Lebens-Anzeige / Lives display

## How to Use
1. Attach `WaveSpawner` to manage enemy waves
2. Attach `Tower` to tower prefabs
3. Attach `Enemy` to enemy prefabs with waypoint following
4. Attach `GameManager` for game state and UI

## Tech Stack
Unity 2022.3+, C#, 2D Physics, Unity Input System, UI Toolkit

## Extension Ideas
- Element-Türme (Feuer, Eis) / Element towers (fire, ice)
- Spezialfähigkeiten / Special abilities
- Multiplayer / Multiplayer
- Karten-Editor / Map editor
