-- Solar2D Top-Down Adventure
display.setStatusBar(display.HiddenStatusBar)

local physics = require("physics")
physics.start()
physics.setGravity(0, 0)

local player
local walls = {}
local enemies = {}
local items = {}
local keys = 0
local playerHP = 5
local gameOver = false
local moveSpeed = 3

-- Player
function createPlayer()
    player = display.newImageRect("player.png", 28, 32)
    player.x = display.contentCenterX
    player.y = display.contentCenterY
    physics.addBody(player, "dynamic", {
        density = 1.0,
        friction = 0.0,
        bounce = 0.0,
        radius = 12
    })
    player.isFixedRotation = true
    player.moveX = 0
    player.moveY = 0
end

-- Walls
function createWall(x, y, w, h)
    local wall = display.newRect(x, y, w, h)
    wall:setFillColor(0.3, 0.3, 0.3)
    physics.addBody(wall, "static", {
        density = 1.0,
        friction = 0.0,
        bounce = 0.0
    })
    table.insert(walls, wall)
end

-- Enemy
function createEnemy(x, y)
    local enemy = display.newImageRect("enemy.png", 24, 24)
    enemy.x = x
    enemy.y = y
    physics.addBody(enemy, "kinematic", {
        density = 1.0,
        friction = 0.0,
        bounce = 0.0,
        radius = 10
    })
    enemy.chaseRange = 200
    enemy.speed = 1.5
    enemy.hp = 2
    enemy.damage = 1
    enemy.attackCooldown = 0
    table.insert(enemies, enemy)
end

-- Item
function createItem(x, y, itemType)
    local item = display.newImageRect("item.png", 16, 16)
    item.x = x
    item.y = y
    item.itemType = itemType
    table.insert(items, item)
end

-- Input
function handleKey(event)
    if gameOver then return true end

    if event.phase == "down" then
        if event.keyName == "right" or event.keyName == "d" then player.moveX = moveSpeed
        elseif event.keyName == "left" or event.keyName == "a" then player.moveX = -moveSpeed
        elseif event.keyName == "up" or event.keyName == "w" then player.moveY = -moveSpeed
        elseif event.keyName == "down" or event.keyName == "s" then player.moveY = moveSpeed
        elseif event.keyName == "space" then tryOpenDoor() end
    elseif event.phase == "up" then
        if event.keyName == "right" or event.keyName == "d" then player.moveX = 0
        elseif event.keyName == "left" or event.keyName == "a" then player.moveX = 0
        elseif event.keyName == "up" or event.keyName == "w" then player.moveY = 0
        elseif event.keyName == "down" or event.keyName == "s" then player.moveY = 0
        end
    end
    return true
end
Runtime:addEventListener("key", handleKey)

-- Collision
function onCollision(event)
    if event.phase == "began" then
        -- Item pickup
        for i, item in ipairs(items) do
            if (event.object1 == item and event.object2 == player) or
               (event.object2 == item and event.object1 == player) then
                if item.itemType == "key" then
                    keys = keys + 1
                elseif item.itemType == "health" then
                    playerHP = math.min(10, playerHP + 2)
                end
                display.remove(item)
                table.remove(items, i)
                return true
            end
        end

        -- Enemy collision
        for _, enemy in ipairs(enemies) do
            if (event.object1 == enemy and event.object2 == player) or
               (event.object2 == enemy and event.object1 == player) then
                if enemy.attackCooldown <= 0 then
                    playerHP = playerHP - enemy.damage
                    enemy.attackCooldown = 30
                    if playerHP <= 0 then
                        gameOver = true
                        showGameOver()
                    end
                end
                return true
            end
        end
    end
end
Runtime:addEventListener("collision", onCollision)

function tryOpenDoor()
    -- Find nearby door
    for _, wall in ipairs(walls) do
        if wall.isDoor and keys > 0 then
            local dist = math.sqrt(
                (wall.x - player.x)^2 + (wall.y - player.y)^2
            )
            if dist < 50 then
                display.remove(wall)
                keys = keys - 1
                return true
            end
        end
    end
    return false
end

function showGameOver()
    local overlay = display.newRect(
        display.contentCenterX, display.contentCenterY,
        display.contentWidth, display.contentHeight
    )
    overlay:setFillColor(0, 0, 0, 0.7)
    display.newText("Game Over", display.contentCenterX, display.contentCenterY - 20, nil, 36)
end

function initGame()
    -- Borders
    createWall(display.contentCenterX, 30, display.contentWidth, 60)
    createWall(display.contentCenterX, display.contentHeight - 30, display.contentWidth, 60)
    createWall(30, display.contentCenterY, 60, display.contentHeight)
    createWall(display.contentWidth - 30, display.contentCenterY, 60, display.contentHeight)

    -- Room walls
    createWall(300, display.contentCenterY, 8, 200)
    createWall(500, display.contentCenterY, 8, 200)

    createPlayer()
    createEnemy(400, 200)
    createEnemy(600, 300)
    createItem(200, 300, "key")
    createItem(550, 400, "health")
end

-- Game loop
function gameLoop()
    if gameOver then return end

    -- Move player
    if player then
        player.x = player.x + player.moveX
        player.y = player.y + player.moveY
    end

    -- Enemy AI
    for _, enemy in ipairs(enemies) do
        if enemy.attackCooldown > 0 then
            enemy.attackCooldown = enemy.attackCooldown - 1
        end

        local dist = math.sqrt(
            (player.x - enemy.x)^2 + (player.y - enemy.y)^2
        )
        if dist < enemy.chaseRange then
            local dx = (player.x - enemy.x) / dist
            local dy = (player.y - enemy.y) / dist
            enemy.x = enemy.x + dx * enemy.speed
            enemy.y = enemy.y + dy * enemy.speed
        end
    end
end
Runtime:addEventListener("enterFrame", gameLoop)

initGame()
