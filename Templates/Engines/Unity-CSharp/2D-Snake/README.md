# 2D-Snake Template

## Beschreibung
Das klassische Snake-Spiel mit Rasterbewegung, Wachstumsmechanik und Kollisionserkennung. Der Spieler steuert eine Schlange, die Futter frisst und wächst.

## Description
The classic snake game with grid movement, growth mechanics, and collision detection. The player controls a snake that eats food and grows.

## Features
- Raster-basierte Bewegung / Grid-based movement
- Schlängelnder Körper mit Verfolgung / Slithering body with following
- Futter-Spawning / Food spawning
- Kollision mit Wänden und eigenem Körper / Collision with walls and self

## How to Use
1. Import into Unity 2022.3+ LTS
2. Attach `SnakeController` to the snake head
3. Attach `FoodSpawner` to a spawn manager
4. Attach `SnakeSegment` to body part prefabs
5. Attach `GameManager` for scoring

## Tech Stack
Unity 2022.3+, C#, 2D, Unity Input System

## Extension Ideas
- Hindernis-Level / Obstacle levels
- Power-Up-Futter / Power-up food
- Multiplayer / Multiplayer
- Highscore-System / Highscore system
