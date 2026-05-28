# RPGMaker JS Templates

RPGMaker-Vorlagen für Rollenspiele mit JavaScript-Plugins (kompatibel mit RPGMaker MZ und MV).

RPGMaker templates for role-playing games using JavaScript plugins (compatible with RPGMaker MZ and MV).

## Vorlagen / Templates

| Genre | Beschreibung / Description |
|-------|--------------------------|
| FantasyRPG | Klassisches Fantasy-RPG / Classic fantasy RPG |
| ModernRPG | Modernes RPG / Modern RPG |
| SciFiRPG | Science-Fiction-RPG / Sci-fi RPG |
| HorrorRPG | Horror-RPG / Horror RPG |
| ComedyRPG | Comedy-RPG / Comedy RPG |
| PuzzleRPG | Rätsel-RPG / Puzzle RPG |
| TacticalRPG | Taktisches RPG / Tactical RPG |

## Verwendung / Usage

Jede Vorlage enthält JS-Plugincode, der in RPGMaker MZ/MV importiert werden kann.
Die Dateien in den `js/plugins`-Ordner des Projekts kopieren und im Plugin-Manager aktivieren.

Each template contains JS plugin code that can be imported into RPGMaker MZ/MV.
Copy the files into the project's `js/plugins` folder and enable them in the Plugin Manager.

## Konventionen / Conventions

- `Game_Interpreter` für Befehle
- `$gameParty` für Partyzugriff
- `$gameSwitches`, `$gameVariables` für Spielstatus
- `SceneManager` für Szenensteuerung
- Plugin-Befehl-Registrierung mit `Game_Interpreter.prototype.pluginCommand`
- `Window_Base` für Fenstererstellung
