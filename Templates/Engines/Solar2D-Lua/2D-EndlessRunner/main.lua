-- Solar2D Endless Runner
display.setStatusBar(display.HiddenStatusBar)

local physics = require("physics")
physics.start()
physics.setGravity(0, 30)

local player
local obstacles = {}
local grounds = {}
local score = 0
local highScore = 0
local gameOver = false
local scrollSpeed = 300
local canJump = true
local isSliding = false

-- Load highscore
local function loadHighScore()
    local file = io.open("runner_highscore.txt", "r")
    if file then
        highScore = tonumber(file:read("*a")) or 0
        file:close()
    end
end

local function saveHighScore()
    local file = io.open("runner_highscore.txt", "w")
    if file then
        file:write(tostring(math.floor(score)))
        file:close()
    end
end

-- Player
function createPlayer()
    player = display.newImageRect("player.png", 32, 48)
    player.x = 120
    player.y = display.contentHeight - 100
    physics.addBody(player, "dynamic", {
        density = 1.0,
        friction = 0.0,
        bounce = 0.0,
        box = { halfWidth = 12, halfHeight = 20 }
    })
    player.isFixedRotation = true
end

-- Ground
function createGround()
    for i = 1, 3 do
        local ground = display.newImageRect("ground.png", display.contentWidth, 40)
        ground.x = display.contentWidth * (i - 1) + display.contentWidth / 2
        ground.y = display.contentHeight - 20
        physics.addBody(ground, "static", {
            density = 1.0,
            friction = 1.0,
            bounce = 0.0,
            box = { halfWidth = display.contentWidth / 2, halfHeight = 20 }
        })
        table.insert(grounds, ground)
    end
end

-- Obstacles
function spawnObstacle()
    if gameOver then return end

    local obsType = math.random(1, 2)
    local obs

    if obsType == 1 then
        -- Low obstacle
        obs = display.newImageRect("obstacle.png", 24, 40)
        obs.y = display.contentHeight - 50
        obs.obstacleType = "low"
    else
        -- High obstacle (can slide under)
        obs = display.newImageRect("obstacle_high.png", 32, 40)
        obs.y = display.contentHeight - 100
        obs.obstacleType = "high"
    end
    obs.x = display.contentWidth + 50
    obs.name = "obstacle"
    physics.addBody(obs, "kinematic", {
        isSensor = true,
        box = { halfWidth = 12, halfHeight = 18 }
    })

    table.insert(obstacles, obs)

    -- Schedule next obstacle
    local interval = math.max(0.5, 1.5 - (score / 1000) * 0.1)
    timer.performWithDelay(interval * 1000, spawnObstacle)
end

-- Collision
function onCollision(event)
    if event.phase == "began" then
        if (event.object1 == player and event.object2.name == "obstacle") or
           (event.object2 == player and event.object1.name == "obstacle") then

            local obs = event.object1.name == "obstacle" and event.object1 or event.object2
            local other = event.object1 == player and event.object2 or event.object1

            if isSliding and obs.obstacleType == "high" then
                -- Sliding under high obstacle - safe
                return true
            end

            -- Game over
            gameOver = true
            player:setLinearVelocity(0, 0)
            player.alpha = 0.5
            saveHighScore()
            showGameOver()
        end
    end
end
Runtime:addEventListener("collision", onCollision)

function showGameOver()
    local overlay = display.newRect(
        display.contentCenterX, display.contentCenterY,
        display.contentWidth, display.contentHeight
    )
    overlay:setFillColor(0, 0, 0, 0.7)
    overlay.alpha = 0
    transition.to(overlay, { alpha = 1, time = 300 })

    display.newText("Game Over!", display.contentCenterX, display.contentCenterY - 50, nil, 48)
    display.newText("Score: " .. math.floor(score), display.contentCenterX, display.contentCenterY, nil, 28)
    display.newText("High Score: " .. highScore, display.contentCenterX, display.contentCenterY + 40, nil, 20)
    display.newText("Press SPACE to restart", display.contentCenterX, display.contentCenterY + 80, nil, 18)
end

-- Input
function handleKey(event)
    if event.keyName == "space" then
        if event.phase == "down" then
            if gameOver then
                restartGame()
            elseif canJump and not isSliding then
                player:setLinearVelocity(0, 0)
                player:applyLinearImpulse(0, -12, player.x, player.y)
                canJump = false
            end
        end
    elseif (event.keyName == "down" or event.keyName == "s") and event.phase == "down" then
        if not isSliding and not gameOver then
            isSliding = true
            player.height = 24
            physics.addBody(player, "dynamic", {
                density = 1.0,
                friction = 0.0,
                bounce = 0.0,
                box = { halfWidth = 12, halfHeight = 10 }
            })
            player.isFixedRotation = true
            timer.performWithDelay(600, function()
                isSliding = false
                player.height = 48
                physics.addBody(player, "dynamic", {
                    density = 1.0,
                    friction = 0.0,
                    bounce = 0.0,
                    box = { halfWidth = 12, halfHeight = 20 }
                })
                player.isFixedRotation = true
            end)
        end
    end
    return true
end
Runtime:addEventListener("key", handleKey)

function restartGame()
    for _, v in ipairs(obstacles) do display.remove(v) end
    for _, v in ipairs(grounds) do display.remove(v) end
    obstacles = {}
    grounds = {}
    display.remove(player)
    score = 0
    gameOver = false
    canJump = true
    isSliding = false
    initGame()
end

-- Game loop
function gameLoop()
    if gameOver then return end

    -- Score
    score = score + 1

    -- Scroll obstacles
    for i = #obstacles, 1, -1 do
        local obs = obstacles[i]
        obs.x = obs.x - scrollSpeed * (1 / 60)
        if obs.x < -100 then
            display.remove(obs)
            table.remove(obstacles, i)
        end
    end

    -- Scroll ground
    for _, ground in ipairs(grounds) do
        ground.x = ground.x - scrollSpeed * (1 / 60)
        if ground.x < -display.contentWidth / 2 then
            ground.x = ground.x + display.contentWidth * 3
        end
    end

    -- Scroll speed increases over time
    scrollSpeed = 300 + math.floor(score / 100) * 10

    -- Update HUD
    if score > highScore then highScore = score end
end
Runtime:addEventListener("enterFrame", gameLoop)

function initGame()
    loadHighScore()
    createGround()
    createPlayer()

    -- Ground collision detection
    local function onLocalCollision(event)
        if event.phase == "began" then
            if event.object1 == player or event.object2 == player then
                canJump = true
            end
        end
    end
    Runtime:addEventListener("collision", onLocalCollision)

    -- Spawn first obstacle after delay
    timer.performWithDelay(1500, spawnObstacle)
end

initGame()
