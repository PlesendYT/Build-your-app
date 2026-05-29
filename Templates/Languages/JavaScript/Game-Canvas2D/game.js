// Game-Canvas2D — simple 2D canvas game
// Player moves with arrow keys, collects coins, avoids falling enemies

const canvas = document.getElementById('game');
const ctx = canvas.getContext('2d');
const scoreSpan = document.getElementById('score');
const livesSpan = document.getElementById('lives');

const W = 640, H = 480;

// Player properties
const player = {
  x: W / 2, y: H - 60,
  w: 32, h: 32,
  speed: 4,
  lives: 3,
};

// Coin array and spawn settings
const coins = [];
const COIN_RADIUS = 8;
const COIN_SPAWN_INTERVAL = 60;
let coinTimer = 0;
let score = 0;

// Enemy array and spawn settings
const enemies = [];
const ENEMY_SPAWN_INTERVAL = 120;
let enemyTimer = 0;

// Keyboard state tracking
const keys = {};

document.addEventListener('keydown', e => { keys[e.key] = true; });
document.addEventListener('keyup', e => { keys[e.key] = false; });

/** Checks axis-aligned bounding box collision between two rectangles */
function rectCollide(a, b) {
  return a.x < b.x + b.w && a.x + a.w > b.x &&
         a.y < b.y + b.h && a.y + a.h > b.y;
}

/** Calculates Euclidean distance between two points */
function dist(x1, y1, x2, y2) {
  return Math.hypot(x2 - x1, y2 - y1);
}

/** Updates game state: movement, spawning, collision detection */
function update() {
  // Player movement via arrow keys or WASD
  if (keys['ArrowLeft'] || keys['a']) player.x -= player.speed;
  if (keys['ArrowRight'] || keys['d']) player.x += player.speed;
  if (keys['ArrowUp'] || keys['w']) player.y -= player.speed;
  if (keys['ArrowDown'] || keys['s']) player.y += player.speed;

  // Clamp player within canvas bounds
  player.x = Math.max(0, Math.min(W - player.w, player.x));
  player.y = Math.max(0, Math.min(H - player.h, player.y));

  // Spawn coins at intervals
  coinTimer++;
  if (coinTimer >= COIN_SPAWN_INTERVAL) {
    coinTimer = 0;
    coins.push({
      x: Math.random() * (W - 2 * COIN_RADIUS) + COIN_RADIUS,
      y: Math.random() * (H - 2 * COIN_RADIUS) + COIN_RADIUS,
    });
  }

  // Spawn enemies at intervals
  enemyTimer++;
  if (enemyTimer >= ENEMY_SPAWN_INTERVAL) {
    enemyTimer = 0;
    enemies.push({
      x: Math.random() * (W - 24),
      y: -24,
      w: 24, h: 24,
      speed: 1 + Math.random() * 2,
    });
  }

  // Coin collection — check distance from player center
  for (let i = coins.length - 1; i >= 0; i--) {
    if (dist(player.x + player.w / 2, player.y + player.h / 2, coins[i].x, coins[i].y) < COIN_RADIUS + 16) {
      coins.splice(i, 1);
      score += 10;
      scoreSpan.textContent = score;
    }
  }

  // Enemy movement and collision
  for (let i = enemies.length - 1; i >= 0; i--) {
    enemies[i].y += enemies[i].speed;
    if (enemies[i].y > H) {
      enemies.splice(i, 1);
      continue;
    }
    if (rectCollide(player, enemies[i])) {
      player.lives--;
      livesSpan.textContent = player.lives;
      enemies.splice(i, 1);
      if (player.lives <= 0) {
        gameOver();
        return;
      }
    }
  }
}

/** Renders all game objects to the canvas */
function draw() {
  ctx.clearRect(0, 0, W, H);

  // Draw coins
  ctx.fillStyle = '#ffd700';
  coins.forEach(c => {
    ctx.beginPath();
    ctx.arc(c.x, c.y, COIN_RADIUS, 0, Math.PI * 2);
    ctx.fill();
  });

  // Draw enemies
  ctx.fillStyle = '#ff4444';
  enemies.forEach(e => {
    ctx.fillRect(e.x, e.y, e.w, e.h);
  });

  // Draw player
  ctx.fillStyle = '#44aaff';
  ctx.fillRect(player.x, player.y, player.w, player.h);
}

let running = true;

/** Displays the game over overlay and stops the game loop */
function gameOver() {
  running = false;
  ctx.fillStyle = 'rgba(0,0,0,0.7)';
  ctx.fillRect(0, 0, W, H);
  ctx.fillStyle = '#fff';
  ctx.font = '48px system-ui';
  ctx.textAlign = 'center';
  ctx.fillText('Game Over', W / 2, H / 2);
  ctx.font = '24px system-ui';
  ctx.fillText(`Score: ${score}`, W / 2, H / 2 + 50);
  ctx.fillText('Reload to restart', W / 2, H / 2 + 90);
}

/** Main game loop — runs update and draw on every animation frame */
function gameLoop() {
  if (running) {
    update();
    draw();
    requestAnimationFrame(gameLoop);
  }
}

// Start the game
gameLoop();
