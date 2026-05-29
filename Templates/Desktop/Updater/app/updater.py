"""
Python updater script for checking and applying application updates.
Downloads new versions from a remote manifest and replaces the local installation.
"""

import os
import sys
import json
import shutil
import tempfile
import zipfile
from pathlib import Path
from urllib.request import urlopen, Request


# URLs and paths for update checking and installation
VERSION_URL = "https://example.com/app/version.json"
DOWNLOAD_URL = "https://example.com/app/downloads/{version}.zip"
APP_DIR = Path.home() / ".mydesktopapp"
BACKUP_DIR = APP_DIR / "backup"


def get_current_version():
    """Read the current installed version from the local version file."""
    version_file = APP_DIR / "version.json"
    if version_file.exists():
        with open(version_file) as f:
            return json.load(f).get("version", "0.0.0")
    return "0.0.0"


def fetch_remote_version():
    """Download the remote version manifest and return the parsed JSON data."""
    req = Request(VERSION_URL, headers={"User-Agent": "Updater/1.0"})
    with urlopen(req, timeout=10) as response:
        return json.loads(response.read().decode("utf-8"))


def compare_versions(current, remote):
    """Compare two semantic version strings, returns True if remote is newer."""
    def parse(v):
        return tuple(map(int, v.split(".")))
    return parse(remote) > parse(current)


def download_update(version):
    """Download the update zip file for the given version and return the temp path."""
    url = DOWNLOAD_URL.format(version=version)
    print(f"Downloading update from {url}...")
    req = Request(url, headers={"User-Agent": "Updater/1.0"})
    tmp_file = tempfile.NamedTemporaryFile(delete=False, suffix=".zip")
    with urlopen(req, timeout=30) as response:
        shutil.copyfileobj(response, tmp_file)
    tmp_file.close()
    return tmp_file.name


def install_update(zip_path, version):
    """Extract the update zip and replace the current application files."""
    # Backup the current installation
    if BACKUP_DIR.exists():
        shutil.rmtree(BACKUP_DIR)
    shutil.copytree(APP_DIR, BACKUP_DIR)
    print(f"Backup created at {BACKUP_DIR}")

    # Extract update over the existing installation
    with zipfile.ZipFile(zip_path, "r") as zf:
        zf.extractall(APP_DIR)
    print("Update files extracted")

    # Write the new version number
    with open(APP_DIR / "version.json", "w") as f:
        json.dump({"version": version}, f)
    print(f"Updated to version {version}")

    # Clean up the temporary zip file
    os.unlink(zip_path)


def check_for_updates():
    """Main update routine: check version and apply update if available."""
    current = get_current_version()
    print(f"Current version: {current}")

    try:
        remote_data = fetch_remote_version()
        remote_version = remote_data.get("version", "0.0.0")
        print(f"Remote version: {remote_version}")

        if compare_versions(current, remote_version):
            print("Update available, downloading...")
            zip_path = download_update(remote_version)
            install_update(zip_path, remote_version)
            print("Update complete!")
        else:
            print("Already up to date.")
    except Exception as e:
        print(f"Update check failed: {e}")
        sys.exit(1)


if __name__ == "__main__":
    check_for_updates()
