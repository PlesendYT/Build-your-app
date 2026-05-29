"""
Telegram bot template using python-telegram-bot.
Responds to /start, /help, and shows an inline keyboard on /menu.
"""

from telegram import Update, InlineKeyboardButton, InlineKeyboardMarkup
from telegram.ext import Application, CommandHandler, CallbackQueryHandler, ContextTypes
from config import BOT_TOKEN


async def start(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Send a welcome message when /start is issued."""
    await update.message.reply_text("Hello! I'm a bot. Use /help to see what I can do.")


async def help_command(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """List available commands."""
    await update.message.reply_text("/start - Welcome\n/help - This message\n/menu - Show inline menu")


async def menu(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Show an inline keyboard with option buttons."""
    keyboard = [
        [InlineKeyboardButton("Option A", callback_data="A")],
        [InlineKeyboardButton("Option B", callback_data="B")],
    ]
    reply_markup = InlineKeyboardMarkup(keyboard)
    await update.message.reply_text("Choose an option:", reply_markup=reply_markup)


async def button_callback(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Handle inline keyboard button presses."""
    query = update.callback_query
    await query.answer()
    await query.edit_message_text(text=f"You selected Option {query.data}")


def main():
    """Start the bot polling loop."""
    app = Application.builder().token(BOT_TOKEN).build()
    app.add_handler(CommandHandler("start", start))
    app.add_handler(CommandHandler("help", help_command))
    app.add_handler(CommandHandler("menu", menu))
    app.add_handler(CallbackQueryHandler(button_callback))
    print("Bot is running...")
    app.run_polling()


if __name__ == "__main__":
    main()
