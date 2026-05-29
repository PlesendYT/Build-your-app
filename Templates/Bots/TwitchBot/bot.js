// Twitch chat bot template using tmi.js
// Listens for chat commands prefixed with "!" in a Twitch channel

const tmi = require("tmi.js");
const config = require("./config");

// Create the tmi.js client with configured identity and channels
const client = new tmi.Client({
  identity: { username: config.username, password: config.password },
  channels: config.channels,
});

// Connect to Twitch IRC
client.connect();

// Log successful connection
client.on("connected", (addr, port) => {
  console.log(`Connected to ${addr}:${port} as ${config.username}`);
});

// Handle incoming chat messages with command routing
client.on("message", (channel, tags, message, self) => {
  // Ignore messages sent by the bot itself
  if (self) return;

  const body = message.trim();
  if (!body.startsWith("!")) return;

  // Parse command and arguments
  const [command, ...args] = body.slice(1).split(" ");
  const target = channel;

  switch (command) {
    case "ping":
      client.say(target, `@${tags.username} Pong!`);
      break;
    case "uptime":
      client.say(target, `@${tags.username} Bot is running!`);
      break;
    case "echo":
      client.say(target, `@${tags.username} ${args.join(" ")}`);
      break;
    case "help":
      client.say(target, "Commands: !ping, !uptime, !echo <text>, !help");
      break;
  }
});
