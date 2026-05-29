/**
 * Main Bedrock Script API addon script.
 * Listens for player events and handles block interactions.
 */
import { world, system, Player } from '@minecraft/server';
import { ActionFormData } from '@minecraft/server-ui';

/**
 * Sends a welcome message when a player joins the world.
 */
world.afterEvents.playerSpawn.subscribe((event) => {
  const player = event.player;
  player.sendMessage(`Welcome to the world, ${player.name}!`);
});

/**
 * Handles block break events — logs the broken block type.
 */
world.afterEvents.playerBreakBlock.subscribe((event) => {
  const { player, block } = event;
  console.warn(`${player.name} broke ${block.typeId} at ${block.location}`);
});

/**
 * Shows a simple interactive menu when a player interacts with a chest.
 */
world.afterEvents.playerInteractWithBlock.subscribe((event) => {
  const { player, block } = event;

  // Only trigger on chest blocks
  if (!block.typeId.includes('chest')) return;

  const menu = new ActionFormData()
    .title('Chest Menu')
    .body('What would you like to do?')
    .button('Open Inventory')
    .button('Cancel');

  menu.show(player).then((response) => {
    if (response.selection === 0) {
      // Open the chest inventory
      player.sendMessage('Opening chest...');
    }
  });
});

/**
 * Every 20 server ticks (1 second), heal nearby players slightly.
 */
system.runInterval(() => {
  for (const player of world.getPlayers()) {
    if (player.getComponent('health')?.currentValue < 20) {
      player.applyDamage(-1);  // Negative damage = healing
    }
  }
}, 20);
