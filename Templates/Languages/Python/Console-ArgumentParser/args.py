"""Argument parser definition."""

from __future__ import annotations

import argparse


def parse_args(argv: list[str] | None = None) -> argparse.Namespace:
    """Parse and return command-line arguments."""
    parser = argparse.ArgumentParser(
        description="A sample CLI with argparse.",
        formatter_class=argparse.RawDescriptionHelpFormatter,
    )
    # Name to greet (positional-style with flag)
    parser.add_argument(
        "-n", "--name",
        type=str,
        default="World",
        help="Name to greet (default: %(default)s)",
    )
    # Number of times to repeat the greeting
    parser.add_argument(
        "-c", "--count",
        type=int,
        default=1,
        help="Number of repetitions (default: %(default)s)",
    )
    # Flag to output greeting in uppercase
    parser.add_argument(
        "-u", "--uppercase",
        action="store_true",
        help="Print greeting in uppercase",
    )
    return parser.parse_args(argv)
