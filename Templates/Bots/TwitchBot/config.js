// Twitch IRC bot configuration
// Set these values in environment variables or a .env file (never commit real credentials)

module.exports = {
  username: process.env.TWITCH_USERNAME || "YOUR_TWITCH_USERNAME",
  password: process.env.TWITCH_OAUTH_TOKEN || "oauth:YOUR_OAUTH_TOKEN",
  channels: [process.env.TWITCH_CHANNEL || "target_channel"],
};
