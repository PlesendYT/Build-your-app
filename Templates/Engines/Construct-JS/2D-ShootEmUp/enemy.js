// Enemy Types & Patterns - Construct 3 Script

const enemyPatterns = {
    basic: {
        moveAngle: 90,
        speed: 120,
        hp: 1,
        fireRate: 0,
        score: 100
    },
    fast: {
        moveAngle: 90,
        speed: 220,
        hp: 1,
        fireRate: 0,
        score: 150
    },
    tank: {
        moveAngle: 90,
        speed: 60,
        hp: 3,
        fireRate: 2,
        score: 300
    },
    shooter: {
        moveAngle: 90,
        speed: 80,
        hp: 2,
        fireRate: 1.5,
        score: 200
    }
};

export function getEnemyConfig(type) {
    return enemyPatterns[type] || enemyPatterns.basic;
}

export function enemyFireAtPlayer(enemy) {
    const pattern = getEnemyConfig(enemy.enemyType || "basic");
    if (!pattern.fireRate || pattern.fireRate <= 0) return null;

    const player = runtime.objects.PlayerShip.getFirstInstance();
    if (!player) return null;

    const angle = Math.atan2(player.y - enemy.y, player.x - enemy.x);
    const bullet = runtime.objects.EnemyBullet.createInstance(0, enemy.x, enemy.y + 16);
    if (bullet) {
        bullet.bulletVx = Math.cos(angle) * 250;
        bullet.bulletVy = Math.sin(angle) * 250;
    }
    return bullet;
}

// Enemy bullet update
export function updateEnemyBullets(dt) {
    runtime.objects.EnemyBullet.getAllInstances().forEach(bullet => {
        bullet.x += (bullet.bulletVx || 0) * dt;
        bullet.y += (bullet.bulletVy || 0) * dt;

        if (bullet.x < -32 || bullet.x > runtime.layout.width + 32 ||
            bullet.y < -32 || bullet.y > runtime.layout.height + 32) {
            bullet.destroy();
        }
    });
}

// Power-up spawn
export function spawnPowerUp(x, y) {
    const types = ["speed", "spread", "shield"];
    const type = types[Math.floor(Math.random() * types.length)];
    const pu = runtime.objects.PowerUp.createInstance(0, x, y);
    if (pu) {
        pu.powerUpType = type;
    }
}

runtime.addEventListener("tick", () => {
    updateEnemyBullets(runtime.dt);
});
