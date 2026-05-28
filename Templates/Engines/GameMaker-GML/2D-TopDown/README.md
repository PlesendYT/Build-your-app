# 2D-TopDown (GameMaker GML)

## Beschreibung / Description

**DE:** Ein Top-Down-Abenteuer mit Bewegung in alle vier Richtungen, Gegnern und einem einfachen Inventarsystem.

**EN:** A top-down adventure with 4-directional movement, enemies, and a simple inventory system.

## Features

- Bewegung in 8 Richtungen / 8-directional movement
- Kollision mit festeren Objekten / Solid object collision
- Gegner mit Verfolgungs-KI / Enemy chase AI
- Inventarsystem / Inventory system
- Interaktion mit NPCs / NPC interaction

## How to Use

1. Neues Projekt, Objekte: obj_player, obj_enemy, obj_wall, obj_item, obj_npc
2. Spieler-Sprite zuweisen
3. Code in Create/Step/Draw Events kopieren

## Tech Stack

- GameMaker LTS 2024 / GML
- lengthdir_x/lengthdir_y für diagonale Bewegung
- place_meeting() / instance_place() für Kollisionen

## Extension Ideas

- Fernkampf / Ranged combat
- Zufallsbegegnungen / Random encounters
- Minimap
- Quest-System / Quest system
