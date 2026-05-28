// Puzzle Core - Construct 3 Script

const TILE_SIZE = 64;
let puzzleState = [];
let moveCount = 0;
let maxMoves = 50;
let puzzleSolved = false;

export function initPuzzle(gridData) {
    puzzleState = JSON.parse(JSON.stringify(gridData));
    moveCount = 0;
    puzzleSolved = false;
    rebuildVisualGrid();
}

export function rebuildVisualGrid() {
    // Clear existing tiles
    runtime.objects.Floor.getAllInstances().forEach(f => f.destroy());
    runtime.objects.Wall.getAllInstances().forEach(w => w.destroy());
    runtime.objects.Block.getAllInstances().forEach(b => b.destroy());
    runtime.objects.Goal.getAllInstances().forEach(g => g.destroy());

    for (let y = 0; y < puzzleState.length; y++) {
        for (let x = 0; x < puzzleState[y].length; x++) {
            const px = x * TILE_SIZE + TILE_SIZE / 2;
            const py = y * TILE_SIZE + TILE_SIZE / 2;
            const cell = puzzleState[y][x];

            // Create floor always
            runtime.objects.Floor.createInstance(0, px, py);

            switch (cell) {
                case 1: runtime.objects.Wall.createInstance(0, px, py); break;
                case 2: break; // Player - handled separately
                case 3: runtime.objects.Block.createInstance(0, px, py); break;
                case 4: runtime.objects.Goal.createInstance(0, px, py); break;
            }
        }
    }
}

export function movePlayer(dx, dy) {
    if (puzzleSolved) return false;

    const player = runtime.objects.Player.getFirstInstance();
    if (!player) return false;

    const gridX = Math.floor(player.x / TILE_SIZE);
    const gridY = Math.floor(player.y / TILE_SIZE);
    const targetX = gridX + dx;
    const targetY = gridY + dy;

    // Bounds check
    if (targetY < 0 || targetY >= puzzleState.length ||
        targetX < 0 || targetX >= puzzleState[0].length) return false;

    // Wall check
    if (puzzleState[targetY][targetX] === 1) return false;

    // Block check
    if (puzzleState[targetY][targetX] === 3) {
        // Try to push block
        const pushX = targetX + dx;
        const pushY = targetY + dy;

        if (pushY < 0 || pushY >= puzzleState.length ||
            pushX < 0 || pushX >= puzzleState[0].length) return false;
        if (puzzleState[pushY][pushX] !== 0 && puzzleState[pushY][pushX] !== 4) return false;

        // Move block
        puzzleState[targetY][targetX] = 0;
        puzzleState[pushY][pushX] = 3;

        // Move block instance
        const block = runtime.objects.Block.getFirstInstance();
        if (block) {
            block.x = pushX * TILE_SIZE + TILE_SIZE / 2;
            block.y = pushY * TILE_SIZE + TILE_SIZE / 2;
        }
    }

    // Move player
    player.x = targetX * TILE_SIZE + TILE_SIZE / 2;
    player.y = targetY * TILE_SIZE + TILE_SIZE / 2;
    moveCount++;

    checkPuzzleSolved();
    return true;
}

function checkPuzzleSolved() {
    let blocksOnGoals = 0;
    let totalBlocks = 0;

    for (let y = 0; y < puzzleState.length; y++) {
        for (let x = 0; x < puzzleState[y].length; x++) {
            if (puzzleState[y][x] === 3) {
                totalBlocks++;
                const goal = runtime.objects.Goal.getAllInstances().find(g =>
                    Math.floor(g.x / TILE_SIZE) === x &&
                    Math.floor(g.y / TILE_SIZE) === y
                );
                if (goal) blocksOnGoals++;
            }
        }
    }

    if (totalBlocks > 0 && blocksOnGoals === totalBlocks) {
        puzzleSolved = true;
        runtime.callFunction("DisplayMessage", "Puzzle Solved!");
    }
}

export function resetPuzzle() {
    // Reinitialize with original grid
    initPuzzle(originalGrid);
    const player = runtime.objects.Player.getFirstInstance();
    if (player) {
        player.x = TILE_SIZE + TILE_SIZE / 2;
        player.y = TILE_SIZE + TILE_SIZE / 2;
    }
}

// Default grid
const originalGrid = [
    [1, 1, 1, 1, 1],
    [1, 0, 0, 0, 1],
    [1, 0, 3, 4, 1],
    [1, 2, 0, 0, 1],
    [1, 1, 1, 1, 1],
];

// Input handling
runtime.addEventListener("tick", () => {
    if (runtime.keyboard.wasKeyPressed("ArrowUp") || runtime.keyboard.wasKeyPressed("W")) movePlayer(0, -1);
    if (runtime.keyboard.wasKeyPressed("ArrowDown") || runtime.keyboard.wasKeyPressed("S")) movePlayer(0, 1);
    if (runtime.keyboard.wasKeyPressed("ArrowLeft") || runtime.keyboard.wasKeyPressed("A")) movePlayer(-1, 0);
    if (runtime.keyboard.wasKeyPressed("ArrowRight") || runtime.keyboard.wasKeyPressed("D")) movePlayer(1, 0);
    if (runtime.keyboard.wasKeyPressed("R")) resetPuzzle();
});

runtime.addEventListener("beforeprojectstart", () => {
    initPuzzle(originalGrid);
});
