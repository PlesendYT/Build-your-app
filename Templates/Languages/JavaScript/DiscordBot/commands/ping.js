// Ping command — responds with Pong! and displays bot/client latency

import { SlashCommandBuilder, EmbedBuilder } from 'discord.js';

export const data = new SlashCommandBuilder()
  .setName('ping')
  .setDescription('Replies with Pong!');

/** Executes the /ping command: measures latency and responds with an embed */
export async function execute(interaction) {
  const sent = await interaction.reply({ content: 'Pong!', fetchReply: true });
  const latency = sent.createdTimestamp - interaction.createdTimestamp;

  const embed = new EmbedBuilder()
    .setTitle('🏓 Pong!')
    .addFields(
      { name: 'Latency', value: `${latency}ms`, inline: true },
      { name: 'API Latency', value: `${interaction.client.ws.ping}ms`, inline: true },
    )
    .setColor(0x44aaff)
    .setTimestamp();

  await interaction.editReply({ content: null, embeds: [embed] });
}
