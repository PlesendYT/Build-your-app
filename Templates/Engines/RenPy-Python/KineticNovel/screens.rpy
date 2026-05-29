## Screens for Kinetic Novel
# Simple UI with auto-forward, save, and preferences buttons

screen kinetic_ui():
    textbutton "Auto" action Preference("auto-forward", "toggle")
    textbutton "Save" action ShowMenu("save")
    textbutton "Prefs" action ShowMenu("preferences")

## Enable auto-forward for reading experience
init python:
    config.auto_forward_time = 2000
