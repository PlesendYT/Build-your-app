"""Python backup tool: file sync, incremental backup, and scheduling.

Watches a source directory and mirrors changes to a destination using
file hashing. Supports full and incremental backup modes.
"""

import os
import sys
import json
import shutil
import hashlib
import argparse
from pathlib import Path

# Manifest file used to track incremental backup state
MANIFEST = ".backup_manifest.json"


def file_hash(path):
    """Compute SHA-256 hash of a file for change detection."""
    h = hashlib.sha256()
    with open(path, "rb") as f:
        for chunk in iter(lambda: f.read(65536), b""):
            h.update(chunk)
    return h.hexdigest()


def load_manifest(dest):
    """Load the backup manifest from the destination directory."""
    manifest_path = Path(dest) / MANIFEST
    if manifest_path.exists():
        return json.loads(manifest_path.read_text())
    return {}


def save_manifest(dest, manifest):
    """Save the backup manifest to the destination directory."""
    (Path(dest) / MANIFEST).write_text(json.dumps(manifest, indent=2))


def backup_full(src, dest):
    """Perform a full backup: copy everything from src to dest."""
    src_path = Path(src)
    dest_path = Path(dest)
    dest_path.mkdir(parents=True, exist_ok=True)

    manifest = {}
    for file_path in src_path.rglob("*"):
        if file_path.is_file():
            rel = file_path.relative_to(src_path)
            target = dest_path / rel
            target.parent.mkdir(parents=True, exist_ok=True)
            shutil.copy2(file_path, target)
            manifest[str(rel)] = file_hash(file_path)

    save_manifest(dest, manifest)
    print(f"Full backup complete: {len(manifest)} files")


def backup_incremental(src, dest):
    """Perform an incremental backup: only copy changed/new files."""
    src_path = Path(src)
    dest_path = Path(dest)
    dest_path.mkdir(parents=True, exist_ok=True)

    manifest = load_manifest(dest)
    changes = 0

    for file_path in src_path.rglob("*"):
        if file_path.is_file():
            rel = str(file_path.relative_to(src_path))
            current_hash = file_hash(file_path)
            if manifest.get(rel) != current_hash:
                target = dest_path / rel
                target.parent.mkdir(parents=True, exist_ok=True)
                shutil.copy2(file_path, target)
                manifest[rel] = current_hash
                changes += 1

    save_manifest(dest, manifest)
    print(f"Incremental backup: {changes} files updated")


def main():
    parser = argparse.ArgumentParser(description="Backup Tool")
    parser.add_argument("src", help="Source directory to back up")
    parser.add_argument("dest", help="Destination backup directory")
    parser.add_argument(
        "--mode", choices=["full", "incremental"], default="incremental",
        help="Backup mode (default: incremental)"
    )
    args = parser.parse_args()

    if not Path(args.src).is_dir():
        print(f"Error: source '{args.src}' is not a directory")
        sys.exit(1)

    if args.mode == "full":
        backup_full(args.src, args.dest)
    else:
        backup_incremental(args.src, args.dest)


if __name__ == "__main__":
    main()
