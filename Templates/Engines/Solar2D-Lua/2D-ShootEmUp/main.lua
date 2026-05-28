-- Solar2D Shoot-em-up
display.setStatusBar(display.HiddenStatusBar)

local physics = require("physics")
physics.start()
physics.setGravity(0, 0)

local player
local bullets = {}
local enemyBullets = {}
local enemies = {}
local stars = {}
local score = 0
local playerHP = 3
local lives = 3
local wave = 0
local gameOver = false
local fireRate = 0.2
local fireTimer = 0
local powerLevel = 1

-- Player
function createPlayer()
    player = display.newImageRect("ship.png", 32, 32)
    player.x = display.contentCenterX
    player.y = display.contentHeight - 80
    physics.addBody(player, "dynamic", {
        density = 1.0,
        friction = 0.0,
        bounce = 0.0,
        radius = 14
    })
    player.isFixedRotation = true
    player.moveX = 0
    player.moveY = 0
end

-- Starfield
function createStars()
    for i = 1, 50 do
        local star = display.newCircle(math.random(display.contentWidth), math.random(display.contentHeight), math.random(1, 3))
        star:setFillColor(1, 1, 1, math.random(50, 200) / 255)
        star.speed = math.random(1, 3)
        table.insert(stars, star)
    end
end

-- Bullets
function fireBullet()
    if gameOver then return end

    if powerLevel == 1 then
        local b = display.newRect(player.x, player.y - 20, 4, 12)
        b:setFillColor(0.2, 1, 0.2)
        b.speed = -8
        b.name = "bullet"
        physics.addBody(b, "dynamic", { isSensor = true })
        b.isFixedRotation = true
        table.insert(bullets, b)
    elseif powerLevel == 2 then
        for _, offset in ipairs({-10, 10}) do
            local b = display.newRect(player.x + offset, player.y - 18, 4, 12)
            b:setFillColor(0.2, 1, 0.2)
            b.speed = -8
            b.name = "bullet"
            physics.addBody(b, "dynamic", { isSensor = true })
            b.isFixedRotation = true
            table.insert(bullets, b)
        end
    else
        for _, offset in ipairs({-16, 0, 16}) do
            local b = display.newRect(player.x + offset, player.y - 20, 4, 12)
            b:setFillColor(0.2, 1, 0.2)
            b.speed = -8
            b.name = "bullet"
            physics.addBody(b, "dynamic", { isSensor = true })
            b.isFixedRotation = true
            if offset ~= 0 then b.xV = offset * 0.05 end
            table.insert(bullets, b)
        end
    end
end

-- Enemies
function spawnEnemyWave()
    wave = wave + 1
    local count = 3 + wave

    for i = 1, count do
        local ex = math.random(30, display.contentWidth - 30)
        local enemy = display.newImageRect("enemy_ship.png", 28, 28)
        enemy.x = ex
        enemy.y = -30
        enemy.hp = 1 + math.floor(wave / 3)
        enemy.scoreValue = 50 + wave * 10
        enemy.speed = 1 + math.random() * 2
        enemy.name = "enemy"
        physics.addBody(enemy, "dynamic", { isSensor = true })
        enemy.isFixedRotation = true
        table.insert(enemies, enemy)
    end

    -- Schedule next wave
    timer.performWithDelay(math.max(2000, 5000 - wave * 200), function()
        if not gameOver then spawnEnemyWave() end
    end)
end

-- Collision
function onCollision(event)
    if event.phase == "began" then
        -- Bullet hits enemy
        if (event.object1.name == "bullet" and event.object2.name == "enemy") or
           (event.object2.name == "bullet" and event.object1.name == "enemy") then

            local enemy = event.object1.name == "enemy" and event.object1 or event.object2
            local bullet = event.object1.name == "bullet" and event.object1 or event.object2

            enemy.hp = enemy.hp - 1
            display.remove(bullet)

            -- Remove from bullet list
            for i = #bullets, 1, -1 do
                if bullets[i] == bullet then table.remove(bullets, i) break end
            end

            if enemy.hp <= 0 then
                score = score + (enemy.scoreValue or 100)
                display.remove(enemy)
                for i = #enemies, 1, -1 do
                    if enemies[i] == enemy then table.remove(enemies, i) break end
                end
            end
            return true
        end

        -- Enemy hits player
        if (event.object1.name == "enemy" and event.object2 == player) or
           (event.object2.name == "enemy" and event.object1 == player) then

            local enemy = event.object1.name == "enemy" and event.object1 or event.object2

            display.remove(enemy)
            for i = #enemies, 1, -1 do
                if enemies[i] == enemy then table.remove(enemies, i) break end
            end

            playerHit()
        end
    end
