// Obstacle Manager - Construct 3 Script

export function updateObstacles(dt) {
    const scrollSpeed = globalThis.scrollSpeed || 300;

    runtime.objects.ObstacleLow.getAllInstances().forEach(obs => {
        obs.x -= scrollSpeed * dt;
    });
    runtime.objects.ObstacleHigh.getAllInstances().forEach(obs => {
        obs.x -= scrollSpeed * dt;
    });
}

// High score management
export function getHighScore() {
    try {
        const saved = localStorage.getItem("runner_highscore");
        return saved ? parseInt(saved) : 0;
    } catch (e) {
        return 0;
    }
}

export function saveHighScore(newScore) {
    try {
        const current = getHighScore();
        if (newScore > current) {
            localStorage.setItem("runner_highscore", String(newScore));
        }
    } catch (e) {
        // localStorage not available
    }
}

// Ground scrolling
export function updateGround(dt) {
    const scrollSpeed = globalThis.scrollSpeed || 300;
    runtime.objects.Ground.getAllInstances().forEach(ground => {
        ground.x -= scrollSpeed * dt;
        if (ground.x <= -64) {
            ground.x += 128; // Reset position
        }
    });
}

runtime.addEventListener("tick", () => {
    const dt = runtime.dt;
    updateObstacles(dt);
    updateGround(dt);
});
