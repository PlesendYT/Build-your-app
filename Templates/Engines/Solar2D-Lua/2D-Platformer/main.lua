-- Solar2D 2D Platformer - Main
display.setStatusBar(display.HiddenStatusBar)

local physics = require("physics")
physics.start()
physics.setGravity(0, 25)

local player
local score = 0
local lives = 3
local gameOver = false
local platforms = {}
local enemies = {}
local coins = {}

-- Constants
local GROUND_Y = display.contentHeight - 60
local PLAYER_SPEED = 4
local JUMP_FORCE = -12

-- Player setup
function createPlayer()
    player = display.newImageRect("player.png", 32, 48)
    player.x = 80
    player.y = GROUND_Y - 24
    player.xScale = 1
    physics.addBody(player, "dynamic", {
        density = 1.0,
        friction = 0.0,
        bounce = 0.0,
        box = { halfWidth = 14, halfHeight = 22 }
    })
    player.isFixedRotation = true
    player.grounded = false
    player.jumps = 2
    player.maxJumps = 2
end

-- Platforms
function createGround()
    local ground = display.newImageRect("ground.png", display.contentWidth * 2, 60)
    ground.x = display.contentCenterX
    ground.y = GROUND_Y + 30
    physics.addBody(ground, "static", {
        density = 1.0,
        friction = 1.0,
        bounce = 0.0,
        box = { halfWidth = display.contentWidth, halfHeight = 30 }
    })
    table.insert(platforms, ground)
end

function createPlatform(x, y, width, height)
    local plat = display.newRoundedRect(x, y, width, height, 4)
    plat:setFillColor(0.4, 0.7, 0.4)
    physics.addBody(plat, "static", {
        density = 1.0,
        friction = 0.5,
        bounce = 0.0,
        box = { halfWidth = width / 2, halfHeight = height / 2 }
    })
    table.insert(platforms, plat)
end

-- Enemy
function createEnemy(x, y)
    local enemy = display.newImageRect("enemy.png", 32, 32)
    enemy.x = x
    enemy.y = y
    physics.addBody(enemy, "kinematic", {
        density = 1.0,
        friction = 0.0,
        bounce = 0.0,
        box = { halfWidth = 14, halfHeight = 14 }
    })
    enemy.moveDir = 1
    enemy.patrolLeft = x - 80
    enemy.patrolRight = x + 80
    enemy.speed = 1
    table.insert(enemies, enemy)
end

-- Coin
function createCoin(x, y)
    local coin = display.newImageRect("coin.png", 20, 20)
    coin.x = x
    coin.y = y
    coin.value = 10
    table.insert(coins, coin)

    -- Floating animation
    transition.to(coin, {
        y = y - 8,
        time = 800,
        transition = easing.inOutSine,
        iterations = -1,
        onReverse = function()
            transition.to(coin, { y = y, time = 800, transition = easing.inOutSine })
        end
    })
end

-- Input handling
function handleKey(event)
    if gameOver then
        if event.keyName == "space" and event.phase == "down" then
            restartGame()
        end
        return true
    end

    if event.phase == "down" then
        if event.keyName == "right" or event.keyName == "d" then
            player.xScale = 1
            player.speedX = PLAYER_SPEED
        elseif event.keyName == "left" or event.keyName == "a" then
            player.xScale = -1
            player.speedX = -PLAYER_SPEED
        elseif event.keyName == "space" or event.keyName == "w" or event.keyName == "up" then
            if player.jumps > 0 then
                player:setLinearVelocity(player:getLinearVelocity())
                player:applyLinearImpulse(0, JUMP_FORCE, player.x, player.y)
                player.jumps = player.jumps - 1
            end
        end
    elseif event.phase == "up" then
        if event.keyName == "right" or event.keyName == "d" or
           event.keyName == "left" or event.keyName == "a" then
            player.speedX = 0
        end
    end
    return true
end
Runtime:addEventListener("key", handleKey)

