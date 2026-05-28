// Player Controller - Construct 3 Script

const MOVE_SPEED = 300;
const JUMP_FORCE = -600;
const GRAVITY = 1500;

export function setupPlayer() {
    const player = runtime.objects.Player.getFirstInstance();
    if (!player) return;

    player.playerSpeed = 0;
    player.isGrounded = false;
    player.jumpsLeft = 2;
    player.maxJumps = 2;
}

export function handlePlayerMovement(dt) {
    const player = runtime.objects.Player.getFirstInstance();
    if (!player) return;

    // Get input
    const left = runtime.keyboard.isKeyDown("ArrowLeft") || runtime.keyboard.isKeyDown("A");
    const right = runtime.keyboard.isKeyDown("ArrowRight") || runtime.keyboard.isKeyDown("D");
    const jumpPressed = runtime.keyboard.wasKeyPressed("Space") || runtime.keyboard.wasKeyPressed("W") || runtime.keyboard.wasKeyPressed("ArrowUp");

    // Ground check
    player.isGrounded = player.platformerInfo.isOnFloor;

    // Reset jumps when grounded
    if (player.isGrounded) {
        player.jumpsLeft = player.maxJumps;
    }

    // Horizontal movement
    if (left) {
        player.platformerInfo.x = -MOVE_SPEED;
        player.animationFrame = Math.floor(player.animationFrame * 10 + 1) / 10;
    } else if (right) {
        player.platformerInfo.x = MOVE_SPEED;
        player.animationFrame = Math.floor(player.animationFrame * 10 + 1) / 10;
    } else {
        player.platformerInfo.x = 0;
    }

    // Jump
    if (jumpPressed && player.jumpsLeft > 0) {
        player.platformerInfo.y = JUMP_FORCE;
        player.jumpsLeft--;
    }

    // Apply gravity
    if (!player.isGrounded) {
        player.platformerInfo.y += GRAVITY * dt;
    }
}

export function onPlayerHitEnemy() {
    const player = runtime.objects.Player.getFirstInstance();
    if (!player) return;

    // Stomp enemy when falling
    if (player.platformerInfo.y > 0) {
        player.platformerInfo.y = JUMP_FORCE * 0.6;
        player.jumpsLeft = Math.max(player.jumpsLeft, 1);
    } else {
        // Player takes damage
        runtime.callFunction("PlayerDeath");
    }
}

// Register update
runtime.addEventListener("tick", () => {
    const dt = runtime.dt;
    handlePlayerMovement(dt);
});
