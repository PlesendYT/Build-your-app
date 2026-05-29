"""Core game loop and logic."""

from __future__ import annotations

import pygame


class Game:
    """Manages the game state and rendering."""

    def __init__(self, screen: pygame.Surface) -> None:
        self.screen = screen
        self.running = False
        self.color = (100, 149, 237)  # Cornflower blue

    def handle_events(self) -> None:
        """Process input events."""
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                self.running = False
            elif event.type == pygame.KEYDOWN:
                if event.key == pygame.K_ESCAPE:
                    self.running = False

    def update(self) -> None:
        """Update game state."""
        pass

    def render(self) -> None:
        """Draw everything to the screen."""
        # Clear screen and draw a blue circle at center
        self.screen.fill((30, 30, 30))
        pygame.draw.circle(
            self.screen, self.color,
            (400, 300), 50,
        )
        pygame.display.flip()

    def run(self, clock: pygame.time.Clock) -> None:
        """Main game loop."""
        self.running = True
        while self.running:
            self.handle_events()
            self.update()
            self.render()
            clock.tick(60)
