# 3D-Racing – Rennspiel Template (UE5)

**Deutsch:** Ein Rennspiel-Grundgerüst mit Fahrzeugphysik, Rundenzähler, Positionstracking und KI-Gegnern.

**English:** A racing game skeleton with vehicle physics, lap counter, position tracking, and AI opponents.

## Features

- Fahrzeugpawn mit Physics-basierter Bewegung | Vehicle pawn with physics-based movement
- Rundensystem mit Start/Ziel-Erkennung | Lap system with start/finish detection
- Platzierungs-Tracking für alle Fahrer | Placement tracking for all racers
- KI-Fahrer mit Spline-basierter Streckenführung | AI drivers with spline-based pathfinding
- Geschwindigkeitsanzeige und Boost-Mechanik | Speed display and boost mechanic

## How to Use

1. Add to `Source/Racing/` folder.
2. Set `ARacingGameMode` as default.
3. Place checkpoints as child actors of BP_Checkpoint.
4. Assign vehicle mesh in Blueprint.

## Tech Stack

- UE5.4+, C++17, ChaosPhysics, EnhancedInput

## Extension Ideas

- Mehrere Strecken und Fahrzeugklassen | Multiple tracks and vehicle classes
- Drift- und Nitro-Mechaniken | Drift and nitro mechanics
- Replay-System und Bestenlisten | Replay system and leaderboards
