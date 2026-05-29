"""Player class for the Pygame game."""

from __future__ import annotations

import pygame


class Player(pygame.sprite.Sprite):
    """Player-controlled character."""

    def __init__(self, x: int, y: int) -> None:
        super().__init__()
        # Create a 32x32 blue square for the player sprite
        self.image = pygame.Surface((32, 32))
        self.image.fill((0, 150, 255))
        self.rect = self.image.get_rect(center=(x, y))
        self.speed = 5
        self.health = 100

    def update(self, keys: pygame.key.ScancodeWrapper,
               screen_width: int, screen_height: int) -> None:
        """Update position based on key input."""
        # Calculate movement delta from arrow keys or WASD
        dx = dy = 0
        if keys[pygame.K_LEFT] or keys[pygame.K_a]:
            dx -= self.speed
        if keys[pygame.K_RIGHT] or keys[pygame.K_d]:
            dx += self.speed
        if keys[pygame.K_UP] or keys[pygame.K_w]:
            dy -= self.speed
        if keys[pygame.K_DOWN] or keys[pygame.K_s]:
            dy += self.speed

        # Clamp position within screen boundaries
        self.rect.x = max(0, min(self.rect.x + dx, screen_width - self.rect.width))
        self.rect.y = max(0, min(self.rect.y + dy, screen_height - self.rect.height))

    def draw(self, surface: pygame.Surface) -> None:
        """Draw the player."""
        surface.blit(self.image, self.rect)
