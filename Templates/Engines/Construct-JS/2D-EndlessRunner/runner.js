// Endless Runner Core - Construct 3 Script

const SCROLL_SPEED_BASE = 300;
const GRAVITY = 2000;
const JUMP_FORCE = -900;
const SLIDE_DURATION = 0.8;

let scrollSpeed = SCROLL_SPEED_BASE;
let score = 0;
let highScore = 0;
let isGameOver = false;
let isSliding = false;
let slideTimer = 0;
let gameTime = 0;

export function initRunner() {
    const player = runtime.objects.Player.getFirstInstance();
    if (player) {
        player.x = 150;
        player.y = 300;
        player.playerVy = 0;
        player.isGrounded = true;
    }
    score = 0;
    isGameOver = false;
    scrollSpeed = SCROLL_SPEED_BASE;
    gameTime = 0;
    isSliding = false;
}

export function handleRunnerInput() {
    if (isGameOver) {
        if (runtime.keyboard.wasKeyPressed("Space") || runtime.keyboard.wasKeyPressed("Enter")) {
            runtime.callFunction("RestartLayout");
        }
        return;
    }

    const player = runtime.objects.Player.getFirstInstance();
    if (!player) return;

    const jumpPressed = runtime.keyboard.wasKeyPressed("Space") || runtime.keyboard.wasKeyPressed("Up");
    const downHeld = runtime.keyboard.isKeyDown("Down");
    const downPressed = runtime.keyboard.wasKeyPressed("Down");

    // Jump
    if (jumpPressed && player.isGrounded && !isSliding) {
        player.playerVy = JUMP_FORCE;
        player.isGrounded = false;
    }

    // Slide
    if (downPressed && player.isGrounded && !isSliding) {
        isSliding = true;
        slideTimer = SLIDE_DURATION;
    }

    if (isSliding) {
        slideTimer -= runtime.dt;
        if (slideTimer <= 0) {
            isSliding = false;
        }
    }
}

export function updateRunner(dt) {
    if (isGameOver) return;

    gameTime += dt;
    scrollSpeed = SCROLL_SPEED_BASE + gameTime * 10;

    const player = runtime.objects.Player.getFirstInstance();
    if (!player) return;

    // Gravity
    if (!player.isGrounded) {
        player.playerVy += GRAVITY * dt;
        player.y += player.playerVy * dt;

        // Ground collision
        if (player.y >= 300) {
            player.y = 300;
            player.playerVy = 0;
            player.isGrounded = true;
        }
    }

    // Score
    score += dt * 10 * (scrollSpeed / SCROLL_SPEED_BASE);
    runtime.callFunction("UpdateScore", Math.floor(score));

    // Parallax scrolling for background
    const bg = runtime.objects.Background.getFirstInstance();
    if (bg) {
        bg.x -= scrollSpeed * 0.3 * dt;
        if (bg.x <= -960) bg.x = 960;
    }
}

export function spawnObstacle() {
    if (isGameOver) return;

    const obstacleTypes = ["low", "high"];
    const type = obstacleTypes[Math.floor(Math.random() * obstacleTypes.length)];
    const spawnX = runtime.layout.width + 64;

    if (type === "low") {
        const obs = runtime.objects.ObstacleLow.createInstance(0, spawnX, 300);
        if (obs) obs.obstacleType = "low";
    } else {
        const obs = runtime.objects.ObstacleHigh.createInstance(0, spawnX, 252);
        if (obs) obs.obstacleType = "high";
    }

    // Schedule next spawn
    const interval = Math.max(0.8, 2.0 - gameTime * 0.01);
    setTimeout(spawnObstacle, interval * 1000);
}

export function onObstacleHit() {
    if (isGameOver || isSliding) return;

    const player = runtime.objects.Player.getFirstInstance();
    if (!player) return;

    // Check if sliding under high obstacle
    if (isSliding) return;

    isGameOver = true;
    if (score > highScore) {
        highScore = Math.floor(score);
    }
    runtime.callFunction("DisplayMessage", `Game Over! Score: ${Math.floor(score)}`);
}

// Collision detection helper
export function checkCollisions() {
    if (isGameOver) return;

    const player = runtime.objects.Player.getFirstInstance();
    if (!player) return;

    // Check low obstacles
    runtime.objects.ObstacleLow.getAllInstances().forEach(obs => {
        const overlap = Math.abs(player.x - obs.x) < 40 && Math.abs(player.y - obs.y) < 40;
        if (overlap) onObstacleHit();
    });

    // Check high obstacles (player can slide under if sliding)
    if (!isSliding) {
        runtime.objects.ObstacleHigh.getAllInstances().forEach(obs => {
            const overlap = Math.abs(player.x - obs.x) < 40 && Math.abs(player.y - obs.y) < 40;
            if (overlap) onObstacleHit();
        });
    }
}

// Remove off-screen obstacles
export function cleanupObstacles() {
    runtime.objects.ObstacleLow.getAllInstances().forEach(obs => {
        if (obs.x < -100) obs.destroy();
    });
    runtime.objects.ObstacleHigh.getAllInstances().forEach(obs => {
        if (obs.x < -100) obs.destroy();
    });
}

runtime.addEventListener("tick", () => {
    const dt = runtime.dt;
    handleRunnerInput();
    updateRunner(dt);
    checkCollisions();
    cleanupObstacles();
});

runtime.addEventListener("beforeprojectstart", () => {
    initRunner();
    setTimeout(spawnObstacle, 2000);
});
