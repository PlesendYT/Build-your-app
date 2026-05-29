"""
Python installer script for desktop application setup.
Handles directory creation, dependency verification, and first-run configuration.
"""

import os
import sys
import subprocess
import json
import shutil
from pathlib import Path


# Configuration constants for the installation process
APP_NAME = "MyDesktopApp"
INSTALL_DIR = Path.home() / ".mydesktopapp"
CONFIG_FILE = INSTALL_DIR / "config.json"
REQUIRED_DIRS = ["data", "logs", "cache"]


def check_python_version():
    """Verify that Python 3.8 or higher is available on the system."""
    if sys.version_info < (3, 8):
        print("Error: Python 3.8 or higher is required.")
        sys.exit(1)
    print(f"Python version: {sys.version_info.major}.{sys.version_info.minor}.{sys.version_info.micro}")


def create_directories():
    """Create the application directory structure if it does not exist."""
    INSTALL_DIR.mkdir(parents=True, exist_ok=True)
    for dir_name in REQUIRED_DIRS:
        (INSTALL_DIR / dir_name).mkdir(exist_ok=True)
    print(f"Created directories under {INSTALL_DIR}")


def check_dependencies():
    """Check for required system dependencies like Node.js and Git."""
    deps = {"node": "node --version", "git": "git --version"}
    for name, cmd in deps.items():
        try:
            result = subprocess.run(cmd.split(), capture_output=True, text=True)
            if result.returncode == 0:
                print(f"{name}: {result.stdout.strip()}")
            else:
                print(f"Warning: {name} not found")
        except FileNotFoundError:
            print(f"Warning: {name} not found")


def create_default_config():
    """Write a default configuration file if one does not already exist."""
    if not CONFIG_FILE.exists():
        config = {
            "app_name": APP_NAME,
            "theme": "light",
            "language": "en",
            "first_run": True
        }
        with open(CONFIG_FILE, "w") as f:
            json.dump(config, f, indent=2)
        print(f"Created default config at {CONFIG_FILE}")
    else:
        print("Config file already exists, skipping")


def main():
    """Run the complete installation sequence for the desktop application."""
    print(f"Installing {APP_NAME}...")
    check_python_version()
    create_directories()
    check_dependencies()
    create_default_config()
    print("Installation complete!")


if __name__ == "__main__":
    main()
