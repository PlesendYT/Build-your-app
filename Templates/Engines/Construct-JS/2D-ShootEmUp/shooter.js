// Shoot-em-up Core - Construct 3 Script

const PLAYER_SPEED = 5;
const BULLET_SPEED = 800;
const FIRE_RATE = 0.15;

let playerHP = 3;
let score = 0;
let fireTimer = 0;
let powerLevel = 1;
let isGameOver = false;

export function initShooter() {
    const player = runtime.objects.PlayerShip.getFirstInstance();
    if (player) {
        player.x = runtime.layout.width / 2;
        player.y = runtime.layout.height - 80;
    }
    playerHP = 3;
    score = 0;
    powerLevel = 1;
    isGameOver = false;
    fireTimer = 0;
}

export function handleShooterInput(dt) {
    if (isGameOver) return;

    const player = runtime.objects.PlayerShip.getFirstInstance();
    if (!player) return;

    // Mouse/touch follow
    const touch = runtime.touch;
    if (touch.isTouching) {
        const tx = touch.getTouchX(0);
        const ty = touch.getTouchY(0);
        player.x += (tx - player.x) * 0.1;
        player.y += (ty - player.y) * 0.1;
    }

    // Keyboard
    if (runtime.keyboard.isKeyDown("Left") || runtime.keyboard.isKeyDown("A")) player.x -= PLAYER_SPEED;
    if (runtime.keyboard.isKeyDown("Right") || runtime.keyboard.isKeyDown("D")) player.x += PLAYER_SPEED;
    if (runtime.keyboard.isKeyDown("Up") || runtime.keyboard.isKeyDown("W")) player.y -= PLAYER_SPEED;
    if (runtime.keyboard.isKeyDown("Down") || runtime.keyboard.isKeyDown("S")) player.y += PLAYER_SPEED;

    // Clamp to screen
    player.x = Math.max(32, Math.min(runtime.layout.width - 32, player.x));
    player.y = Math.max(32, Math.min(runtime.layout.height - 32, player.y));

    // Auto-fire
    fireTimer -= dt;
    if (fireTimer <= 0) {
        fireBullets(player);
        fireTimer = FIRE_RATE;
    }
}

function fireBullets(player) {
    if (powerLevel === 1) {
        const b = runtime.objects.Bullet.createInstance(0, player.x, player.y - 24);
        if (b) b.bulletVy = -BULLET_SPEED;
    } else if (powerLevel === 2) {
        const b1 = runtime.objects.Bullet.createInstance(0, player.x - 12, player.y - 20);
        if (b1) b1.bulletVy = -BULLET_SPEED;
        const b2 = runtime.objects.Bullet.createInstance(0, player.x + 12, player.y - 20);
        if (b2) b2.bulletVy = -BULLET_SPEED;
    } else {
        for (let i = -1; i <= 1; i++) {
            const b = runtime.objects.Bullet.createInstance(0, player.x + i * 16, player.y - 24);
            if (b) {
                b.bulletVy = -BULLET_SPEED;
                b.bulletVx = i * 40;
            }
        }
    }
}

export function updateBullets(dt) {
    runtime.objects.Bullet.getAllInstances().forEach(bullet => {
        bullet.y += (bullet.bulletVy || -BULLET_SPEED) * dt;
        bullet.x += (bullet.bulletVx || 0) * dt;

        if (bullet.y < -32 || bullet.y > runtime.layout.height + 32 ||
            bullet.x < -32 || bullet.x > runtime.layout.width + 32) {
            bullet.destroy();
        }

        // Check enemy collision
        runtime.objects.Enemy.getAllInstances().forEach(enemy => {
            const dist = Math.sqrt(
                Math.pow(bullet.x - enemy.x, 2) + Math.pow(bullet.y - enemy.y, 2)
            );
            if (dist < 32) {
                enemy.enemyHP = (enemy.enemyHP || 1) - 1;
                bullet.destroy();
                if (enemy.enemyHP <= 0) {
                    score += 100;
                    enemy.destroy();
                    spawnExplosion(enemy.x, enemy.y);
                }
            }
        });
    });
}

function spawnExplosion(x, y) {
    const explosion = runtime.objects.Explosion.createInstance(0, x, y);
    if (explosion) {
        setTimeout(() => explosion.destroy(), 500);
    }
}

export function spawnEnemyWave() {
    const types = ["basic", "fast", "tank"];
    const count = 3 + Math.floor(Math.random() * 3);

    for (let i = 0; i < count; i++) {
        const type = types[Math.floor(Math.random() * types.length)];
        const ex = Math.random() * (runtime.layout.width - 64) + 32;
        const enemy = runtime.objects.Enemy.createInstance(0, ex, -32);

        if (enemy) {
            enemy.enemyType = type;
            enemy.enemyHP = type === "tank" ? 3 : type === "fast" ? 1 : 2;
            enemy.enemySpeed = type === "fast" ? 200 : type === "tank" ? 60 : 120;
            enemy.enemyScore = type === "tank" ? 300 : type === "fast" ? 150 : 100;
        }
    }
}

export function updateEnemies(dt) {
    runtime.objects.Enemy.getAllInstances().forEach(enemy => {
        enemy.y += (enemy.enemySpeed || 120) * dt;

        // Simple movement pattern
        enemy.x += Math.sin(enemy.y * 0.01) * 60 * dt;

        if (enemy.y > runtime.layout.height + 64) {
            enemy.destroy();
        }

        // Check collision with player
        const player = runtime.objects.PlayerShip.getFirstInstance();
        if (player) {
            const dist = Math.sqrt(
                Math.pow(enemy.x - player.x, 2) + Math.pow(enemy.y - player.y, 2)
            );
            if (dist < 32) {
                playerHP--;
                enemy.destroy();
                runtime.callFunction("UpdateHealth", playerHP);
                if (playerHP <= 0) {
                    isGameOver = true;
                    runtime.callFunction("DisplayMessage", `Game Over! Score: ${score}`);
                }
            }
        }
    });
}

// Wave timer
let waveTimer = 0;
runtime.addEventListener("tick", () => {
    const dt = runtime.dt;
    if (isGameOver) return;

    handleShooterInput(dt);
    updateBullets(dt);
    updateEnemies(dt);

    waveTimer -= dt;
    if (waveTimer <= 0) {
        spawnEnemyWave();
        waveTimer = 3 + Math.random() * 2;
    }
});

runtime.addEventListener("beforeprojectstart", () => {
    initShooter();
    waveTimer = 2;
});
