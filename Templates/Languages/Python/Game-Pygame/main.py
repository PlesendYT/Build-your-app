"""Game entry point and main loop."""

from __future__ import annotations

import pygame
import sys

from player import Player
from enemy import Enemy

# Window and timing constants
WINDOW_WIDTH = 800
WINDOW_HEIGHT = 600
FPS = 60


class Game:
    """Main game class managing the game loop."""

    def __init__(self) -> None:
        # Initialize Pygame and create the display window
        pygame.init()
        self.screen = pygame.display.set_mode((WINDOW_WIDTH, WINDOW_HEIGHT))
        pygame.display.set_caption("PyGame Game")
        self.clock = pygame.time.Clock()
        self.font = pygame.font.Font(None, 36)

        # Create player at center and set up enemy group
        self.player = Player(WINDOW_WIDTH // 2, WINDOW_HEIGHT // 2)
        self.enemies = pygame.sprite.Group()
        self.score = 0
        self.spawn_timer = 0
        self.running = True

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
        # Update player position based on pressed keys
        keys = pygame.key.get_pressed()
        self.player.update(keys, WINDOW_WIDTH, WINDOW_HEIGHT)

        # Spawn a new enemy every 60 frames
        self.spawn_timer += 1
        if self.spawn_timer >= 60:
            self.enemies.add(Enemy(WINDOW_WIDTH, WINDOW_HEIGHT))
            self.spawn_timer = 0

        # Move all enemies
        self.enemies.update()

        # Check collisions between player and enemies
        hits = pygame.sprite.spritecollide(
            self.player, self.enemies, True,
        )
        self.score += len(hits)

        # End game if player health reaches zero
        if self.player.health <= 0:
            self.running = False

    def render(self) -> None:
        """Draw everything."""
        # Clear screen with dark background
        self.screen.fill((20, 20, 40))
        self.player.draw(self.screen)
        self.enemies.draw(self.screen)
        # Render score and health text
        score_text = self.font.render(
            f"Score: {self.score}", True, (255, 255, 255),
        )
        health_text = self.font.render(
            f"Health: {self.player.health}", True, (255, 100, 100),
        )
        self.screen.blit(score_text, (10, 10))
        self.screen.blit(health_text, (10, 50))
        pygame.display.flip()

    def run(self) -> None:
        """Main game loop."""
        while self.running:
            self.handle_events()
            self.update()
            self.render()
            self.clock.tick(FPS)

        # Show game over screen for 3 seconds then exit
        self.screen.fill((0, 0, 0))
        go_text = self.font.render(
            f"Game Over! Score: {self.score}", True, (255, 255, 255),
        )
        self.screen.blit(go_text, (WINDOW_WIDTH // 2 - 100, WINDOW_HEIGHT // 2))
        pygame.display.flip()
        pygame.time.wait(3000)
        pygame.quit()
        sys.exit()


def main() -> None:
    game = Game()
    game.run()


if __name__ == "__main__":
    main()
