// Say command — repeats the user's message in the channel

import { SlashCommandBuilder } from 'discord.js';

export const data = new SlashCommandBuilder()
  .setName('say')
  .setDescription('Makes the bot send a message')
  .addStringOption(option =>
    option.setName('message')
      .setDescription('The message to send')
      .setRequired(true));

/** Executes the /say command: privately confirms then publicly repeats the message */
export async function execute(interaction) {
  const message = interaction.options.getString('message');
  await interaction.reply({ content: `You said: ${message}`, ephemeral: true });
  await interaction.channel.send(message);
}

