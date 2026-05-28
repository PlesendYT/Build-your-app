"""Discord bot entry point."""

from __future__ import annotations

import os

import discord
from discord.ext import commands


intents = discord.Intents.default()
intents.message_content = True

bot = commands.Bot(command_prefix="!", intents=intents)


@bot.event
async def on_ready() -> None:
    """Called when the bot is ready."""
    assert bot.user is not None
    print(f"Logged in as {bot.user}")


@bot.hybrid_command(name="hello")
async def hello(ctx: commands.Context) -> None:
    """Say hello to the user."""
    await ctx.send(f"Hello, {ctx.author.mention}!")


@bot.hybrid_command(name="ping")
async def ping(ctx: commands.Context) -> None:
    """Check bot latency."""
    latency = round(bot.latency * 1000)
    await ctx.send(f"Pong! {latency}ms")


async def load_cogs() -> None:
    """Load extensions."""
    await bot.load_extension("cogs.example")


def main() -> None:
    """Start the bot."""
    token = os.getenv("DISCORD_TOKEN")
    if not token:
        raise ValueError("DISCORD_TOKEN environment variable is not set")
    bot.run(token)


if __name__ == "__main__":
    import asyncio
    asyncio.run(load_cogs())
    main()
