"""File and OS automation script."""

from __future__ import annotations

from datetime import datetime
from pathlib import Path


def create_project_structure(base: Path) -> None:
    """Create a sample project directory structure."""
    folders = ["src", "tests", "docs", "data/raw", "data/processed"]
    for folder in folders:
        (base / folder).mkdir(parents=True, exist_ok=True)
        print(f"Created: {base / folder}")


def create_sample_files(base: Path) -> None:
    """Create sample files with timestamps."""
    timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
    files = {
        "src/main.py": f"# Main module - generated {timestamp}\ndef main() -> None:\n    print('Hello from automation!')\n",
        "src/utils.py": f"# Utilities - generated {timestamp}\n",
        "docs/README.md": f"# Project Documentation\n\nGenerated on {timestamp}\n",
    }
    for rel_path, content in files.items():
        filepath = base / rel_path
        filepath.write_text(content)
        print(f"Created: {filepath}")


def cleanup(base: Path) -> None:
    """Remove all .pyc files and __pycache__ dirs."""
    count = 0
    for pyc in base.rglob("*.pyc"):
        pyc.unlink()
        count += 1
    for cache in base.rglob("__pycache__"):
        cache.rmdir()
        count += 1
    print(f"Cleaned up {count} items")


def main() -> None:
    root = Path("example_project")
    if root.exists():
        print("Removing existing example_project...")
        import shutil
        shutil.rmtree(root)

    print("Creating project structure...")
    create_project_structure(root)
    create_sample_files(root)
    print("\nDone! Example project created at:", root.resolve())


if __name__ == "__main__":
    main()
