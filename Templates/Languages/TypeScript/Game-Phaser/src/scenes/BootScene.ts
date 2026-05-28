import Phaser from 'phaser';

export class BootScene extends Phaser.Scene {
  constructor() {
    super('Boot');
  }

  preload(): void {
    // Assets laden (z.B. this.load.image('player', 'assets/player.png'))
  }

  create(): void {
    this.scene.start('Game');
  }
}
