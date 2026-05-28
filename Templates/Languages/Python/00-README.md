# Python Template Collection 📦

> Eine Sammlung fertiger Python-Projektvorlagen für schnellen Start.
> A collection of ready-to-use Python project templates for quick starts.

---

## Übersicht | Overview

Diese Vorlagen decken die häufigsten Python-Anwendungsbereiche ab – von CLI-Tools über GUIs, Web-Apps, Bots, Data Science bis zu Netzwerk-Tools. Jede Vorlage enthält produktionsreifen, modernen Python-Code (3.11+, Type Hints, f-strings, Dataclasses).

These templates cover the most common Python application areas – from CLI tools to GUIs, web apps, bots, data science, and networking tools. Each template contains production-grade, modern Python code (3.11+, type hints, f-strings, dataclasses).

---

## Template-Liste | Template List

| # | Name | Bereich / Area |
|---|------|----------------|
| 1 | `Console-CLI` | Einfaches CLI / Basic CLI |
| 2 | `Console-Interactive` | Interaktive Konsole / Interactive console |
| 3 | `Console-ArgumentParser` | CLI mit Argumenten / CLI with arguments |
| 4 | `GUI-Tkinter` | Tkinter Fenster / Tkinter window |
| 5 | `GUI-PyQt` | PyQt6 Desktop-App / PyQt6 desktop app |
| 6 | `GUI-PyGame` | Pygame Fenster / Pygame window |
| 7 | `GUI-Kivy` | Kivy Mobile-App / Kivy mobile app |
| 8 | `Web-Flask` | Flask Web-App / Flask web app |
| 9 | `Web-Django` | Django Projekt / Django project |
| 10 | `Web-FastAPI` | FastAPI REST-API / FastAPI REST API |
| 11 | `Web-Streamlit` | Streamlit Daten-App / Streamlit data app |
| 12 | `DiscordBot` | Discord Bot / Discord bot |
| 13 | `TelegramBot` | Telegram Bot / Telegram bot |
| 14 | `DataScience-Analysis` | Datenanalyse / Data analysis |
| 15 | `DataScience-ML` | Maschinelles Lernen / Machine learning |
| 16 | `Automation-Scripting` | Datei-Automation / File automation |
| 17 | `Automation-Testing` | Tests mit pytest / Testing with pytest |
| 18 | `Network-Server` | TCP/HTTP Server / TCP/HTTP server |
| 19 | `Network-Client` | TCP/HTTP Client / TCP/HTTP client |
| 20 | `Network-Scraper` | Web Scraper / Web scraper |
| 21 | `Game-Pygame` | Vollständiges Spiel / Full game |

---

## Verwendung | Usage

Jede Vorlage ist ein eigenständiges Verzeichnis. Navigiere hinein und lies die README.md für Details.

Each template is a standalone directory. Navigate into it and read the README.md for details.

```bash
cd Templates/Languages/Python/Web-Flask
python3 -m venv venv
source venv/bin/activate
pip install -r requirements.txt
python app.py
```

---

## Technische Hinweise | Technical Notes

- **Python 3.11+** wird vorausgesetzt / is required
- **Type Hints** werden durchgängig verwendet / are used throughout
- **Virtual Environment** wird empfohlen / is recommended
- Code folgt **PEP 8** / follows PEP 8

---

## Erweiterungsideen | Extension Ideas

- Template-Generator-Skript schreiben (CLI, das Vorlagen kopiert)
- CI/CD-Pipeline für automatisierte Tests aller Vorlagen
- Docker-Container für jede Web-Vorlage
- Gemeinsame `utils.py`-Bibliothek für alle Vorlagen

- Write a template generator script (CLI that copies templates)
- CI/CD pipeline for automated testing of all templates
- Docker container for each web template
- Shared `utils.py` library for all templates
