"""Basic CLI application template."""

from __future__ import annotations

import sys


def greet(name: str) -> str:
    """Return a greeting message."""
    return f"Hello, {name}! Welcome to the CLI template."


def main() -> None:
    """Main entry point."""
    args = sys.argv[1:]
    name = args[0] if args else "World"
    print(greet(name))


if __name__ == "__main__":
    main()
