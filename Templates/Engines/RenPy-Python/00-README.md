# RenPy Python Templates

RenPy-Vorlagen für visuelle Romane und interaktive Geschichten mit der RenPy Engine (Python-basiert).

RenPy templates for visual novels and interactive stories using the RenPy engine (Python-based).

## Vorlagen / Templates

| Genre | Beschreibung / Description |
|-------|--------------------------|
| VisualNovel-Dating | Datingsimulation / Dating simulation |
| VisualNovel-Mystery | Mysteriengeschichte / Mystery story |
| VisualNovel-Fantasy | Fantasy-Abenteuer / Fantasy adventure |
| VisualNovel-SciFi | Science-Fiction / Science fiction |
| VisualNovel-Horror | Horrorspiel / Horror game |
| VisualNovel-Romance | Liebesgeschichte / Romance story |
| VisualNovel-Comedy | Comedy-Geschichte / Comedy story |
| KineticNovel | Kinetischer Roman (keine Entscheidungen) / Kinetic novel (no choices) |
| ChoiceGame | Entscheidungsbasiertes Spiel / Choice-based game |

## Verwendung / Usage

Jede Vorlage enthält `script.rpy` (Hauptstory), `screens.rpy` (UI) und `characters.rpy` (Charaktere).
In RenPy ein neues Projekt erstellen und die .rpy-Dateien in das Spielverzeichnis kopieren.

Each template includes `script.rpy` (main story), `screens.rpy` (UI), and `characters.rpy` (characters).
Create a new project in RenPy and copy the .rpy files into the game directory.

## Konventionen / Conventions

- `define` für Charaktere und persistenten Daten
- `label start:` für den Einstieg
- `menu:` für Entscheidungen
- `screen` für UI-Elemente
- `image` für Hintergründe und CG
- Python-Blöcke mit `$` oder `python:`
