import { SlashCommandBuilder } from 'discord.js';

export const data = new SlashCommandBuilder()
  .setName('say')
  .setDescription('Lässt den Bot eine Nachricht senden')
  .addStringOption(option =>
    option.setName('message')
      .setDescription('Die Nachricht')
      .setRequired(true));

export async function execute(interaction) {
  const message = interaction.options.getString('message');
  await interaction.reply({ content: `Du sagst: ${message}`, ephemeral: true });
  await interaction.channel.send(message);
}
