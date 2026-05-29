/**
 * Bot configuration settings.
 * Modify these values before starting the bot.
 */

// Server connection settings
module.exports = {
  host: 'localhost',       // Minecraft server IP or hostname
  port: 25565,             // Server port (default: 25565)
  username: 'FarmBot',     // Bot's in-game name
  auth: 'offline',         // 'offline' for cracked, 'microsoft' for premium

  // Auto-farm settings
  autoFarm: true,          // Enable automatic crop harvesting
  farmRadius: 5,           // Radius in blocks to search for crops
  farmInterval: 10000,     // Milliseconds between farm cycles

  // Inventory thresholds
  minFoodCount: 16,        // Minimum food before auto-eating
  maxToolDurability: 100,  // Minimum durability before tool replacement
};
