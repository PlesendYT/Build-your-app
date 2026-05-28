# 2D-Platformer (GameMaker GML)

## Beschreibung / Description

**DE:** Ein klassisches 2D-Jump'n'Run mit Spieler, Gegnern und Sammelobjekten. Der Spieler kann laufen, springen und Plattformen erklimmen.

**EN:** A classic 2D platformer with player, enemies, and collectibles. The player can run, jump, and climb platforms.

## Features

- Spielerbewegung mit Beschleunigung / Player movement with acceleration
- Doppelsprung / Double jump
- Gegner-Patrouille / Enemy patrol AI
- Sammelobjekte (Münzen) / Collectibles (coins)
- Kollisionsabfrage mit pixelgenauen Masken / Pixel-perfect collision
- Kamera folgt Spieler / Camera follows player

## How to Use

1. Neues GameMaker-Projekt erstellen
2. Objekte anlegen: obj_player, obj_enemy, obj_coin, obj_ground
3. Sprites für jedes Objekt zuweisen
4. GML-Code in die entsprechenden Events kopieren
5. Raum (Room) erstellen und Objekte platzieren

1. Create a new GameMaker project
2. Create objects: obj_player, obj_enemy, obj_coin, obj_ground
3. Assign sprites to each object
4. Copy GML code into the corresponding events
5. Create a room and place objects

## Tech Stack

- GameMaker LTS 2024 / 2024+
- GML (GameMaker Language)
- Built-in-Variablen: x, y, hspeed, vspeed, gravity
- instance_create_layer()

## Extension Ideas

- Gegnertypen (fliegend, werfend) / Enemy types (flying, throwing)
- Power-Ups (Geschwindigkeit, Unsichtbarkeit) / Power-ups (speed, invisibility)
- Level-Abschnitte / Level sections
- Checkpoints
- Bosskämpfe / Boss fights
