import { Client, GatewayIntentBits, Events, Collection, type RESTPostAPIChatInputApplicationCommandsJSONBody } from 'discord.js';
import { readdirSync } from 'node:fs';
import { join, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

interface BotCommand {
  data: RESTPostAPIChatInputApplicationCommandsJSONBody & { name: string };
  execute: (...args: unknown[]) => Promise<void>;
}

const client = new Client({
  intents: [
    GatewayIntentBits.Guilds,
    GatewayIntentBits.GuildMessages,
    GatewayIntentBits.MessageContent,
  ],
});

client.commands = new Collection<string, BotCommand>();

const commandsPath = join(__dirname, 'commands');
const commandFiles = readdirSync(commandsPath).filter(f => f.endsWith('.ts') || f.endsWith('.js'));

for (const file of commandFiles) {
  const command: BotCommand = await import(`./commands/${file}`);
  client.commands.set(command.data.name, command);
}

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

client.once(Events.ClientReady, async () => {
  const commands = client.commands.map(c => c.data);
  await client.application!.commands.set(commands);
  console.log(`Bot online als ${client.user!.tag}`);
});

client.login(process.env.DISCORD_TOKEN);
