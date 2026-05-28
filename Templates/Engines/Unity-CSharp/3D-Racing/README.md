# 3D-Racing Template

## Beschreibung
Ein Rennspiel mit Fahrzeug-Physik, Runden-Zählung, Gegner-Fahrzeugen und mehreren Strecken.

## Description
A racing game with vehicle physics, lap counting, opponent vehicles, and multiple tracks.

## Features
- Fahrzeug-Physik (Beschleunigung, Lenkung, Bremsen) / Vehicle physics (acceleration, steering, braking)
- Runden-Zählsystem / Lap counting system
- KI-Gegner / AI opponents
- Geschwindigkeits- und Positions-Anzeige / Speed and position display
- Start-/Ziellinie / Start/finish line
- Drift-Mechanik / Drift mechanics

## How to Use
1. Attach `CarController` to vehicle with WheelColliders
2. Attach `TrackManager` for track waypoints
3. Attach `LapCounter` for lap tracking
4. Attach `GameManager` for race state

## Tech Stack
Unity 2022.3+, C#, 3D Physics, Unity Input System, WheelColliders, Cinemachine

## Extension Ideas
- Fahrzeug-Upgrades / Vehicle upgrades
- Mehrere Strecken / Multiple tracks
- Split-Screen-Multiplayer / Split-screen multiplayer
- Online-Multiplayer / Online multiplayer
- Wetter-Effekte / Weather effects
