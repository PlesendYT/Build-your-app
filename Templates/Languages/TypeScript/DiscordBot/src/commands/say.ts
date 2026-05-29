// Slash command "say" — echoes the user's message both ephemerally and in the channel
import { SlashCommandBuilder, type CommandInteraction, type TextChannel } from 'discord.js';

export const data = new SlashCommandBuilder()
  .setName('say')
  .setDescription('Lässt den Bot eine Nachricht senden')
  .addStringOption(option =>
    option.setName('message')
      .setDescription('Die Nachricht')
      .setRequired(true));

// Execute the command: reply privately and then send the message to the channel
export async function execute(interaction: CommandInteraction): Promise<void> {
  const message = interaction.options.get('message', true).value as string;
  await interaction.reply({ content: `Du sagst: ${message}`, ephemeral: true });
  await (interaction.channel as TextChannel).send(message);
}
