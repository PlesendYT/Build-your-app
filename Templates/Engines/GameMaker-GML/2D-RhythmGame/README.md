# 2D-RhythmGame (GameMaker GML)

## Beschreibung / Description

**DE:** Ein Rhythmusspiel mit Noten, die im Takt fallen, und Punktesystem für Timing.

**EN:** A rhythm game with notes falling to the beat and a timing-based scoring system.

## Features

- Fallende Noten / Falling notes
- Takt-Synchronisation / Beat synchronization
- Timing-Bewertung (Perfect, Great, Good, Miss) / Timing ratings
- Punkte und Combo / Score and combo
- Mehrere Songs / Multiple songs
- Geschwindigkeitseinstellung / Speed setting

## How to Use

1. obj_note, obj_receptor, obj_rhythm_manager, obj_score_display
2. Song als JSON (Takt-Daten)
3. 4 Spalten (Pfeiltasten oder D/F/J/K)

## Tech Stack

- GameMaker LTS 2024 / GML
- audio_sound_get_track_position für Takt
- draw_sprite_ext für Noten-Rendering
- ds_list für Noten-Queue

## Extension Ideas

- Partikeleffekte / Particle effects
- Chart-Editor
- Verschiedene Schwierigkeitsgrade / Difficulty levels
- Online-Ranglisten / Online leaderboards
