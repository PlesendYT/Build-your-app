"""Bot configuration loaded from environment variables."""

from __future__ import annotations

import os
from dataclasses import dataclass, field


@dataclass
class BotConfig:
    """Application configuration for the Discord AI bot."""

    # Discord bot token from the Discord Developer Portal
    token: str = ""

    # Command prefix for traditional text commands
    prefix: str = "!"

    # AI API key for the language model backend
    api_key: str = ""

    # Name of the AI model to use for responses
    model: str = "gpt-3.5-turbo"

    @classmethod
    def from_env(cls) -> BotConfig:
        """Create a BotConfig instance from environment variables."""
        return cls(
            token=os.getenv("DISCORD_TOKEN", ""),
            prefix=os.getenv("BOT_PREFIX", "!"),
            api_key=os.getenv("AI_API_KEY", ""),
            model=os.getenv("AI_MODEL", "gpt-3.5-turbo"),
        )
