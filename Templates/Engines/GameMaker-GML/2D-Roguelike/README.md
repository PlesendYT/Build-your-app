# 2D-Roguelike (GameMaker GML)

## Beschreibung / Description

**DE:** Ein Roguelike mit prozeduraler Dungeon-Generierung, rundenbasierter Bewegung und Permadeath.

**EN:** A roguelike with procedural dungeon generation, turn-based movement, and permadeath.

## Features

- Prozedurale Dungeon-Generierung (BSP/Raum-Gang) / Procedural dungeon generation (BSP/room-corridor)
- Rundenbasierte Bewegung / Turn-based movement
- Gegner mit KI / Enemy AI
- Items und Waffen / Items and weapons
- Permadeath
- Erkundung (Fog of War)

## How to Use

1. Objekte: obj_player, obj_enemy, obj_wall, obj_floor, obj_item, obj_stairs
2. Dungeon-Generator in einem Controller-Objekt
3. Level-System über ds_grid oder 2D-Array

## Tech Stack

- GameMaker LTS 2024 / GML
- 2D-Arrays für Tilemaps
- ds_grid / Arrays für Leveldaten
- mp_grid für Pfadfindung

## Extension Ideas

- Charakterklassen / Character classes
- Zauber und Fallen / Spells and traps
- Händler / Merchants
- Bosse pro Ebene / Bosses per floor
