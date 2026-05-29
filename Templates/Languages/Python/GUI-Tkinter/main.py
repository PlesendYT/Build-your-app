"""Tkinter desktop application template."""

from __future__ import annotations

import tkinter as tk
from tkinter import ttk


class Application(ttk.Frame):
    """Main application window."""

    def __init__(self, master: tk.Tk) -> None:
        super().__init__(master)
        self.master.title("Tkinter App")
        self.master.geometry("400x300")
        self.create_widgets()

    def create_widgets(self) -> None:
        """Create and arrange UI widgets."""
        # Welcome label
        self.label = ttk.Label(self, text="Welcome to Tkinter!")
        self.label.pack(pady=20)

        # Name entry field
        self.name_var = tk.StringVar()
        self.entry = ttk.Entry(self, textvariable=self.name_var)
        self.entry.pack(pady=5)
        self.entry.insert(0, "Enter your name")

        # Greet button wired to on_greet handler
        self.button = ttk.Button(
            self, text="Greet", command=self.on_greet
        )
        self.button.pack(pady=10)

        # Result label showing the greeting
        self.result = ttk.Label(self, text="")
        self.result.pack(pady=10)

        self.pack(fill=tk.BOTH, expand=True)

    def on_greet(self) -> None:
        """Handle greet button click."""
        name = self.name_var.get().strip()
        self.result.config(text=f"Hello, {name or 'World'}!")


def main() -> None:
    """Start the application."""
    root = tk.Tk()
    app = Application(master=root)
    app.mainloop()


if __name__ == "__main__":
    main()
