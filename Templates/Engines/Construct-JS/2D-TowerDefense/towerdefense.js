// Tower Defense Core - Construct 3 Script

const TOWER_COST = { cannon: 50, frost: 75, fire: 100 };
const TOWER_RANGE = 150;
const TOWER_FIRE_RATE = { cannon: 0.5, frost: 0.8, fire: 0.6 };

let gold = 200;
let lives = 20;
let wave = 0;
let enemiesThisWave = 0;
let enemiesSpawned = 0;
let waveActive = false;
let waveTimer = 0;

export function initTD() {
    gold = 200;
    lives = 20;
    wave = 0;
    waveActive = false;
    waveTimer = 2;
}

export function placeTower(tileX, tileY, type) {
    const cost = TOWER_COST[type];
    if (!cost || gold < cost) return false;

    gold -= cost;
    const tower = runtime.objects.Tower.createInstance(0, tileX, tileY);
    if (tower) {
        tower.towerType = type;
        tower.towerRange = TOWER_RANGE;
        tower.fireTimer = 0;
        tower.fireRate = TOWER_FIRE_RATE[type] || 0.5;
        tower.towerDamage = type === "fire" ? 25 : type === "frost" ? 15 : 20;
        tower.towerLevel = 1;
    }
    return true;
}

export function upgradeTower(tower) {
    const cost = 50 * tower.towerLevel;
    if (gold < cost || tower.towerLevel >= 3) return false;

    gold -= cost;
    tower.towerLevel++;
    tower.towerDamage += 10;
    tower.towerRange += 20;
    tower.fireRate = Math.max(0.2, tower.fireRate - 0.1);
    return true;
}

export function updateTowers(dt) {
    runtime.objects.Tower.getAllInstances().forEach(tower => {
        tower.fireTimer -= dt;
        if (tower.fireTimer <= 0) {
            // Find target
            let target = null;
            let minDist = tower.towerRange || TOWER_RANGE;

            runtime.objects.EnemyTD.getAllInstances().forEach(enemy => {
                const dist = Math.sqrt(
                    Math.pow(enemy.x - tower.x, 2) + Math.pow(enemy.y - tower.y, 2)
                );
                if (dist < minDist) {
                    minDist = dist;
                    target = enemy;
                }
            });

            if (target) {
                tower.fireTimer = tower.fireRate || 0.5;
                // Fire projectile
                const proj = runtime.objects.Projectile.createInstance(0, tower.x, tower.y);
                if (proj) {
                    proj.targetEnemy = target;
                    proj.projectileDamage = tower.towerDamage || 20;
                    proj.towerType = tower.towerType;
                }
            }
        }
    });
}

export function updateProjectiles(dt) {
    runtime.objects.Projectile.getAllInstances().forEach(proj => {
        const target = proj.targetEnemy;
        if (!target || !target.isAlive) {
            proj.destroy();
            return;
        }

        const dx = target.x - proj.x;
        const dy = target.y - proj.y;
        const dist = Math.sqrt(dx * dx + dy * dy);

        if (dist < 20) {
            // Hit
            target.enemyHP = (target.enemyHP || 10) - (proj.projectileDamage || 20);
            proj.destroy();

            if (target.enemyHP <= 0) {
                gold += target.enemyReward || 10;
                target.destroy();
            }
        } else {
            proj.x += (dx / dist) * 400 * dt;
            proj.y += (dy / dist) * 400 * dt;
        }
    });
}

export function spawnEnemyTD() {
    if (enemiesSpawned >= enemiesThisWave) {
        waveActive = false;
        return;
    }

    const hp = 15 + wave * 5;
    const reward = 5 + wave * 2;
    const speed = 60 + wave * 5;

    const enemy = runtime.objects.EnemyTD.createInstance(0, -32, 200);
    if (enemy) {
        enemy.enemyHP = hp;
        enemy.enemyMaxHP = hp;
        enemy.enemyReward = reward;
        enemy.enemySpeed = speed;
        enemy.waypointIndex = 0;
    }
    enemiesSpawned++;
}

export function updateEnemiesTD(dt) {
    runtime.objects.EnemyTD.getAllInstances().forEach(enemy => {
        // Follow waypoints (simplified: move right, then down, then left)
        const path = [
            { x: 200, y: 200 },
            { x: 600, y: 200 },
            { x: 600, y: 400 },
            { x: 200, y: 400 },
            { x: 200, y: 600 },
        ];

        const wpIdx = enemy.waypointIndex || 0;
        if (wpIdx >= path.length) {
            // Reached end
            lives--;
            enemy.destroy();
            runtime.callFunction("UpdateLives", lives);
            if (lives <= 0) {
                runtime.callFunction("DisplayMessage", "Game Over!");
            }
            return;
        }

        const wp = path[wpIdx];
        const dx = wp.x - enemy.x;
        const dy = wp.y - enemy.y;
        const dist = Math.sqrt(dx * dx + dy * dy);

        if (dist < 8) {
            enemy.waypointIndex = wpIdx + 1;
        } else {
            enemy.x += (dx / dist) * (enemy.enemySpeed || 60) * dt;
            enemy.y += (dy / dist) * (enemy.enemySpeed || 60) * dt;
        }
    });
}

export function startNextWave() {
    if (waveActive) return;
    wave++;
    enemiesThisWave = 5 + wave * 3;
    enemiesSpawned = 0;
    waveActive = true;
    waveTimer = 1.0;
}

// Main update
runtime.addEventListener("tick", () => {
    const dt = runtime.dt;

    if (waveActive) {
        waveTimer -= dt;
        if (waveTimer <= 0) {
            spawnEnemyTD();
            waveTimer = Math.max(0.3, 1.5 - wave * 0.05);
        }
    }

    updateTowers(dt);
    updateProjectiles(dt);
    updateEnemiesTD(dt);
});

runtime.addEventListener("beforeprojectstart", initTD);
