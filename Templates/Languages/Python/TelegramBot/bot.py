"""Telegram bot using python-telegram-bot."""

from __future__ import annotations

import os

from telegram import Update
from telegram.ext import (
    Application,
    CommandHandler,
    ContextTypes,
    MessageHandler,
    filters,
)


async def start(update: Update, context: ContextTypes.DEFAULT_TYPE) -> None:
    """Handle /start command."""
    assert update.effective_user is not None
    await update.message.reply_text(
        f"Hello, {update.effective_user.first_name}! I'm a Telegram bot."
    )


async def help_command(
    update: Update, context: ContextTypes.DEFAULT_TYPE,
) -> None:
    """Handle /help command."""
    await update.message.reply_text(
        "Available commands:\n/start - Greeting\n/help - This message\n"
        "Send any text and I'll echo it!"
    )


async def echo(update: Update, context: ContextTypes.DEFAULT_TYPE) -> None:
    """Echo user messages."""
    assert update.message is not None and update.message.text is not None
    await update.message.reply_text(f"You said: {update.message.text}")


def main() -> None:
    """Start the bot."""
    token = os.getenv("TELEGRAM_TOKEN")
    if not token:
        raise ValueError("TELEGRAM_TOKEN environment variable is not set")

    app = Application.builder().token(token).build()
    app.add_handler(CommandHandler("start", start))
    app.add_handler(CommandHandler("help", help_command))
    app.add_handler(MessageHandler(filters.TEXT & ~filters.COMMAND, echo))

    print("Bot is running...")
    app.run_polling(allowed_updates=Update.ALL_TYPES)


if __name__ == "__main__":
    main()
