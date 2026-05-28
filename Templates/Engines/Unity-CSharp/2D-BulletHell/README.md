# 2D-BulletHell Template

## Beschreibung
Ein Bullet-Hell-Shooter mit dichten Projektilmustern, Ausweichmechaniken und Score-Chaining. Der Spieler steuert ein Schiff und weicht komplexen Geschosswellen aus.

## Description
A bullet-hell shooter with dense projectile patterns, dodge mechanics, and score chaining. The player controls a ship and avoids complex bullet waves.

## Features
- Dichte Projektilmuster mit mathematischen Kurven / Dense projectile patterns with mathematical curves
- Spielerausweichen mit Boost / Player dodge with boost
- Mehrere Schwierigkeitsstufen / Multiple difficulty tiers
- Punktesystem mit Chain-Multiplikator / Score system with chain multiplier

## How to Use
1. Import into Unity 2022.3+ LTS
2. Attach `PlayerShip` to player GameObject with Collider2D
3. Attach `BulletPattern` to enemy spawners
4. Attach `GameManager` to a persistent GameObject
5. Attach `Projectile` to bullet prefabs

## Tech Stack
Unity 2022.3+, C#, 2D Physics, Unity Input System, Object Pooling

## Extension Ideas
- Bosskampf-Phasen / Boss fight phases
- Power-Up-Drops / Power-up drops
- Replay-System / Replay system
- Online-Rangliste / Online leaderboard
