# 3D-FPS – Ego-Shooter Template (UE5)

**Deutsch:** Ein komplettes Ego-Shooter-Grundgerüst für Unreal Engine 5. Enthält Waffensystem, Gesundheits- und Munitionsverwaltung, schlüssellose Treffererkennung sowie ein erweitertes EnhancedInput-Setup für flüssige Steuerung.

**English:** A complete first-person shooter skeleton for Unreal Engine 5. Includes weapon system, health and ammo management, hit-scan detection, and an EnhancedInput setup for smooth controls.

## Features

- **DE:** Waffensystem mit Feuerrate, Nachladen und Schaden | Weapon system with fire rate, reload, and damage
- **DE:** Gesundheits- und Rüstungssystem mit Schaden-Nachberechnung | Health and armor system with damage calculation
- **DE:** EnhancedInput-basierte Steuerung (Bewegen, Zielen, Schießen, Sprinten) | EnhancedInput-based controls (move, aim, shoot, sprint)
- **DE:** Projektil-Ballistik-Option (Blueprint-einstellbar) | Projectile ballistic option (Blueprint-configurable)
- **DE:** Munitions- und Magazinverwaltung | Ammo and magazine management

## How to Use

1. Add the files to your project's `Source/` folder under a module like `FPS`.
2. Generate project files and compile.
3. Set `AFPSGameMode` as Default GameMode.
4. Assign your EnhancedInput actions in the Character blueprint.
5. Create a weapon Blueprint child of `AFPSWeapon`.

## Tech Stack

- **Engine:** Unreal Engine 5.4+
- **Language:** C++17
- **Input:** EnhancedInput / UInputMappingContext
- **Replication:** Actor replication for network games

## Extension Ideas

- **DE:** Unterschiedliche Waffentypen (Pistole, MG, Schrotflinte) | Different weapon types (pistol, SMG, shotgun)
- **DE:** Granaten und Wurfwaffen | Grenades and throwables
- **DE:** Mehrspieler-Lobby und Matchmaking | Multiplayer lobby and matchmaking
