# 3D-RPG – Rollenspiel Template (UE5)

**Deutsch:** Ein umfassendes Rollenspiel-Grundgerüst mit Inventarsystem, Charakterwerten, Quest-Log und Dialogsystem für Unreal Engine 5.

**English:** A comprehensive RPG skeleton with inventory system, character stats, quest log, and dialogue system for Unreal Engine 5.

## Features

- Attributsystem (Stärke, Geschick, Intelligenz, Konstitution) | Attribute system (strength, dexterity, intelligence, constitution)
- Inventar mit Rüstung, Waffen, Verbrauchsgegenständen | Inventory with armor, weapons, consumables
- Quest-System mit Belohnungen und Fortschritt | Quest system with rewards and progress
- Dialog-System mit Entscheidungsbäumen | Dialogue system with branching choices
- Level-System mit Erfahrungspunkten | Level system with experience points

## How to Use

1. Add files to `Source/RPG/` folder.
2. Set `ARPGGameMode` as default game mode.
3. Create Blueprint children for items, quests, and dialogues.
4. Bind EnhancedInput actions in Blueprint.

## Tech Stack

- UE5.4+, C++17, EnhancedInput, UMG, DataTables

## Extension Ideas

- Fähigkeiten-Baum mit freischaltbaren Skills | Skill tree with unlockable abilities
- Fraktionssystem und Ruf | Faction system and reputation
- Handwerks- und Verzauberungssystem | Crafting and enchanting system
