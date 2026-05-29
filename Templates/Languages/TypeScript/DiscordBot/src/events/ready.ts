// Ready event — logs when the bot comes online
import { Events, type Client } from 'discord.js';

export const name = Events.ClientReady;
export const once = true;

export function execute(client: Client): void {
  console.log(`Ready! Eingeloggt als ${client.user!.tag}`);
}
