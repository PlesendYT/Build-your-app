"""Example cog for the Discord bot."""

from __future__ import annotations

import discord
from discord.ext import commands


class ExampleCog(commands.Cog):
    """A simple example cog."""

    def __init__(self, bot: commands.Bot) -> None:
        self.bot = bot

    @commands.hybrid_command(name="echo")
    async def echo(self, ctx: commands.Context, *, text: str) -> None:
        """Repeat what the user said."""
        await ctx.send(f"You said: {text}")

    @commands.Cog.listener()
    async def on_message(self, message: discord.Message) -> None:
        """Listen for messages."""
        if message.author.bot:
            return
        if "python" in message.content.lower():
            await message.add_reaction("🐍")


async def setup(bot: commands.Bot) -> None:
    """Register the cog."""
    await bot.add_cog(ExampleCog(bot))
