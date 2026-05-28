// Platformer Core - Construct 3 Script
import * as platform from "./player.js";

const PLATFORMER_READY = true;

// Level configuration
const levels = [
    {
        name: "Level 1",
        spawn: { x: 64, y: 384 },
        objective: "Collect all coins",
        coinCount: 5
    },
    {
        name: "Level 2",
        spawn: { x: 64, y: 256 },
        objective: "Reach the flag",
        coinCount: 8
    }
];

let currentLevel = 0;
let score = 0;

// Initialize level
function loadLevel(levelIndex) {
    const level = levels[levelIndex];
    if (!level) return;

    const player = runtime.objects.Player.getFirstInstance();
    if (player) {
        player.x = level.spawn.x;
        player.y = level.spawn.y;
    }

    // Reset coins
    runtime.objects.Coin.getAllInstances().forEach(coin => {
        coin.isVisible = true;
    });

    runtime.callFunction("DisplayMessage", `Level: ${level.name}`);
}

// Called when player collects a coin
export function onCoinCollected() {
    score += 100;
    runtime.callFunction("UpdateScore", score);
}

// Called when player dies
export function onPlayerDeath() {
    runtime.callFunction("DisplayMessage", "Game Over!");
    runtime.callFunction("RestartLevel");
}

// Level complete check
globalThis.checkLevelComplete = function() {
    const remainingCoins = runtime.objects.Coin.getAllInstances()
        .filter(c => c.isVisible).length;
    if (remainingCoins === 0) {
        currentLevel++;
        if (currentLevel < levels.length) {
            loadLevel(currentLevel);
            runtime.callFunction("DisplayMessage", "Level Complete!");
        } else {
            runtime.callFunction("DisplayMessage", "You Win!");
        }
    }
};

// Register event handlers
runtime.addEventListener("beforeprojectstart", () => {
    loadLevel(0);
});
