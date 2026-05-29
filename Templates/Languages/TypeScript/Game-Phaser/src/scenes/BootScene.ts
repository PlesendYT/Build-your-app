// Boot/loading scene — placeholder for asset preloading before launching the game
import Phaser from 'phaser';

export class BootScene extends Phaser.Scene {
  constructor() {
    super('Boot');
  }

  preload(): void {
    // Preload assets here (e.g. this.load.image('player', 'assets/player.png'))
  }

  // Transition to the main game scene once assets are loaded
  create(): void {
    this.scene.start('Game');
  }
}
