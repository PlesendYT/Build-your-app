// Ready event — fires once when the bot successfully logs in

import { Events } from 'discord.js';

export const name = Events.ClientReady;
export const once = true;

/** Called when the Discord client is ready and logged in */
export function execute(client) {
  console.log(`Ready! Logged in as ${client.user.tag}`);
}
