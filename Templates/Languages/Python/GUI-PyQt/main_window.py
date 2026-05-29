"""Main window for PyQt6 application."""

from __future__ import annotations

from PyQt6.QtCore import Qt
from PyQt6.QtWidgets import (
    QLabel,
    QLineEdit,
    QMainWindow,
    QPushButton,
    QVBoxLayout,
    QWidget,
)


class MainWindow(QMainWindow):
    """Custom main window."""

    def __init__(self) -> None:
        super().__init__()
        self.setWindowTitle("PyQt6 App")
        self.setMinimumSize(400, 300)
        self._setup_ui()

    def _setup_ui(self) -> None:
        """Build the user interface."""
        # Create a central widget with a vertical layout
        central = QWidget()
        self.setCentralWidget(central)
        layout = QVBoxLayout(central)

        # Greeting label at the top
        self.label = QLabel("Welcome to PyQt6!")
        self.label.setAlignment(Qt.AlignmentFlag.AlignCenter)
        layout.addWidget(self.label)

        # Name input field
        self.input = QLineEdit()
        self.input.setPlaceholderText("Enter your name")
        layout.addWidget(self.input)

        # Greet button connected to handler
        self.button = QPushButton("Greet")
        self.button.clicked.connect(self._on_greet)
        layout.addWidget(self.button)

        # Result label that updates on greet
        self.result = QLabel("")
        self.result.setAlignment(Qt.AlignmentFlag.AlignCenter)
        layout.addWidget(self.result)

    def _on_greet(self) -> None:
        """Handle greet button click."""
        name = self.input.text().strip()
        self.result.setText(f"Hello, {name or 'World'}!")
