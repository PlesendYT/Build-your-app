"""Click-based CLI template."""
from __future__ import annotations

import click


@click.group()
def main() -> None:
    """Demo CLI built with Click."""
    pass


@main.command()
@click.option("--name", default="World", show_default=True, help="Name to greet.")
@click.option("--shout/--no-shout", default=False, help="Uppercase the greeting.")
def hello(name: str, shout: bool) -> None:
    """Print a friendly greeting."""
    msg = f"Hello, {name}!"
    click.echo(msg.upper() if shout else msg)


@main.command()
@click.argument("numbers", nargs=-1, type=float)
def add(numbers: tuple[float, ...]) -> None:
    """Sum a list of numbers."""
    total = sum(numbers)
    click.echo(f"sum = {total}")


if __name__ == "__main__":
    main()