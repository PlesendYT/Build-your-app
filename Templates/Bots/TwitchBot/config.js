// Twitch IRC bot configuration
// Get OAuth token at https://twitchapps.com/tmi

module.exports = {
  // Your Twitch username
  username: "YOUR_TWITCH_USERNAME",
  // OAuth token from twitchapps.com/tmi (include the "oauth:" prefix)
  password: "oauth:YOUR_OAUTH_TOKEN",
  // Channel(s) to join (lowercase, without #)
  channels: ["target_channel"],
};
