// Discord bot entry point — initialises client, loads commands and events, then logs in
import { Client, GatewayIntentBits, Events, Collection, type RESTPostAPIChatInputApplicationCommandsJSONBody } from 'discord.js';
import { readdirSync } from 'node:fs';
import { join, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

// Shape of a loaded command module
interface BotCommand {
  data: RESTPostAPIChatInputApplicationCommandsJSONBody & { name: string };
  execute: (...args: unknown[]) => Promise<void>;
}

// Create the Discord client with required intents
const client = new Client({
  intents: [
    GatewayIntentBits.Guilds,
    GatewayIntentBits.GuildMessages,
    GatewayIntentBits.MessageContent,
  ],
});

client.commands = new Collection<string, BotCommand>();

// Dynamically load all command files from the commands directory
const commandsPath = join(__dirname, 'commands');
const commandFiles = readdirSync(commandsPath).filter(f => f.endsWith('.ts') || f.endsWith('.js'));

for (const file of commandFiles) {
  const command: BotCommand = await import(`./commands/${file}`);
  client.commands.set(command.data.name, command);
}

// Dynamically load all event files from the events directory
const eventsPath = join(__dirname, 'events');
const eventFiles = readdirSync(eventsPath).filter(f => f.endsWith('.ts') || f.endsWith('.js'));

for (const file of eventFiles) {
  const event = await import(`./events/${file}`);
  if (event.once) {
    client.once(event.name, (...args: unknown[]) => event.execute(...args));
  } else {
    client.on(event.name, (...args: unknown[]) => event.execute(...args));
  }
}

// Register slash commands globally once the client is ready
client.once(Events.ClientReady, async () => {
  const commands = client.commands.map(c => c.data);
  await client.application!.commands.set(commands);
  console.log(`Bot online als ${client.user!.tag}`);
});

// Log in using the token from environment variables
client.login(process.env.DISCORD_TOKEN);