-- Collision detection
function onCollision(event)
    if event.phase == "began" then
        if event.object1 == player or event.object2 == player then
            local other = event.object1 == player and event.object2 or event.object1

            -- Check if it's a coin
            for i, coin in ipairs(coins) do
                if other == coin then
                    score = score + coin.value
                    display.remove(coin)
                    table.remove(coins, i)
                    return true
                end
            end

            -- Check if it's an enemy
            for i, enemy in ipairs(enemies) do
                if other == enemy then
                    -- Check if stomping (player above)
                    local playerY = player.y
                    local enemyY = enemy.y
                    if playerY < enemyY - 10 then
                        display.remove(enemy)
                        table.remove(enemies, i)
                        player:setLinearVelocity(player:getLinearVelocity())
                        player:applyLinearImpulse(0, JUMP_FORCE * 0.6, player.x, player.y)
                        player.jumps = math.max(player.jumps, 1)
                        score = score + 50
                    else
                        playerHit()
                    end
                    return true
                end
            end
        end
    end

    -- Ground detection
    if event.phase == "began" then
        if event.object1 == player or event.object2 == player then
            player.grounded = true
            player.jumps = player.maxJumps
        end
    end
end
Runtime:addEventListener("collision", onCollision)

function playerHit()
    lives = lives - 1
    if lives <= 0 then
        gameOver = true
        showGameOver()
    else
        player.x = 80
        player.y = GROUND_Y - 24
        player:setLinearVelocity(0, 0)
    end
end

function showGameOver()
    local overlay = display.newRect(
        display.contentCenterX, display.contentCenterY,
        display.contentWidth, display.contentHeight
    )
    overlay:setFillColor(0, 0, 0, 0.7)
    overlay.alpha = 0
    transition.to(overlay, { alpha = 1, time = 500 })

    local text = display.newText("Game Over!", display.contentCenterX, display.contentCenterY - 40, nil, 48)
    local scoreText = display.newText("Score: " .. score, display.contentCenterX, display.contentCenterY + 10, nil, 24)
    local prompt = display.newText("Press SPACE to restart", display.contentCenterX, display.contentCenterY + 50, nil, 18)
end

function restartGame()
    for _, v in ipairs(platforms) do display.remove(v) end
    for _, v in ipairs(enemies) do display.remove(v) end
    for _, v in ipairs(coins) do display.remove(v) end
    platforms = {}
    enemies = {}
    coins = {}
    display.remove(player)
    score = 0
    lives = 3
    gameOver = false
    initGame()
end

-- Game loop
function gameLoop()
    if gameOver then return end

    -- Move player
    if player.speedX then
        player.x = player.x + player.speedX
    end

    -- Enemy patrol
    for _, enemy in ipairs(enemies) do
        if enemy.patrolLeft and enemy.patrolRight then
            enemy.x = enemy.x + enemy.speed * enemy.moveDir
            if enemy.x > enemy.patrolRight then
                enemy.moveDir = -1
                enemy.xScale = -1
            elseif enemy.x < enemy.patrolLeft then
                enemy.moveDir = 1
                enemy.xScale = 1
            end
        end
    end

    -- Update HUD
    if player and not gameOver then
        display.getCurrentStage():setFocus(player)
    end
end
Runtime:addEventListener("enterFrame", gameLoop)

-- Init
function initGame()
    createGround()
    createPlatform(200, GROUND_Y - 120, 120, 16)
    createPlatform(400, GROUND_Y - 200, 120, 16)
    createPlatform(600, GROUND_Y - 120, 120, 16)
    createPlayer()
    createEnemy(350, GROUND_Y - 16)
    createEnemy(550, GROUND_Y - 16)
    createCoin(200, GROUND_Y - 140)
    createCoin(250, GROUND_Y - 140)
    createCoin(400, GROUND_Y - 220)
    createCoin(600, GROUND_Y - 140)
end

initGame()
