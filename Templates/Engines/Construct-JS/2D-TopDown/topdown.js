// Top-Down Core - Construct 3 Script

const MOVE_SPEED = 250;
const DIAGONAL_FACTOR = 0.707;

let keys = [];
let hasKey = false;
let gameState = "playing"; // "playing", "paused"

export function initTopDown() {
    const player = runtime.objects.Player.getFirstInstance();
    if (player) {
        player.x = 320;
        player.y = 240;
    }
    hasKey = false;
    gameState = "playing";
}

export function handleTopDownMovement(dt) {
    if (gameState !== "playing") return;

    const player = runtime.objects.Player.getFirstInstance();
    if (!player) return;

    let dx = 0, dy = 0;

    if (runtime.keyboard.isKeyDown("ArrowLeft") || runtime.keyboard.isKeyDown("A")) dx -= 1;
    if (runtime.keyboard.isKeyDown("ArrowRight") || runtime.keyboard.isKeyDown("D")) dx += 1;
    if (runtime.keyboard.isKeyDown("ArrowUp") || runtime.keyboard.isKeyDown("W")) dy -= 1;
    if (runtime.keyboard.isKeyDown("ArrowDown") || runtime.keyboard.isKeyDown("S")) dy += 1;

    // Diagonal normalization
    if (dx !== 0 && dy !== 0) {
        dx *= DIAGONAL_FACTOR;
        dy *= DIAGONAL_FACTOR;
    }

    // Store movement for behavior
    player.playerDx = dx;
    player.playerDy = dy;

    // Move
    player.x += dx * MOVE_SPEED * dt;
    player.y += dy * MOVE_SPEED * dt;

    // Animation based on direction
    if (dx !== 0 || dy !== 0) {
        player.animationFrame = player.animationFrame + 8 * dt;
    }
}

export function onItemCollected() {
    const player = runtime.objects.Player.getFirstInstance();
    const item = runtime.objects.Item.getFirstInstance();
    if (player && item && item.isVisible) {
        if (item.itemType === "key") {
            hasKey = true;
        } else if (item.itemType === "health") {
            runtime.callFunction("HealPlayer", 25);
        }
        item.isVisible = false;
    }
}

export function onDoorCollision() {
    const door = runtime.objects.Door.getFirstInstance();
    if (door && hasKey) {
        door.isVisible = false;
        hasKey = false;
        runtime.callFunction("DisplayMessage", "Door unlocked!");
    } else if (door) {
        runtime.callFunction("DisplayMessage", "You need a key!");
    }
}

// Enemy patrol
export function updateEnemies(dt) {
    runtime.objects.Enemy.getAllInstances().forEach(enemy => {
        if (!enemy.patrolDir) enemy.patrolDir = 1;
        if (!en PatrolTimer) enemy.patrolTimer = 0;

        enemy.patrolTimer += dt;
        if (enemy.patrolTimer > 2) {
            enemy.patrolDir *= -1;
            enemy.patrolTimer = 0;
        }

        enemy.x += enemy.patrolDir * 60 * dt;

        // Check distance to player
        const player = runtime.objects.Player.getFirstInstance();
        if (player) {
            const dist = Math.sqrt(
                Math.pow(player.x - enemy.x, 2) + Math.pow(player.y - enemy.y, 2)
            );
            if (dist < 150) {
                // Chase player
                const angle = Math.atan2(player.y - enemy.y, player.x - enemy.x);
                enemy.x += Math.cos(angle) * 120 * dt;
                enemy.y += Math.sin(angle) * 120 * dt;
            }
        }
    });
}

runtime.addEventListener("tick", () => {
    const dt = runtime.dt;
    handleTopDownMovement(dt);
    updateEnemies(dt);
});
