# 2D-Metroidvania Template

## Beschreibung
Ein 2D-Metroidvania mit vernetzter Weltkarte, Power-Ups, verschlossenen Türen und Bossgegnern. Grundlage für Erkundungs- und Actionspiele.

## Description
A 2D Metroidvania with connected world map, power-ups, locked doors, and boss enemies. Foundation for exploration and action games.

## Features
- Nicht-lineare Level-Erkundung / Non-linear level exploration
- Power-Up-System (Doppelsprung, Dash, etc.) / Power-up system (double jump, dash, etc.)
- Verschlossene Türen und Schlüssel / Locked doors and keys
- Boss-Gegner / Boss enemies
- Karten-System / Map system
- Speicherpunkte / Save points

## How to Use
1. Attach `PlayerController` to player with Rigidbody2D
2. Attach `GameManager` for persistent game state
3. Attach `DoorController` to door GameObjects
4. Attach `PowerUp` to collectible power-up GameObjects

## Tech Stack
Unity 2022.3+, C#, 2D Physics, Unity Input System, Tilemaps, Cinemachine

## Extension Ideas
- Fast-Travel-Systeme / Fast travel systems
- Mehrere Waffen / Multiple weapons
- Skill-Baum / Skill tree
- Geheime Bereiche / Secret areas
- Speicher- und Ladesystem / Save and load system
