// Slash command "ping" — responds with bot latency and API latency in an embedded message
import { SlashCommandBuilder, EmbedBuilder, type CommandInteraction } from 'discord.js';

export const data = new SlashCommandBuilder()
  .setName('ping')
  .setDescription('Antwortet mit Pong!');

// Execute the command: calculate round-trip latency and display in an embed
export async function execute(interaction: CommandInteraction): Promise<void> {
  const sent = await interaction.reply({ content: 'Pong!', fetchReply: true });
  const latency = sent.createdTimestamp - interaction.createdTimestamp;

  const embed = new EmbedBuilder()
    .setTitle('🏓 Pong!')
    .addFields(
      { name: 'Latenz / Latency', value: `${latency}ms`, inline: true },
      { name: 'API Latency', value: `${interaction.client.ws.ping}ms`, inline: true },
    )
    .setColor(0x44aaff)
    .setTimestamp();

  await interaction.editReply({ content: null, embeds: [embed] });
}
