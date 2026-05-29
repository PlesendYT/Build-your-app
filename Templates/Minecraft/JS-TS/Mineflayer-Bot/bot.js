/**
 * Main Mineflayer bot script.
 * Connects to a Minecraft server and provides auto-farming and chat commands.
 */
const mineflayer = require('mineflayer');
const config = require('./config');

// Create the bot instance with settings from config.js
const bot = mineflayer.createBot({
  host: config.host,
  port: config.port,
  username: config.username,
  auth: config.auth,
});

/**
 * Logs connection events to the console.
 */
bot.on('login', () => {
  console.log(`Bot logged in as ${bot.username}`);
  bot.chat('/welcome');  // Sends a command on join
});

/**
 * Handles incoming chat messages for basic commands.
 * Listens for "!ping" and "!time" commands from other players.
 */
bot.on('chat', (username, message) => {
  // Ignore messages from the bot itself
  if (username === bot.username) return;

  if (message === '!ping') {
    bot.chat(`Pong! My ping is ${bot.player?.ping}ms`);
  }

  if (message === '!time') {
    const time = bot.time.timeOfDay;
    bot.chat(`The current in-game time is ${time}`);
  }
});

/**
 * Auto-farming logic — harvests mature crops within the configured radius.
 * Runs every farmInterval milliseconds.
 */
async function autoFarm() {
  if (!config.autoFarm) return;

  // Find all mature wheat crops within the configured radius
  const crops = bot.findBlocks({
    matching: block => block.name === 'wheat' && block.properties.age === 7,
    maxDistance: config.farmRadius,
    count: 10,
  });

  for (const pos of crops) {
    // Break the mature crop and collect the drops
    await bot.dig(bot.blockAt(pos));
  }

  console.log(`Auto-farmed ${crops.length} crops`);
}

// Start the auto-farm loop at the configured interval
setInterval(autoFarm, config.farmInterval);

/**
 * Handles inventory management: eats food when below threshold.
 */
bot.on('health', () => {
  if (bot.food < config.minFoodCount) {
    // Find food items in the hotbar and eat them
    const food = bot.inventory.items().find(item => item.foodPoints > 0);
    if (food) {
      bot.equip(food, 'hand').then(() => bot.consume());
    }
  }
});
