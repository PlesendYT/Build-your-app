"""PyQt6 desktop application entry point."""

from __future__ import annotations

import sys

from PyQt6.QtWidgets import QApplication

from main_window import MainWindow


def main() -> None:
    """Start the PyQt6 application."""
    app = QApplication(sys.argv)
    window = MainWindow()
    window.show()
    sys.exit(app.exec())


if __name__ == "__main__":
    main()
