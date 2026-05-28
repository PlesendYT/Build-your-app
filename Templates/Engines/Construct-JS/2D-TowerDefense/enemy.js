// Enemy TD Types - Construct 3 Script

const enemyTypesTD = {
    normal: {
        hp: 10,
        speed: 60,
        reward: 10,
        color: "red"
    },
    fast: {
        hp: 6,
        speed: 120,
        reward: 15,
        color: "yellow"
    },
    armored: {
        hp: 30,
        speed: 40,
        reward: 25,
        color: "gray"
    },
    boss: {
        hp: 100,
        speed: 30,
        reward: 100,
        color: "purple"
    }
};

export function getEnemyTDConfig(type) {
    return enemyTypesTD[type] || enemyTypesTD.normal;
}

export function spawnEnemyOfType(type, x, y, hpMultiplier) {
    const config = getEnemyTDConfig(type);
    const hp = config.hp * (hpMultiplier || 1);
    const enemy = runtime.objects.EnemyTD.createInstance(0, x, y);

    if (enemy) {
        enemy.enemyHP = hp;
        enemy.enemyMaxHP = hp;
        enemy.enemySpeed = config.speed;
        enemy.enemyReward = config.reward;
        enemy.enemyType = type;
        enemy.waypointIndex = 0;
    }
    return enemy;
}

// Health bar rendering
export function drawEnemyHealthBar(enemy) {
    const ctx = runtime.document.createElement("canvas").getContext("2d");
    const maxHP = enemy.enemyMaxHP || 10;
    const hp = enemy.enemyHP || 0;
    const ratio = Math.max(0, hp / maxHP);

    runtime.objects.EnemyTD.getAllInstances().forEach(e => {
        const barWidth = 40;
        const barHeight = 4;
        const x = e.x - barWidth / 2;
        const y = e.y - 30;

        // Background
        ctx.fillStyle = "red";
        ctx.fillRect(x, y, barWidth, barHeight);

        // HP
        ctx.fillStyle = "green";
        ctx.fillRect(x, y, barWidth * ratio, barHeight);

        // Border
        ctx.strokeStyle = "white";
        ctx.strokeRect(x, y, barWidth, barHeight);
    });
}

// Wave configuration
const waveConfigs = [];
for (let i = 1; i <= 10; i++) {
    waveConfigs.push({
        wave: i,
        enemies: [
            { type: "normal", count: 3 + i * 2, hpMult: 1 + i * 0.1 },
            { type: "fast", count: Math.max(0, i - 2), hpMult: 1 + i * 0.1 },
            { type: "armored", count: Math.max(0, Math.floor(i / 3)), hpMult: 1 + i * 0.1 },
        ],
        bossWave: i % 5 === 0
    });
}

export function getWaveConfig(waveNumber) {
    return waveConfigs.find(w => w.wave === waveNumber) || waveConfigs[waveConfigs.length - 1];
}
