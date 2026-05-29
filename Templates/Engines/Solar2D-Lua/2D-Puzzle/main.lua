-- Solar2D Puzzle Game (Sokoban-style)
-- Main game controller: loads levels, moves player, pushes blocks onto goals
display.setStatusBar(display.HiddenStatusBar)

local TILE_SIZE = 64
local grid = {}
local player
local blocks = {}
local goals = {}
local currentLevel = 0
local moveCount = 0

-- Level data: 0=empty, 1=wall, 2=player start, 3=pushable block, 4=goal
local levels = {
    {
        {1, 1, 1, 1, 1},
        {1, 2, 0, 0, 1},
        {1, 0, 3, 4, 1},
        {1, 0, 0, 0, 1},
        {1, 1, 1, 1, 1},
    },
    {
        {1, 1, 1, 1, 1, 1},
        {1, 2, 0, 0, 0, 1},
        {1, 0, 3, 0, 3, 1},
        {1, 0, 0, 4, 0, 1},
        {1, 0, 0, 4, 0, 1},
        {1, 1, 1, 1, 1, 1},
    },
}

function loadLevel(levelIndex)
    -- Clear existing
    for _, v in ipairs(blocks) do display.remove(v) end
    for _, v in ipairs(goals) do display.remove(v) end
    blocks = {}
    goals = {}

    local levelData = levels[levelIndex + 1]
    if not levelData then
        showWin()
        return
    end

    grid = {}
    local offsetX = display.contentCenterX - (#levelData[1] * TILE_SIZE) / 2
    local offsetY = display.contentCenterY - (#levelData * TILE_SIZE) / 2

    for y, row in ipairs(levelData) do
        grid[y] = {}
        for x, cell in ipairs(row) do
            grid[y][x] = cell
            local px = offsetX + (x - 1) * TILE_SIZE + TILE_SIZE / 2
            local py = offsetY + (y - 1) * TILE_SIZE + TILE_SIZE / 2

            if cell == 1 then
                local wall = display.newRect(px, py, TILE_SIZE - 2, TILE_SIZE - 2)
                wall:setFillColor(0.4, 0.4, 0.4)
            elseif cell == 2 then
                player = display.newCircle(px, py, 14)
                player:setFillColor(0.2, 0.6, 1.0)
                player.gridX = x
                player.gridY = y
                grid[y][x] = 0
            elseif cell == 3 then
                local block = display.newRect(px, py, TILE_SIZE - 8, TILE_SIZE - 8)
                block:setFillColor(0.8, 0.6, 0.2)
                block.gridX = x
                block.gridY = y
                table.insert(blocks, block)
            elseif cell == 4 then
                local goal = display.newCircle(px, py, 12)
                goal:setFillColor(0.2, 0.9, 0.2)
                goal.alpha = 0.5
                table.insert(goals, goal)
            end
        end
    end
    moveCount = 0
end

function movePlayer(dx, dy)
    local nx = player.gridX + dx
    local ny = player.gridY + dy

    -- Bounds
    if ny < 1 or ny > #grid or nx < 1 or nx > #grid[1] then return false end

    -- Wall check
    if grid[ny][nx] == 1 then return false end

    -- Block check
    if grid[ny][nx] == 3 then
        local pushX = nx + dx
        local pushY = ny + dy
        if pushY < 1 or pushY > #grid or pushX < 1 or pushX > #grid[1] then return false end
        if grid[pushY][pushX] ~= 0 and grid[pushY][pushX] ~= 4 then return false end

        -- Move block in grid
        grid[ny][nx] = 0
        grid[pushY][pushX] = 3

        -- Move block visually
        for _, block in ipairs(blocks) do
            if block.gridX == nx and block.gridY == ny then
                block.gridX = pushX
                block.gridY = pushY
                transition.to(block, {
                    x = block.x + dx * TILE_SIZE,
                    y = block.y + dy * TILE_SIZE,
                    time = 100
                })
                break
            end
        end
    end

    -- Move player
    player.gridX = nx
    player.gridY = ny
    transition.to(player, {
        x = player.x + dx * TILE_SIZE,
        y = player.y + dy * TILE_SIZE,
        time = 80
    })
    moveCount = moveCount + 1

    checkWin()
    return true
end

function checkWin()
    local levelData = levels[currentLevel + 1]
    if not levelData then return end

    local onGoal = 0
    local totalBlocks = 0

    for y, row in ipairs(grid) do
        for x, cell in ipairs(row) do
            if cell == 3 then
                totalBlocks = totalBlocks + 1
                if levelData[y] and levelData[y][x] == 4 then
                    onGoal = onGoal + 1
                end
            end
        end
    end

    if totalBlocks > 0 and onGoal == totalBlocks then
        currentLevel = currentLevel + 1
        timer.performWithDelay(500, function()
            loadLevel(currentLevel)
        end)
    end
end

function showWin()
    display.newText("You Win!", display.contentCenterX, display.contentCenterY, nil, 48)
end

function resetLevel()
    loadLevel(currentLevel)
end

-- Handles WASD/arrow keys for movement and R to reset the current level
function handleKey(event)
    if event.phase == "down" then
        if event.keyName == "up" or event.keyName == "w" then movePlayer(0, -1)
        elseif event.keyName == "down" or event.keyName == "s" then movePlayer(0, 1)
        elseif event.keyName == "left" or event.keyName == "a" then movePlayer(-1, 0)
        elseif event.keyName == "right" or event.keyName == "d" then movePlayer(1, 0)
        elseif event.keyName == "r" then resetLevel() end
    end
    return true
end
Runtime:addEventListener("key", handleKey)

loadLevel(0)
