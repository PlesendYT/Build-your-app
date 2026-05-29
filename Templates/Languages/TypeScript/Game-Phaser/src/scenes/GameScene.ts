// Main gameplay scene — player movement, enemy spawning, and coin collection with scoring
import Phaser from 'phaser';

export class GameScene extends Phaser.Scene {
  private player!: Phaser.GameObjects.Rectangle;
  private cursors!: Phaser.Types.Input.Keyboard.CursorKeys;
  private enemies!: Phaser.Physics.Arcade.Group;
  private coins!: Phaser.Physics.Arcade.Group;
  private score = 0;
  private scoreText!: Phaser.GameObjects.Text;

  constructor() {
    super('Game');
  }

  // Initialise scene objects, physics, and timed spawners
  create(): void {
    this.score = 0;
    this.scoreText = this.add.text(16, 16, 'Score: 0', { fontSize: '24px', color: '#fff' });

    this.player = this.add.rectangle(400, 500, 32, 32, 0x44aaff);
    this.physics.add.existing(this.player, false);
    (this.player.body as Phaser.Physics.Arcade.Body).setCollideWorldBounds(true);

    this.cursors = this.input.keyboard!.createCursorKeys();
    this.enemies = this.physics.add.group();
    this.coins = this.physics.add.group();

    // Spawn enemies every 1.5 seconds
    this.time.addEvent({
      delay: 1500,
      callback: this.spawnEnemy,
      callbackScope: this,
      loop: true,
    });

    // Spawn coins every 0.8 seconds
    this.time.addEvent({
      delay: 800,
      callback: this.spawnCoin,
      callbackScope: this,
      loop: true,
    });

    // Register overlap callbacks for collisions
    this.physics.add.overlap(this.player, this.enemies, this.hitEnemy, undefined, this);
    this.physics.add.overlap(this.player, this.coins, this.collectCoin, undefined, this);
  }

  // Per-frame update — handle player movement via cursor keys
  update(): void {
    const body = this.player.body as Phaser.Physics.Arcade.Body;
    body.setVelocity(0);

    if (this.cursors.left.isDown)  body.setVelocityX(-200);
    if (this.cursors.right.isDown) body.setVelocityX(200);
    if (this.cursors.up.isDown)    body.setVelocityY(-200);
    if (this.cursors.down.isDown)  body.setVelocityY(200);
  }

  // Spawn a red enemy rectangle at a random X position, falling downward
  private spawnEnemy(): void {
    const x = Phaser.Math.Between(20, 780);
    const enemy = this.add.rectangle(x, -20, 24, 24, 0xff4444);
    this.physics.add.existing(enemy, false);
    (enemy.body as Phaser.Physics.Arcade.Body).setVelocityY(Phaser.Math.Between(100, 200));
    this.enemies.add(enemy);
  }

  // Spawn a gold coin rectangle at a random position
  private spawnCoin(): void {
    const x = Phaser.Math.Between(20, 780);
    const coin = this.add.rectangle(x, Phaser.Math.Between(20, 580), 16, 16, 0xffd700);
    this.physics.add.existing(coin, true);
    this.coins.add(coin);
  }

  // Called when the player overlaps an enemy — restart the scene
  private hitEnemy(
    _player: Phaser.Types.Physics.Arcade.GameObjectWithBody | Phaser.Tilemaps.Tile,
    _enemy: Phaser.Types.Physics.Arcade.GameObjectWithBody | Phaser.Tilemaps.Tile
  ): void {
    this.scene.start('Game');
  }

  // Called when the player overlaps a coin — destroy it and increase score
  private collectCoin(
    _player: Phaser.Types.Physics.Arcade.GameObjectWithBody | Phaser.Tilemaps.Tile,
    coin: Phaser.Types.Physics.Arcade.GameObjectWithBody | Phaser.Tilemaps.Tile
  ): void {
    (coin as Phaser.GameObjects.GameObject).destroy();
    this.score += 10;
    this.scoreText.setText(`Score: ${this.score}`);
  }
}
