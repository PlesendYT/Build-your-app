"""Interactive console application with menu."""

from __future__ import annotations


def show_menu() -> None:
    """Display the main menu."""
    print("\n=== Main Menu ===")
    print("1. Say Hello")
    print("2. Show Info")
    print("3. Exit")


def handle_choice(choice: str) -> bool:
    """Process user choice. Return False to exit."""
    match choice:
        case "1":
            name = input("Enter your name: ").strip()
            print(f"Hello, {name or 'World'}!")
            return True
        case "2":
            print("Interactive Console App v1.0")
            return True
        case "3":
            print("Goodbye!")
            return False
        case _:
            print("Invalid choice. Try again.")
            return True


def main() -> None:
    """Run the interactive loop."""
    running = True
    while running:
        show_menu()
        choice = input("> ").strip()
        running = handle_choice(choice)


if __name__ == "__main__":
    main()
