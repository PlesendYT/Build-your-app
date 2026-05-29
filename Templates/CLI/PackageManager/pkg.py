"""Python package manager CLI: install, remove, list, update.

A simulated package manager that operates on a local JSON database to
track installed packages. Demonstrates a clean CLI structure with
subcommands and argument parsing.
"""

import json
import sys
import argparse
from pathlib import Path

# Path to the local package database file
DB_PATH = Path.home() / ".pkg_manager_db.json"


def load_db():
    """Load the package database from disk, returning an empty dict if missing."""
    if DB_PATH.exists():
        return json.loads(DB_PATH.read_text())
    return {}


def save_db(db):
    """Persist the package database to disk as pretty-printed JSON."""
    DB_PATH.write_text(json.dumps(db, indent=2))


def cmd_install(args):
    """Install one or more packages by adding them to the database."""
    db = load_db()
    for pkg in args.packages:
        if pkg in db:
            print(f"Package '{pkg}' is already installed")
        else:
            db[pkg] = {"version": "1.0.0", "status": "installed"}
            print(f"Installed '{pkg}'")
    save_db(db)


def cmd_remove(args):
    """Remove specified packages from the database."""
    db = load_db()
    for pkg in args.packages:
        if pkg in db:
            del db[pkg]
            print(f"Removed '{pkg}'")
        else:
            print(f"Package '{pkg}' not found")
    save_db(db)


def cmd_list(args):
    """List all currently installed packages with their versions."""
    db = load_db()
    if not db:
        print("No packages installed")
        return
    print(f"{'Package':<20} {'Version':<10} {'Status':<10}")
    print("-" * 40)
    for name, info in db.items():
        print(f"{name:<20} {info['version']:<10} {info['status']:<10}")


def cmd_update(args):
    """Simulate updating all packages to a newer version."""
    db = load_db()
    for name, info in db.items():
        old_ver = info["version"]
        info["version"] = "2.0.0"
        print(f"Updated '{name}' ({old_ver} -> {info['version']})")
    save_db(db)


def main():
    parser = argparse.ArgumentParser(description="Simple Python Package Manager")
    subparsers = parser.add_subparsers(dest="command", required=True)

    install_parser = subparsers.add_parser("install", help="Install packages")
    install_parser.add_argument("packages", nargs="+", help="Package names")

    remove_parser = subparsers.add_parser("remove", help="Remove packages")
    remove_parser.add_argument("packages", nargs="+", help="Package names")

    subparsers.add_parser("list", help="List installed packages")
    subparsers.add_parser("update", help="Update all packages")

    args = parser.parse_args()
    commands = {
        "install": cmd_install,
        "remove": cmd_remove,
        "list": cmd_list,
        "update": cmd_update,
    }
    commands[args.command](args)


if __name__ == "__main__":
    main()
