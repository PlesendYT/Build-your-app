"""Pygame application entry point."""

from __future__ import annotations

import pygame
from game import Game


def main() -> None:
    """Initialize and run the Pygame application."""
    pygame.init()
    screen = pygame.display.set_mode((800, 600))
    pygame.display.set_caption("PyGame App")
    clock = pygame.time.Clock()

    game = Game(screen)
    game.run(clock)

    pygame.quit()


if __name__ == "__main__":
    main()
