// Game-Phaser — Hauptspiel

import Phaser from 'phaser';

// ── Boot-Szene ──

class BootScene extends Phaser.Scene {
  constructor() { super('Boot'); }

  preload() {
    // Hier könnten Assets geladen werden
    // this.load.image('player', 'assets/player.png');
  }

  create() {
    this.scene.start('Game');
  }
}

// ── Game-Szene ──

class GameScene extends Phaser.Scene {
  constructor() { super('Game'); }

  create() {
    this.score = 0;
    this.scoreText = this.add.text(16, 16, 'Score: 0', { fontSize: '24px', fill: '#fff' });

    // Spieler
    this.player = this.add.rectangle(400, 500, 32, 32, 0x44aaff);
    this.physics.add.existing(this.player, false);
    this.player.body.setCollideWorldBounds(true);

    // Tastatur
    this.cursors = this.input.keyboard.createCursorKeys();

    // Gegner-Gruppe
    this.enemies = this.physics.add.group();
    this.coins = this.physics.add.group();

    // Gegner spawnen
    this.time.addEvent({
      delay: 1500,
      callback: this.spawnEnemy,
      callbackScope: this,
      loop: true,
    });

    // Münzen spawnen
    this.time.addEvent({
      delay: 800,
      callback: this.spawnCoin,
      callbackScope: this,
      loop: true,
    });

    // Kollisionen
    this.physics.add.overlap(this.player, this.enemies, this.hitEnemy, null, this);
    this.physics.add.overlap(this.player, this.coins, this.collectCoin, null, this);
  }

  update() {
    this.player.body.setVelocity(0);

    if (this.cursors.left.isDown)  this.player.body.setVelocityX(-200);
    if (this.cursors.right.isDown) this.player.body.setVelocityX(200);
    if (this.cursors.up.isDown)    this.player.body.setVelocityY(-200);
    if (this.cursors.down.isDown)  this.player.body.setVelocityY(200);
  }

  spawnEnemy() {
    const x = Phaser.Math.Between(20, 780);
    const enemy = this.add.rectangle(x, -20, 24, 24, 0xff4444);
    this.physics.add.existing(enemy, false);
    enemy.body.setVelocityY(Phaser.Math.Between(100, 200));
    this.enemies.add(enemy);
  }

  spawnCoin() {
    const x = Phaser.Math.Between(20, 780);
    const coin = this.add.rectangle(x, Phaser.Math.Between(20, 580), 16, 16, 0xffd700);
    this.physics.add.existing(coin, true);
    this.coins.add(coin);
  }

  hitEnemy(_player, _enemy) {
    this.scene.start('GameOver');
  }

  collectCoin(_player, coin) {
    coin.destroy();
    this.score += 10;
    this.scoreText.setText(`Score: ${this.score}`);
  }
}

// ── GameOver-Szene ──

class GameOverScene extends Phaser.Scene {
  constructor() { super('GameOver'); }

  create() {
    this.add.text(400, 250, 'Game Over', { fontSize: '48px', fill: '#fff' }).setOrigin(0.5);
    this.add.text(400, 320, 'Klicke zum Neustart', { fontSize: '24px', fill: '#aaa' }).setOrigin(0.5);

    this.input.once('pointerdown', () => {
      this.scene.start('Game');
    });
  }
}

// ── Konfiguration ──

const config = {
  type: Phaser.AUTO,
  width: 800,
  height: 600,
  backgroundColor: '#1a1a2e',
  parent: 'game',
  physics: {
    default: 'arcade',
    arcade: { gravity: { y: 0 }, debug: false },
  },
  scene: [BootScene, GameScene, GameOverScene],
};

new Phaser.Game(config);
