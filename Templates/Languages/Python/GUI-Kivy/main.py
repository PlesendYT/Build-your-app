"""Kivy application entry point."""

from __future__ import annotations

from kivy.app import App
from kivy.uix.boxlayout import BoxLayout


class MainLayout(BoxLayout):
    """Custom root widget defined in app.kv."""

    def greet(self, name_input: str) -> None:
        """Handle greet action from the UI."""
        name = name_input.strip() or "World"
        self.ids.result_label.text = f"Hello, {name}!"


class KivyApp(App):
    """Main application class."""

    def build(self) -> MainLayout:
        self.title = "Kivy App"
        return MainLayout()


def main() -> None:
    KivyApp().run()


if __name__ == "__main__":
    main()
