# 2D-Platformer Template

## Beschreibung
Ein klassisches 2D-Platformer-Spiel mit seitlichem Scrollen, Sprungmechaniken, sammelbaren Gegenständen und Gegnerinteraktionen. Ideal für Jump'n'Run-Spiele.

## Description
A classic 2D platformer game with side-scrolling, jumping mechanics, collectible items, and enemy interactions. Ideal for jump'n'run games.

## Features
- Spielerbewegung mit Beschleunigung und Sprung / Player movement with acceleration and jump
- Gegner mit KI-Patrouille / Enemies with AI patrol
- Sammelbare Münzen und Power-Ups / Collectible coins and power-ups
- Kamerafolge / Camera following
- Lebens- und Punktesystem / Health and score system

## How to Use
1. Import into Unity 2022.3+ LTS
2. Attach `PlayerController` to player GameObject with Rigidbody2D and Collider2D
3. Attach `EnemyController` to enemy GameObjects
4. Attach `GameManager` to a persistent GameObject
5. Attach `Collectible` to coin/power-up GameObjects with trigger Collider2D

## Tech Stack
Unity 2022.3+, C#, 2D Physics, Unity Input System, Tilemaps

## Extension Ideas
- Mehrere Level mit Levelübergängen / Multiple levels with transitions
- Bosskämpfe / Boss fights
- Speicher- und Ladesystem / Save and load system
- Power-Up-Shop / Power-up shop
- Partikeleffekte / Particle effects
