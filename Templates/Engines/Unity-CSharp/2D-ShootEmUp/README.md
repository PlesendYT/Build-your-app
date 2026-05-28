# 2D-ShootEmUp Template

## Beschreibung
Ein vertikales Bullet-Hell-Shoot'em'up mit Spieler-Schiff, Gegner-Wellen, Power-Ups und Endgegnern. Grundlage für Arcade-Shooter.

## Description
A vertical bullet-hell shoot'em'up with player ship, enemy waves, power-ups, and boss enemies. Foundation for arcade shooters.

## Features
- Spieler-Schiff mit automatischem Feuer / Player ship with auto-fire
- Mehrere Gegnertypen / Multiple enemy types
- Power-Ups (Waffen-Upgrades, Schilde) / Power-ups (weapon upgrades, shields)
- Bosskämpfe / Boss fights
- Punkte- und Highscore-System / Score and highscore system
- Partikeleffekte / Particle effects

## How to Use
1. Attach `PlayerShip` to player with Rigidbody2D
2. Attach `Bullet` to bullet prefab
3. Attach `EnemySpawner` to manage wave spawning
4. Attach `GameManager` for game state

## Tech Stack
Unity 2022.3+, C#, 2D Physics, Unity Input System, Particle System

## Extension Ideas
- Seiten-Scrolling (horizontal) / Side-scrolling (horizontal)
- Mehrspieler / Multiplayer
- Verschiedene Schiffstypen / Different ship types
- Online-Bestenliste / Online leaderboard
