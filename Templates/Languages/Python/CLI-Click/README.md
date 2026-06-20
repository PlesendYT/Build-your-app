# Click CLI Template

> Strukturierte CLI-Anwendungen mit Click. / Structured CLI applications with Click.

## Features
- Mehrere Sub-Commands / Multiple sub-commands
- Optionen und Argumente / Options and arguments
- Saubere Exit-Codes / Clean exit codes
- Integrierte Hilfe-Texte / Built-in help text

## How to Use
```bash
cd CLI-Click
pip install -r requirements.txt
python cli.py hello --name World
python cli.py add 1 2 3
```

## Tech Stack
- Python 3.11+
- Click

## Extension Ideas
- pytest-click für Tests / pytest-click for tests
- Rich für bunte Ausgaben / Rich for colorful output
- Packaging als pip-Install / Package as pip-installable