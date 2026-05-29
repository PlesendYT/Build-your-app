-- Puzzle module for Solar2D
-- Provides level definitions, grid cloning, player finding, and block counting utilities
local M = {}

local TILE_SIZE = 64

-- Puzzle level definitions with name and grid layout
M.levels = {
    {
        name = "Tutorial 1",
        grid = {
            {1, 1, 1, 1, 1},
            {1, 2, 0, 0, 1},
            {1, 0, 3, 4, 1},
            {1, 0, 0, 0, 1},
            {1, 1, 1, 1, 1},
        }
    },
    {
        name = "Two Blocks",
        grid = {
            {1, 1, 1, 1, 1, 1},
            {1, 2, 0, 0, 0, 1},
            {1, 0, 3, 0, 3, 1},
            {1, 0, 0, 4, 0, 1},
            {1, 0, 0, 4, 0, 1},
            {1, 1, 1, 1, 1, 1},
        }
    },
    {
        name = "Corner Push",
        grid = {
            {1, 1, 1, 1, 1},
            {1, 2, 0, 0, 1},
            {1, 0, 0, 3, 1},
            {1, 4, 0, 0, 1},
            {1, 1, 1, 1, 1},
        }
    }
}

-- Returns a level by index (0-based), defaults to first level
function M.getLevel(index)
    return M.levels[index + 1] or M.levels[1]
end

-- Returns the total number of defined levels
function M.getLevelCount()
    return #M.levels
end

-- Creates a deep copy of a grid for safe manipulation
function M.cloneGrid(gridData)
    local new = {}
    for y, row in ipairs(gridData) do
        new[y] = {}
        for x, cell in ipairs(row) do
            new[y][x] = cell
        end
    end
    return new
end

-- Scans grid for player position (value 2), returns (1,1) as fallback
function M.findPlayer(grid)
    for y, row in ipairs(grid) do
        for x, cell in ipairs(row) do
            if cell == 2 then return x, y end
        end
    end
    return 1, 1
end

-- Counts all pushable blocks (value 3) in the grid
function M.countBlocks(grid)
    local count = 0
    for _, row in ipairs(grid) do
        for _, cell in ipairs(row) do
            if cell == 3 then count = count + 1 end
        end
    end
    return count
end

return M
