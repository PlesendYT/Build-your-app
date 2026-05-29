"""Python git automation tool: batch operations and repo management.

Provides a command-line interface for common multi-repository git
workflows such as status checks, bulk commits, and branch management.
"""

import os
import sys
import argparse
import subprocess
from pathlib import Path


def find_git_repos(root_dir):
    """Yield all directories under root_dir that contain a .git folder."""
    root = Path(root_dir).resolve()
    for path in root.rglob(".git"):
        if path.is_dir():
            yield path.parent


def run_git_command(repo_path, *args):
    """Execute a git command in the specified repository and return output."""
    result = subprocess.run(
        ["git", *args],
        cwd=repo_path,
        capture_output=True,
        text=True,
    )
    return result.stdout.strip(), result.stderr.strip()


def cmd_status(args):
    """Show git status for all repositories under the given root."""
    for repo in find_git_repos(args.root):
        stdout, stderr = run_git_command(repo, "status", "--short")
        if stdout:
            print(f"\n=== {repo.name} ===")
            print(stdout)
        if stderr:
            print(f"Error in {repo.name}: {stderr}")


def cmd_commit(args):
    """Stage all changes and create a commit in every repository."""
    for repo in find_git_repos(args.root):
        run_git_command(repo, "add", "-A")
        stdout, stderr = run_git_command(repo, "commit", "-m", args.message)
        if stdout:
            print(f"\n=== {repo.name} ===\n{stdout}")
        if stderr:
            print(f"Error in {repo.name}: {stderr}")


def cmd_pull(args):
    """Pull the latest changes in every repository."""
    for repo in find_git_repos(args.root):
        stdout, stderr = run_git_command(repo, "pull")
        print(f"\n=== {repo.name} ===\n{stdout}")
        if stderr:
            print(f"Error: {stderr}")


def cmd_branch(args):
    """List all branches across all repositories."""
    for repo in find_git_repos(args.root):
        stdout, stderr = run_git_command(repo, "branch", "-a")
        print(f"\n=== {repo.name} ===")
        print(stdout)


def main():
    parser = argparse.ArgumentParser(description="Multi-Repo Git Tool")
    parser.add_argument("--root", default=".", help="Root directory to scan for repos")
    subparsers = parser.add_subparsers(dest="command", required=True)

    subparsers.add_parser("status", help="Show status of all repos")
    subparsers.add_parser("pull", help="Pull latest changes in all repos")

    commit_parser = subparsers.add_parser("commit", help="Commit in all repos")
    commit_parser.add_argument("-m", "--message", required=True, help="Commit message")

    subparsers.add_parser("branch", help="List branches in all repos")

    args = parser.parse_args()
    commands = {
        "status": cmd_status,
        "commit": cmd_commit,
        "pull": cmd_pull,
        "branch": cmd_branch,
    }
    commands[args.command](args)


if __name__ == "__main__":
    main()
