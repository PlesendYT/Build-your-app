# 3D-FPS Template

## Beschreibung
Ein Ego-Shooter mit First-Person-Kamera, Waffen, Gegnern und Health-System. Grundlage für Action-Shooter.

## Description
A first-person shooter with first-person camera, weapons, enemies, and health system. Foundation for action shooters.

## Features
- First-Person-Steuerung / First-person controls
- Waffen-System (Schusswaffen, Nachladen) / Weapon system (firearms, reloading)
- Gegner mit KI / Enemies with AI
- Zielvorrichtung (Fadenkreuz) / Aiming reticle
- Munitions-Management / Ammo management
- Schadens-Effekte / Damage effects

## How to Use
1. Attach `PlayerController` to player with CharacterController
2. Attach `Weapon` to weapon GameObject
3. Attach `EnemyController` to enemy prefabs
4. Set up camera as child of player head

## Tech Stack
Unity 2022.3+, C#, 3D Physics, Unity Input System, Cinemachine, ProBuilder

## Extension Ideas
- Mehrere Waffentypen / Multiple weapon types
- Granaten / Grenades
- Mehrspieler / Multiplayer
- Kampagne mit Levels / Campaign with levels
