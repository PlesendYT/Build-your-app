// Interaction create event — handles all slash command interactions

import { Events } from 'discord.js';

export const name = Events.InteractionCreate;

/** Processes incoming interactions: routes to the matching command handler */
export async function execute(interaction) {
  if (!interaction.isChatInputCommand()) return;

  const command = interaction.client.commands.get(interaction.commandName);
  if (!command) return;

  try {
    await command.execute(interaction);
  } catch (err) {
    console.error(err);
    const reply = { content: 'An error occurred.', ephemeral: true };
    if (interaction.replied || interaction.deferred) {
      await interaction.followUp(reply);
    } else {
      await interaction.reply(reply);
    }
  }
}
