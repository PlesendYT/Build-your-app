// WhatsApp bot template using whatsapp-web.js
// Scans QR code once, then parses commands like !ping, !help

const { Client, LocalAuth } = require("whatsapp-web.js");
const qrcode = require("qrcode-terminal");
const config = require("./config");

// Create client with persistent local authentication
const client = new Client({
  authStrategy: new LocalAuth({ dataPath: config.sessionPath }),
});

// Show QR code in terminal for initial authentication
client.on("qr", (qr) => {
  qrcode.generate(qr, { small: true });
});

// Notify when the client is ready
client.on("ready", () => {
  console.log("WhatsApp bot is ready!");
});

// Parse incoming messages for commands prefixed with "!"
client.on("message", async (msg) => {
  // Ignore messages from groups and status broadcasts
  if (msg.from.includes("@g.us") || msg.from === "status@broadcast") return;

  const body = msg.body.trim();

  // Simple command router using the first token
  const [command, ...args] = body.split(" ");
  switch (command) {
    case "!ping":
      await msg.reply("Pong!");
      break;
    case "!help":
      await msg.reply("Commands: !ping, !help, !echo <text>");
      break;
    case "!echo":
      await msg.reply(args.join(" ") || "Say something!");
      break;
  }
});

client.initialize();
