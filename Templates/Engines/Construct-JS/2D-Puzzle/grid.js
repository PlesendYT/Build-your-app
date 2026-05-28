// Grid Manager - Construct 3 Script

// Grid utility functions
export function createEmptyGrid(columns, rows) {
    const grid = [];
    for (let y = 0; y < rows; y++) {
        grid[y] = [];
        for (let x = 0; x < columns; x++) {
            grid[y][x] = 0;
        }
    }
    return grid;
}

export function cloneGrid(grid) {
    return JSON.parse(JSON.stringify(grid));
}

export function gridToString(grid) {
    return grid.map(row => row.join(",")).join("\n");
}

// Level data
const puzzleLevels = [
    {
        name: "Tutorial",
        grid: [
            [1, 1, 1, 1, 1],
            [1, 2, 0, 0, 1],
            [1, 0, 3, 4, 1],
            [1, 0, 0, 0, 1],
            [1, 1, 1, 1, 1],
        ]
    },
    {
        name: "Two Blocks",
        grid: [
            [1, 1, 1, 1, 1, 1],
            [1, 2, 0, 0, 0, 1],
            [1, 0, 3, 0, 3, 1],
            [1, 0, 0, 4, 0, 1],
            [1, 0, 0, 4, 0, 1],
            [1, 1, 1, 1, 1, 1],
        ]
    },
    {
        name: "L-Shape",
        grid: [
            [1, 1, 1, 1, 1],
            [1, 2, 0, 0, 1],
            [1, 0, 3, 0, 1],
            [1, 0, 0, 0, 1],
            [1, 1, 4, 1, 1],
        ]
    }
];

export function getLevel(index) {
    return puzzleLevels[index] || puzzleLevels[0];
}

export function getLevelCount() {
    return puzzleLevels.length;
}

export function isOnGoal(blockX, blockY, grid) {
    // Simplified: goal is represented by value 4 in original grid
    // But during gameplay, blocks (3) move onto empty (0) where goals (4) were
    return grid[blockY] && grid[blockY][blockX] === 4;
}