end
Runtime:addEventListener("collision", onCollision)

function playerHit()
    playerHP = playerHP - 1
    if playerHP <= 0 then
        lives = lives - 1
        if lives > 0 then
            playerHP = 3
            player.alpha = 0.5
            timer.performWithDelay(1500, function() player.alpha = 1 end)
        else
            gameOver = true
            showGameOver()
        end
    end
end

function showGameOver()
    local overlay = display.newRect(
        display.contentCenterX, display.contentCenterY,
        display.contentWidth, display.contentHeight
    )
    overlay:setFillColor(0, 0, 0, 0.7)
    display.newText("Game Over!", display.contentCenterX, display.contentCenterY - 30, nil, 48)
    display.newText("Score: " .. score, display.contentCenterX, display.contentCenterY + 15, nil, 28)
    display.newText("Wave: " .. wave, display.contentCenterX, display.contentCenterY + 50, nil, 20)
    display.newText("Press SPACE to restart", display.contentCenterX, display.contentCenterY + 90, nil, 16)
end

-- Input
function handleKey(event)
    if gameOver then
        if event.keyName == "space" and event.phase == "down" then
            restartGame()
        end
        return true
    end

    if event.phase == "down" then
        if event.keyName == "right" or event.keyName == "d" then player.moveX = 5
        elseif event.keyName == "left" or event.keyName == "a" then player.moveX = -5
        elseif event.keyName == "up" or event.keyName == "w" then player.moveY = -5
        elseif event.keyName == "down" or event.keyName == "s" then player.moveY = 5
        end
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

function restartGame()
    for _, v in ipairs(bullets) do display.remove(v) end
    for _, v in ipairs(enemies) do display.remove(v) end
    for _, v in ipairs(stars) do display.remove(v) end
    for _, v in ipairs(enemyBullets) do display.remove(v) end
    bullets = {}
    enemies = {}
    stars = {}
    enemyBullets = {}
    display.remove(player)
    score = 0
    playerHP = 3
    lives = 3
    wave = 0
    powerLevel = 1
    gameOver = false
    initGame()
end

-- Game loop
function gameLoop()
    if gameOver then return end

    -- Move player
    if player then
        player.x = player.x + (player.moveX or 0)
        player.y = player.y + (player.moveY or 0)
        player.x = math.max(16, math.min(display.contentWidth - 16, player.x))
        player.y = math.max(16, math.min(display.contentHeight - 16, player.y))
    end

    -- Auto-fire
    fireTimer = fireTimer + (1 / 60)
    if fireTimer >= fireRate then
        fireBullet()
        fireTimer = 0
    end

    -- Move bullets
    for i = #bullets, 1, -1 do
        local b = bullets[i]
        b.y = b.y + (b.speed or -8)
        if b.xV then b.x = b.x + b.xV end
        if b.y < -20 then
            display.remove(b)
            table.remove(bullets, i)
        end
    end

    -- Move enemies
    for i = #enemies, 1, -1 do
        local e = enemies[i]
        e.y = e.y + (e.speed or 1)
        e.x = e.x + math.sin(e.y * 0.01) * 0.5
        if e.y > display.contentHeight + 30 then
            display.remove(e)
            table.remove(enemies, i)
        end
    end

    -- Scroll stars
    for _, star in ipairs(stars) do
        star.y = star.y + star.speed
        if star.y > display.contentHeight then
            star.y = 0
            star.x = math.random(display.contentWidth)
        end
    end

    -- Update HUD
    if not gameOver then
        display.newText("Score: " .. score .. "  Lives: " .. lives .. "  Wave: " .. wave, display.contentCenterX, 20, nil, 18)
    end
end
Runtime:addEventListener("enterFrame", gameLoop)

function initGame()
    createStars()
    createPlayer()
    timer.performWithDelay(1500, spawnEnemyWave)
end

initGame()
