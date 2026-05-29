"""Discord AI bot that responds to messages using an external AI API."""

from __future__ import annotations

import discord
from discord.ext import commands

from config import BotConfig

# Load configuration from the config module
config = BotConfig.from_env()

# Set up the bot with a command prefix and required intents
intents = discord.Intents.default()
intents.message_content = True
bot = commands.Bot(command_prefix=config.prefix, intents=intents)


async def get_ai_response(prompt: str) -> str:
    """Call an external AI API and return the response text."""
    # Placeholder for an AI API call
    # Replace with: openai.ChatCompletion.create(...) or similar
    return f"AI reply to: \"{prompt[:50]}\"... [API response placeholder]"


@bot.event
async def on_ready() -> None:
    """Log bot login information when ready."""
    assert bot.user is not None
    print(f"Logged in as {bot.user} (ID: {bot.user.id})")


@bot.event
async def on_message(message: discord.Message) -> None:
    """Respond to messages with an AI-generated reply."""
    # Ignore messages from the bot itself
    if message.author == bot.user:
        return

    # Only respond if the bot is mentioned or it's a DM
    if bot.user in message.mentions or isinstance(message.channel, discord.DMChannel):
        # Strip the mention from the prompt
        prompt = message.content.replace(f"<@{bot.user.id}>", "").strip()
        if not prompt:
            await message.channel.send("How can I help you?")
            return

        async with message.channel.typing():
            reply = await get_ai_response(prompt)
            await message.reply(reply, mention_author=False)

    # Allow commands to still process
    await bot.process_commands(message)


@bot.command(name="ping")
async def ping(ctx: commands.Context) -> None:
    """Check the bot's latency."""
    latency = round(bot.latency * 1000)
    await ctx.send(f"Pong! {latency}ms")


def main() -> None:
    """Start the Discord bot."""
    if not config.token:
        raise ValueError("Discord bot token is not configured")
    bot.run(config.token)


if __name__ == "__main__":
    main()
