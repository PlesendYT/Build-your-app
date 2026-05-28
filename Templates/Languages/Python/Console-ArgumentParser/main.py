"""CLI application using argparse."""

from __future__ import annotations

import argparse

from args import parse_args


def greet(name: str, count: int, uppercase: bool) -> list[str]:
    """Generate greeting messages."""
    messages = [f"Hello, {name}!"] * count
    if uppercase:
        messages = [m.upper() for m in messages]
    return messages


def main() -> None:
    """Main entry point."""
    args = parse_args()
    messages = greet(args.name, args.count, args.uppercase)
    for msg in messages:
        print(msg)


if __name__ == "__main__":
    main()
