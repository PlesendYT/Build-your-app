local love = love

local player = {
    x = 400, y = 500, speed = 300,
    radius = 20, color = {0.2, 0.6, 1.0}
}

local bullets = {}
local enemies = {}
local score = 0
local spawnTimer = 0
local gameOver = false

local function spawnEnemy()
    table.insert(enemies, {
        x = math.random(50, 750),
        y = -30,
        speed = 100 + math.random(50),
        radius = 15,
        color = {1.0, 0.3, 0.3}
    })
end

function love.load()
    love.window.setTitle("LÖVE Game")
    love.window.setMode(800, 600)
    love.graphics.setBackgroundColor(0.05, 0.05, 0.15)
end

function love.update(dt)
    if gameOver then return end

    -- Player movement
    if love.keyboard.isDown("left") then player.x = player.x - player.speed * dt end
    if love.keyboard.isDown("right") then player.x = player.x + player.speed * dt end
    if love.keyboard.isDown("up") then player.y = player.y - player.speed * dt end
    if love.keyboard.isDown("down") then player.y = player.y + player.speed * dt end

    -- Clamp
    player.x = math.max(player.radius, math.min(800 - player.radius, player.x))
    player.y = math.max(player.radius, math.min(600 - player.radius, player.y))

    -- Update bullets
    for i = #bullets, 1, -1 do
        bullets[i].y = bullets[i].y - 400 * dt
        if bullets[i].y < 0 then
            table.remove(bullets, i)
        end
    end

    -- Spawn enemies
    spawnTimer = spawnTimer + dt
    if spawnTimer > 1.0 then
        spawnEnemy()
        spawnTimer = 0
    end

    -- Update enemies & collision
    for i = #enemies, 1, -1 do
        enemies[i].y = enemies[i].y + enemies[i].speed * dt
        if enemies[i].y > 650 then
            table.remove(enemies, i)
        else
            -- Bullet-enemy collision
            local enemy = enemies[i]
            for j = #bullets, 1, -1 do
                local dx = bullets[j].x - enemy.x
                local dy = bullets[j].y - enemy.y
                if dx * dx + dy * dy < (enemy.radius + 5) ^ 2 then
                    table.remove(bullets, j)
                    table.remove(enemies, i)
                    score = score + 10
                    break
                end
            end
        end
    end

    -- Player-enemy collision
    for _, enemy in ipairs(enemies) do
        local dx = player.x - enemy.x
        local dy = player.y - enemy.y
        if dx * dx + dy * dy < (player.radius + enemy.radius) ^ 2 then
            gameOver = true
            break
        end
    end
end

function love.draw()
    -- Player
    love.graphics.setColor(player.color)
    love.graphics.circle("fill", player.x, player.y, player.radius)

    -- Bullets
    love.graphics.setColor(1, 1, 0)
    for _, b in ipairs(bullets) do
        love.graphics.circle("fill", b.x, b.y, 4)
    end

    -- Enemies
    for _, e in ipairs(enemies) do
        love.graphics.setColor(e.color)
        love.graphics.circle("fill", e.x, e.y, e.radius)
    end

    -- Score
    love.graphics.setColor(1, 1, 1)
    love.graphics.print("Score: " .. score, 10, 10)

    if gameOver then
        love.graphics.setColor(1, 0, 0)
        love.graphics.printf("GAME OVER", 0, 250, 800, "center")
        love.graphics.printf("Score: " .. score, 0, 300, 800, "center")
        love.graphics.printf("Drücke R für Neustart", 0, 350, 800, "center")
    end
end

function love.keypressed(key)
    if key == "escape" then
        love.event.quit()
    end
    if key == "space" and not gameOver then
        table.insert(bullets, {x = player.x, y = player.y})
    end
    if key == "r" and gameOver then
        gameOver = false
        bullets = {}
        enemies = {}
        score = 0
        spawnTimer = 0
        player.x = 400
        player.y = 500
    end
end

function love.mousepressed(x, y, button)
    if button == 1 and not gameOver then
        table.insert(bullets, {x = player.x, y = player.y})
    end
end
