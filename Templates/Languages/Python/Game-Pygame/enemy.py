"""Enemy class for the Pygame game."""

from __future__ import annotations

import random

import pygame


class Enemy(pygame.sprite.Sprite):
    """Enemy that moves downward."""

    def __init__(self, screen_width: int, screen_height: int) -> None:
        super().__init__()
        self.image = pygame.Surface((24, 24))
        self.image.fill((255, 50, 50))
        self.rect = self.image.get_rect(
            center=(
                random.randint(20, screen_width - 20),
                -20,
            ),
        )
        self.speed = random.randint(2, 5)
        self.screen_height = screen_height

    def update(self, *args: object, **kwargs: object) -> None:
        """Move enemy downward."""
        self.rect.y += self.speed
        if self.rect.top > self.screen_height:
            self.kill()
